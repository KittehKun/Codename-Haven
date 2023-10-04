
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LootItem : UdonSharpBehaviour
{
    public int ItemPrice; //Assigned in Unity
    public int StorageSlotCost; //Assigned in Unity | Used for calculating if the player has enough space for item
    public PlayerRaidInventory playerRaidInventory; //PlayerRaidInventory from PlayerScriptsContainer GameObject
    
    public override void Interact()
    {
        //Check if player has enough storage points to claim item
        if(playerRaidInventory.StoragePoints < StorageSlotCost)
        {
            Debug.Log("Player cannot pickup item because player does not have enough storage points.");
        } else
        {
            //Subtract storage cost of item and add the item's value to the raid wallet
            playerRaidInventory.StoragePoints -= StorageSlotCost;
            playerRaidInventory.AddToRaidWallet(ItemPrice);
            Debug.Log($"$Player picked up item successfully. They now have {playerRaidInventory.StoragePoints}.");
            Debug.Log("Removing item.");
            PlayerVRHUD.UpdateSPCount(playerRaidInventory.StoragePoints, playerRaidInventory.StoragePointCap);
            Destroy(this.transform.gameObject);
        }
    }
}
