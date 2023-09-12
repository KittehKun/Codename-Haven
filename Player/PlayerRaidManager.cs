﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerRaidManager : UdonSharpBehaviour
{
    private PlayerRaidInventory raidInventory; //PlayerRaidInventory taken from GameObject | Should be attached to GameObject from this script
    private PlayerStats playerStats; //PlayerStats taken from parent GameObject | Should be within the PlayerScriptsContainer GameObject
    private AudioSource[] audioSources; //Array of audio sources | Used for playing victory sound
    private bool isInRaid = false; //Used for checking if player is in raid
    private Transform[] lootConatiners; //Array of loot containers | Used for resetting loot containers on extraction
    public Transform[] extractionPoints; //Array of extraction points | Assigned in Unity | Used for enabling extraction points on raid end | ARRAY IS GRABBED FROM ExtractPlayer script

    void Start()
    {
        //Get PlayerRaidInventory from GameObject
        raidInventory = this.GetComponent<PlayerRaidInventory>();
        //Get PlayerStats from parent GameObject
        playerStats = this.transform.parent.Find("PlayerStats").GetComponent<PlayerStats>();
        //Get audio sources from children GameObjects
        audioSources = this.GetComponentsInChildren<AudioSource>();
        //Initialize loot containers array based on childCount
        lootConatiners = new Transform[GameObject.Find("_LOOTCONTAINERS").transform.childCount]; //Expected: New array with length of childCount
        //Fill loot containers array with all loot containers
        for (int i = 0; i < lootConatiners.Length; i++)
        {
            lootConatiners[i] = GameObject.Find("_LOOTCONTAINERS").transform.GetChild(i);
        }
    }

    //Method is used for adding RaidWallet money to the player's inventory on successful extraction
    public void AddRaidWalletToInventory()
    {
        //Add RaidWallet to player's money
        Debug.Log($"Player extracted! Adding {raidInventory.GetCurrentRaidWallet()} to player's money.");
        playerStats.AddMoney(raidInventory.GetCurrentRaidWallet());
        //Reset RaidWallet
        ResetPlayer(false);

        //Update Menu GUI with new money value | Includes shopkeepers and Main Menu
        playerStats.UpdateMenuMoneyGUI();

        PlayVictorySound();
    }

    /// <summary>
    /// Method is used for restting the player's stats when they die. Method is private as it shold only be called from this script.
    /// </summary>
    public void ResetPlayer(bool playerDeath)
    {
        Debug.Log("Player is being reset! Resetting StoragePoints and RaidWallet.");
        //Reset RaidWallet
        raidInventory.ResetRaidWallet();
        Debug.Log($"Reset RaidWallet to {raidInventory.GetCurrentRaidWallet()}");
        //Reset StoragePoints
        raidInventory.ResetStoragePoints();
        Debug.Log($"Reset StoragePoints to {raidInventory.StoragePoints}");
        
        int playerHealth; //Variable used to assign player health based on if player died or extracted
        //Reset PlayerHealth back to full if player extracted successfully
        if(playerDeath)
        {
            Debug.Log("Player died! Resetting PlayerHealth to 150.");
            playerHealth = 100; //Player must heal back to 150 if they died in raid
        } else
        {
            Debug.Log("Player extracted! Resetting PlayerHealth to 150.");
            playerHealth = 150;
        }

        //Reset HUD
        PlayerHUD.UpdateSPCount(raidInventory.StoragePoints);
        PlayerHUD.UpdateHPCount(playerHealth);
        PlayerVRHUD.UpdateSPCount(raidInventory.StoragePoints);
        PlayerVRHUD.UpdateHPCount(playerHealth);

        //Reset loot containers
        ResetLootContainers();
    }

    private void PlayVictorySound()
    {
        //Play victory sound
        Debug.Log("Playing victory sound.");
        audioSources[Random.Range(0, audioSources.Length)].Play();
    }

    private void ResetLootContainers()
    {
        //Get all loot containers
        for (int i = 0; i < GameObject.Find("_LOOTCONTAINERS").transform.childCount; i++)
        {
            //Get the UdonBehavior script of each object and set the collider to true and the isLooted bool to false
            this.lootConatiners[i].GetComponent<UdonBehaviour>().SetProgramVariable("isLooted", false);
            //Set collider to true
            this.lootConatiners[i].GetComponent<Collider>().enabled = true;

            //Play "OpenContainer" animation with -1 speed of objects with "hasOpenAnimation" bool set to true
            if (this.lootConatiners[i].GetComponent<UdonBehaviour>().GetProgramVariable("hasOpenAnimation").Equals(true))
            {
                //Play "CloseContainer" animation state
                this.lootConatiners[i].GetComponent<Animator>().Play("CloseContainer");
            }

            //Remove all objects from the _SPAWNEDLOOT GameObject
            for (int j = 0; j < GameObject.Find("_SPAWNEDLOOT").transform.childCount; j++)
            {
                //Destroy all objects in the _SPAWNEDLOOT GameObject
                Destroy(GameObject.Find("_SPAWNEDLOOT").transform.GetChild(j).gameObject);
            }
        }
    }
}