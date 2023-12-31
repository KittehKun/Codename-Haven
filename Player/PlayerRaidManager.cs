﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerRaidManager : UdonSharpBehaviour
{
    private PlayerRaidInventory raidInventory; //PlayerRaidInventory taken from GameObject | Should be attached to GameObject from this script
    private PlayerStats playerStats; //PlayerStats taken from parent GameObject | Should be within the PlayerScriptsContainer GameObject
    public AudioSource[] audioSources; //Array of audio sources | Used for playing victory sound | Assigned in Unity
    public AudioSource[] bigAudioSources; //Array of audio sources | Used for playing big victory sound | Assigned in Unity due to audio being special
    private AudioSource deathSFX; //Death sound | Assigned in Unity
    public bool isInRaid = false; //Used for checking if player is in raid
    private Transform[] lootConatiners; //Array of loot containers | Used for resetting loot containers on extraction
    public Transform[] extractionPoints; //Array of extraction points | Assigned in Unity | Used for enabling extraction points on raid end | ARRAY IS GRABBED FROM ExtractPlayer script
    public Transform playerSpawnPoint; //Player spawn point | Assigned in Unity | Used for teleporting player to spawn point on death
    public PlayerRig playerRig; //PlayerRig | Assigned in Unity | Used for returning weapon to pool on death
    public Leaderboard leaderboard; //Leaderboard | Assigned in Unity | Used for updating leaderboard on extraction

    void Start()
    {
        //Get PlayerRaidInventory from GameObject
        raidInventory = this.GetComponent<PlayerRaidInventory>();
        //Get PlayerStats from parent GameObject
        playerStats = this.transform.parent.Find("PlayerStats").GetComponent<PlayerStats>();
        //Initialize loot containers array based on childCount
        lootConatiners = new Transform[GameObject.Find("_LOOTCONTAINERS").transform.childCount]; //Expected: New array with length of childCount
        //Fill loot containers array with all loot containers
        for (int i = 0; i < lootConatiners.Length; i++)
        {
            lootConatiners[i] = GameObject.Find("_LOOTCONTAINERS").transform.GetChild(i);
        }
        //Assign DeathSFX
        deathSFX = GameObject.Find("PlayerDeathSFX").GetComponent<AudioSource>();
    }

    //Method is used for adding RaidWallet money to the player's inventory on successful extraction
    public void AddRaidWalletToInventory()
    {
        //Add RaidWallet to player's money
        Debug.Log($"Player extracted! Adding {raidInventory.GetCurrentRaidWallet()} to player's money.");
        playerStats.AddMoney(raidInventory.GetCurrentRaidWallet());
        
        //Update Menu GUI with new money value | Includes shopkeepers and Main Menu
        //playerStats.UpdateMenuMoneyGUI(); //This code is redundant due to VR update method
        PlayerVRHUD.UpdateMoneyCounter(playerStats.PlayerMoney);

        //Check if the player extracted with more than 1000 dollars
        if(raidInventory.GetCurrentRaidWallet() >= 100)
        {
            //Play big victory sound
            PlayBigVictorySound();
        } else
        {
            //Play victory sound
            PlayVictorySound();
        }
        
        //Reset RaidWallet
        ResetPlayer(false);
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
        
        //Reset PlayerHealth back to full if player extracted successfully
        if(playerDeath)
        {
            Debug.Log("Player died! Resetting PlayerHealth to 125 & MaximumHealth back to default.");
            playerStats.PlayerHealth = 50;
            playerStats.SetMaximumHealth(125);

            //Reset player rig
            playerRig.ReturnWeaponToPool();

            //Play death sound
            Debug.Log("Playing death sound.");
            deathSFX.Play();

            //Reset Storage Points back to Default
            raidInventory.StoragePointCap = 25;
        }

        //Reset Storage Points to maximum
        raidInventory.ResetStoragePoints();

        //Reset HUD
        PlayerVRHUD.UpdateSPCount(raidInventory.StoragePoints, raidInventory.StoragePointCap);
        PlayerVRHUD.UpdateHPCount(playerStats.PlayerHealth, playerStats.MaximumHealth);
        PlayerVRHUD.UpdateMoneyCounter(playerStats.PlayerMoney);

        //Reset loot containers
        ResetLootContainers();

        //Reset opened safes
        ResetOpenSafes();

        //Teleport Player to spawn point
        Networking.LocalPlayer.TeleportTo(playerSpawnPoint.position, playerSpawnPoint.rotation);

        //Update the leaderboard
        leaderboard.UpdateLeaderboard(playerStats.PlayerMoney);
    }

    private void PlayVictorySound()
    {
        //Play victory sound
        Debug.Log("Playing victory sound.");
        audioSources[Random.Range(0, audioSources.Length)].Play();
    }

    private void PlayBigVictorySound()
    {
        //Play big victory sound
        Debug.Log("Playing big victory sound.");
        bigAudioSources[Random.Range(0, bigAudioSources.Length)].Play();
    }

    private void ResetLootContainers()
    {
        //Chance variable to roll 30% chance for loot container to not be reset
        int chance;

        //Get all loot containers
        for (int i = 0; i < GameObject.Find("_LOOTCONTAINERS").transform.childCount; i++)
        {
            //Roll for chance
            chance = Random.Range(0, 100);

            if(chance <= 30)
            {
                //If the chance is less than or equal to 30, do not reset the loot container
                continue;
            }
            
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

    private void ResetOpenSafes()
    {
        //Create a 60% chance for a safe to not be reset
        int chance;
        
        //Get all safes
        for (int i = 0; i < GameObject.Find("_SAFES").transform.childCount; i++)
        {
            //Roll for chance
            chance = Random.Range(0, 100);

            //If the chance is less than or equal to 60, do not reset the safe
            if(chance <= 60)
            {
                continue;
            }
            
            //Get the UdonBehavior script of each object and set the collider to true and the isLooted bool to false
            GameObject.Find("_SAFES").transform.GetChild(i).GetComponent<UdonBehaviour>().SetProgramVariable("isLooted", false);
            //Set collider to true
            GameObject.Find("_SAFES").transform.GetChild(i).GetComponent<Collider>().enabled = true;

            //Play "CloseSafe" animation state
            GameObject.Find("_SAFES").transform.GetChild(i).GetComponent<Animator>().Play("CloseSafe");

            //Remove all objects from the _SPAWNEDLOOT GameObject
            for (int j = 0; j < GameObject.Find("_SPAWNEDLOOT").transform.childCount; j++)
            {
                //Destroy all objects in the _SPAWNEDLOOT GameObject
                Destroy(GameObject.Find("_SPAWNEDLOOT").transform.GetChild(j).gameObject);
            }
        }
    }
}
