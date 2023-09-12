
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseSelectedShotgun : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    
    public int SelectedShotgun; //Assigned in Unity | Value will be used to select index from array
    private int[] ShotgunPrices; // M590A1 | Double Barrel | M1014 | SPAS 12 | AA12

    void Start()
    {
        ShotgunPrices = new int[] {300 , 150, 450, 700, 750};
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= ShotgunPrices[SelectedShotgun] && playerInventory.PlayerOwnedShotguns[SelectedShotgun] < 5)
       {
        playerMoney.PlayerMoney -= ShotgunPrices[SelectedShotgun];
        playerInventory.PlayerOwnedShotguns[SelectedShotgun]++; //Add one to shotgun counter
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
       }
       else
       {
        Debug.Log("User attempted to buy gun but do not have enough money or their stash is full.");
       }
    }
}
