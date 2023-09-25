
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseMP5 : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public int MP5Price; //Assigned in Unity
    private AudioSource buySFX; //Assigned in Start()
    void Start()
    {
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= MP5Price && playerInventory.PlayerOwnedSMGs[1] < 5)
       {
        //Remove money from player and add one to the pistol counter
        playerMoney.PlayerMoney -= MP5Price;
        playerInventory.PlayerOwnedSMGs[1]++; //Add one to pistol counter

        //Update the money counter and play the buy sound
        PlayerVRHUD.UpdateMoneyCounter(playerMoney.PlayerMoney);

        //Play Buy SFX
        buySFX.Play();
       }
       else
       {
        Debug.Log("User attempted to buy gun but do not have enough money or their stash if full.");
       }
    }
}
