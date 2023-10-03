
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerStats : UdonSharpBehaviour
{
    public int PlayerMoney{get; set;} = 1000; //PlayerMoney | Used for storing player's money
    //public GameObject[] traderGUIs; //Assigned in Unity | Array needed for changing all shopkeeper displays on startup
    public int PlayerHealth; //Default Health Value
    public int MaximumHealth {get; private set;} = 125; //Default Max Health Value
    public int PlayerLevel {get; set;} = 1; //Default Player Level | Earned by killing enemies and completing tasks
    public int PlayerXP {get; set;} = 0; //Default Player XP | Earned by killing enemies and completing tasks
    public int XPToNextLevel = 100; //Default XP needed to level up | Increases by 100 each level
    public PlayerRaidManager playerRaidManager; //PlayerRaidManager | Assigned in Unity | Used for resetting player inventory
    void Start()
    {
        //UpdateMenuMoneyGUI();

        PlayerHealth = MaximumHealth;
        PlayerVRHUD.UpdateHPCount(PlayerHealth, MaximumHealth);
        PlayerVRHUD.UpdateMoneyCounter(PlayerMoney);
        PlayerVRHUD.UpdateXPBar(PlayerXP);
        PlayerVRHUD.UpdateXPToNextLevel(XPToNextLevel);
        PlayerVRHUD.UpdateLevel(PlayerLevel);
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
        
        //Display death screen
        PlayerVRHUD.ShowDeathScreen();

        this.SendCustomEventDelayedSeconds("TriggerHideDeathScreen", 7f); //Hide death screen after 7 seconds as audio clip is 7 seconds
    }

    public void TriggerHideDeathScreen()
    {
        PlayerVRHUD.HideDeathScreen();
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

    //This method will set the maximum health to a new value
    public void SetMaximumHealth(int newMaxHealth)
    {
        MaximumHealth = newMaxHealth;
    }

    public void AddXP(int xpToAdd)
    {
        PlayerXP += xpToAdd;
        PlayerVRHUD.UpdateXPBar(xpToAdd);

        //Check if player has enough XP to level up
        if(PlayerXP >= XPToNextLevel)
        {
            LevelUp();
            //Reset XP bar to 0
            PlayerVRHUD.UpdateXPBar(-XPToNextLevel);
        }
    }

    private void LevelUp()
    {
        PlayerLevel++;
        PlayerVRHUD.UpdateLevel(PlayerLevel);
        PlayerVRHUD.UpdateXPBar(-XPToNextLevel);
        XPToNextLevel += 100;
        PlayerXP = 0;
        PlayerVRHUD.UpdateXPToNextLevel(XPToNextLevel);
    }
}
