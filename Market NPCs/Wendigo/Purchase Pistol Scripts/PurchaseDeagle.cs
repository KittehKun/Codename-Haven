
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseDeagle : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public int DeaglePrice; //Assigned in Unity
    private AudioSource buySFX; //Assigned in Start()
    private AudioSource errorSFX; //Assigned in Start()
    void Start()
    {
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();

        //Find errorSFX in the PlayerScriptsContainer and the PlayerUIError GameObject
        this.errorSFX = GameObject.Find("PlayerScriptsContainer").transform.Find("PlayerErrorAudio").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= DeaglePrice && playerInventory.PlayerOwnedPistols[4] < 5)
       {
        PurchaseWeapon();
       }
       else
       {
        Debug.Log("User attempted to buy gun but do not have enough money.");
        errorSFX.Play();
       }
    }

    private void PurchaseWeapon()
    {
        //Remove money from player and add one to the pistol counter
        playerMoney.PlayerMoney -= DeaglePrice;
        playerInventory.PlayerOwnedPistols[4]++; //Add one to pistol counter

        //Update the money counter and play the buy sound
        PlayerVRHUD.UpdateMoneyCounter(playerMoney.PlayerMoney);

        //Play the buy sound
        buySFX.Play();

        //Print Debug Statement
        Debug.Log($"Player bought Deagle! Player now has {playerInventory.PlayerOwnedPistols[4]} Deagles.");
    }
}
