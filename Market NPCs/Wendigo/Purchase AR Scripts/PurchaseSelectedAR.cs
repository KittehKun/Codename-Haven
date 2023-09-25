
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseSelectedAR : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    
    public int SelectedAR; //Assigned in Unity | Value will be used to select index from array
    private int[] ARPrices; // M4 | AK | G36 | LVOA | AUG | M16

    private AudioSource buySFX; //Assigned in Start()

    void Start()
    {
        ARPrices = new int[] {750 , 600, 700, 1250, 750, 425};
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= ARPrices[SelectedAR] && playerInventory.PlayerOwnedARs[SelectedAR] < 5)
       {
        //Remove money from player and add one to the AR counter
        playerMoney.PlayerMoney -= ARPrices[SelectedAR];
        playerInventory.PlayerOwnedARs[SelectedAR]++; //Add one to AR counter

        //Play the buy sound
        buySFX.Play();

        //Update the money counter
        PlayerVRHUD.UpdateMoneyCounter(playerMoney.PlayerMoney);
       }
       else
       {
        Debug.Log("User attempted to buy gun but does not have enough money or their stash is full.");
       }
    }
}
