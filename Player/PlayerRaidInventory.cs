
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerRaidInventory : UdonSharpBehaviour
{
    //Players receive a set amount of storage points each raid. Players cannot discard storage points however on raid end storage points are reset.
    //This circumvents the need for an inventory system when in raid
    public int StoragePoints; //Assigned in Unity | Used for calculating a player's storage points
    private int StoragePointCap; //Used for calculating maximum storage points. Above variable is used for current points.
    
    //RaidWallet will be used to track all items that a player grabs in raid. Each valueable will be auto-sold at the end of each raid and RaidWallet will be added to PlayerStats on raid end.
    private int RaidWallet {get; set;} //Used in place of inventory system

    void Start()
    {
        //Set StoragePointCap to 20 | By default players will have 20 points without a backpack | StoragePoints set in Unity should be 20
        StoragePointCap = StoragePoints;
        //Set RaidWallet to 0
        RaidWallet = 0;
    }
    
    /// <summary>
    /// Adds money to the current raid wallet. Takes the item's value as items are assumed to be auto sold on raid end.
    /// </summary>
    public void AddToRaidWallet(int ItemValue)
    {
        this.RaidWallet += ItemValue;
    }

    /// <summary>
    /// Returns the current raid wallet value.
    /// </summary>
    public int GetCurrentRaidWallet()
    {
        return this.RaidWallet;
    }

    /// <summary>
    /// Resets raid wallet back to 0. Used on raid-end.
    /// </summary>
    public void ResetRaidWallet()
    {
        this.RaidWallet = 0;
    }

    /// <summary>
    /// Resets storage points back to 0. Used on raid-end.
    /// </summary>
    public void ResetStoragePoints()
    {
        this.StoragePoints = 20;
    }

}
