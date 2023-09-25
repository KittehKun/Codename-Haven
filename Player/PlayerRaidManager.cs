
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
    public bool isInRaid = false; //Used for checking if player is in raid
    private Transform[] lootConatiners; //Array of loot containers | Used for resetting loot containers on extraction
    public Transform[] extractionPoints; //Array of extraction points | Assigned in Unity | Used for enabling extraction points on raid end | ARRAY IS GRABBED FROM ExtractPlayer script
    public Transform playerSpawnPoint; //Player spawn point | Assigned in Unity | Used for teleporting player to spawn point on death

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
    }

    //Method is used for adding RaidWallet money to the player's inventory on successful extraction
    public void AddRaidWalletToInventory()
    {
        //Add RaidWallet to player's money
        Debug.Log($"Player extracted! Adding {raidInventory.GetCurrentRaidWallet()} to player's money.");
        playerStats.AddMoney(raidInventory.GetCurrentRaidWallet());
        
        //Update Menu GUI with new money value | Includes shopkeepers and Main Menu
        playerStats.UpdateMenuMoneyGUI();
        PlayerVRHUD.UpdateMoneyCounter(playerStats.PlayerMoney);

        //Check if the player extracted with more than 500 dollars
        if(raidInventory.GetCurrentRaidWallet() >= 500)
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
        //Reset StoragePoints
        raidInventory.ResetStoragePoints();
        Debug.Log($"Reset StoragePoints to {raidInventory.StoragePoints}");
        
        //Reset PlayerHealth back to full if player extracted successfully
        if(playerDeath)
        {
            Debug.Log("Player died! Resetting PlayerHealth to 125 & MaximumHealth back to default.");
            playerStats.PlayerHealth = 100;
            playerStats.SetMaximumHealth(125);
        } else
        {
            Debug.Log("Player extracted! Resetting PlayerHealth to MaximumHealth.");
            playerStats.PlayerHealth = playerStats.MaximumHealth;
        }

        //Reset HUD
        PlayerVRHUD.UpdateSPCount(raidInventory.StoragePoints);
        PlayerVRHUD.UpdateHPCount(playerStats.PlayerHealth, playerStats.MaximumHealth);
        
        PlayerVRHUD.UpdateMoneyCounter(playerStats.PlayerMoney);

        //Reset loot containers
        ResetLootContainers();

        //Reset opened safes
        ResetOpenSafes();

        //Teleport Player to spawn point
        Networking.LocalPlayer.TeleportTo(playerSpawnPoint.position, playerSpawnPoint.rotation);
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

    private void ResetOpenSafes()
    {
        //Get all safes
        for (int i = 0; i < GameObject.Find("_SAFES").transform.childCount; i++)
        {
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
