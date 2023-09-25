
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to mimic the LootItem.cs script, but instead of adding value to RaidWallet it will add a key to the player's inventory
public class LootSafeKey : UdonSharpBehaviour
{
    public PlayerInventory playerInventory; //Assigned in Unity | Used for adding a key to the player's inventory

    public override void Interact()
    {
        //Add key to player's inventory
        playerInventory.AddSafeKey();
        Debug.Log("Player picked up key successfully.");
        Debug.Log("Removing key.");

        //Destroy the key object
        Destroy(this.transform.gameObject);
    }
}
