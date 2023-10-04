
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BuyBackpack : UdonSharpBehaviour
{
    public int BackpackCost; //Assigned in Unity based on Backpack GameObject | Used for calculating the cost of a backpack | 1 = $800, 2 = $1600, 3 = $3500
    public PlayerStats playerStats; //Assigned in Unity | Used for calculating a player's money
    public PlayerRaidInventory playerRaidInventory; //Assigned in Unity | Used for calculating a player's storage points
    public AudioSource buySFX; //Assigned in Unity | Used for playing the buy sound effect

    public override void Interact()
    {
        switch(BackpackCost)
        {
            case 0:
                //Check if the player has enough money to buy the backpack
                if(playerStats.PlayerMoney >= 800 && playerRaidInventory.StoragePointCap <= 25 || !playerStats.HasBackpack)
                {
                    //Give the player a backpack
                    playerStats.HasBackpack = true; //Give the player a backpack
                    playerStats.PlayerMoney -= 800; //Remove the cost of the backpack from the player's money
                    playerRaidInventory.StoragePointCap += 10; //Increase the player's storage point cap by 10
                    playerRaidInventory.StoragePoints = playerRaidInventory.StoragePointCap; //Increase the player's storage points by 10

                    //Play the buy sound effect
                    buySFX.Play();

                    //Update HUD
                    PlayerVRHUD.UpdateMaxSPCount(playerRaidInventory.StoragePoints, playerRaidInventory.StoragePointCap); //Update the player's storage point counter
                    PlayerVRHUD.UpdateMoneyCounter(playerStats.PlayerMoney); //Update the player's money counter
                }
                break;
            case 1:
                //Check if the player has enough money to buy the backpack
                if(playerStats.PlayerMoney >= 1600 && playerRaidInventory.StoragePointCap <= 35 || !playerStats.HasBackpack)
                {
                    //Give the player a backpack
                    playerStats.HasBackpack = true; //Give the player a backpack
                    playerStats.PlayerMoney -= 1600; //Remove the cost of the backpack from the player's money
                    playerRaidInventory.StoragePointCap += 10; //Increase the player's storage point cap by 10
                    playerRaidInventory.StoragePoints = playerRaidInventory.StoragePointCap; //Increase the player's storage points by 10

                    //Play the buy sound effect
                    buySFX.Play();

                    //Update HUD
                    PlayerVRHUD.UpdateMaxSPCount(playerRaidInventory.StoragePoints, playerRaidInventory.StoragePointCap); //Update the player's storage point counter
                    PlayerVRHUD.UpdateMoneyCounter(playerStats.PlayerMoney); //Update the player's money counter
                }
                break;
            case 2:
                //Check if the player has enough money to buy the backpack
                if(playerStats.PlayerMoney >= 3500 && playerRaidInventory.StoragePointCap <= 50 || !playerStats.HasBackpack)
                {
                    //Give the player a backpack
                    playerStats.HasBackpack = true; //Give the player a backpack
                    playerStats.PlayerMoney -= 3500; //Remove the cost of the backpack from the player's money
                    playerRaidInventory.StoragePointCap += 15; //Increase the player's storage point cap by 15
                    playerRaidInventory.StoragePoints = playerRaidInventory.StoragePointCap; //Increase the player's storage points by 15

                    //Play the buy sound effect
                    buySFX.Play();

                    //Update HUD
                    PlayerVRHUD.UpdateMaxSPCount(playerRaidInventory.StoragePoints, playerRaidInventory.StoragePointCap); //Update the player's storage point counter
                    PlayerVRHUD.UpdateMoneyCounter(playerStats.PlayerMoney); //Update the player's money counter
                }
                break;
            default:
                Debug.LogError("Kitteh your code is broken and needs to be fixed.");
            break;
        }
    }
}
