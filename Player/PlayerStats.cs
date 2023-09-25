﻿
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerStats : UdonSharpBehaviour
{
    public int PlayerMoney{get; set;} = 10000; //PlayerMoney | Used for storing player's money
    public GameObject[] traderGUIs; //Assigned in Unity | Array needed for changing all shopkeeper displays on startup
    public int PlayerHealth; //Default Health Value
    public int MaximumHealth {get; private set;} = 125; //Default Max Health Value
    public PlayerRaidManager playerRaidManager; //PlayerRaidManager | Assigned in Unity | Used for resetting player inventory
    void Start()
    {
        UpdateMenuMoneyGUI();

        PlayerHealth = MaximumHealth;
        PlayerVRHUD.UpdateHPCount(PlayerHealth, MaximumHealth);
        PlayerVRHUD.UpdateMoneyCounter(PlayerMoney);
    }

    public void TakeDamage(int damageAmount)
    {
        PlayerHealth -= damageAmount;
        Debug.Log($"Player took {damageAmount} damage! Player health is now {PlayerHealth}.");

        //Update HP counter for both desktop and VR
        PlayerVRHUD.UpdateHPCount(PlayerHealth, MaximumHealth);

        //Check to see if player is dead
        if(PlayerHealth <= 0)
        {
            //Kill player
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead!");
        playerRaidManager.ResetPlayer(true);
    }

    //Gets PlayerMoney from Player
    public int GetPlayerMoney()
    {
        return PlayerMoney;
    }

    //Method to add money to player
    public void AddMoney(int amount)
    {
        PlayerMoney += amount;
    }

    /// <summary>
    /// Method is used for updating all GUIs that display the player's money.
    /// </summary>
    public void UpdateMenuMoneyGUI()
    {
        foreach(GameObject traderScreen in traderGUIs)
        {
            
            traderScreen.GetComponent<Text>().text = $"${PlayerMoney}";
        }
    }

    //This method will set the maximum health to a new value
    public void SetMaximumHealth(int newMaxHealth)
    {
        MaximumHealth = newMaxHealth;
    }

}
