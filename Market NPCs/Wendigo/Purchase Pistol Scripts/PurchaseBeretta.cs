
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseBeretta : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public int BerettaPrice; //Assigned in Unity
    private AudioSource buySFX; //Assigned in Start()
    void Start()
    {
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= BerettaPrice && playerInventory.PlayerOwnedPistols[2] < 5)
       {
        PurchaseWeapon();
       }
       else
       {
        Debug.Log("User attempted to purchase gun but cannot afford it or stash is full!");
       }
    }

    private void PurchaseWeapon()
    {
        //Remove money from player and add one to the pistol counter
        playerMoney.PlayerMoney -= BerettaPrice;
        playerInventory.PlayerOwnedPistols[2]++; //Add one to pistol counter

        //Update the money counter and play the buy sound
        PlayerVRHUD.UpdateMoneyCounter(playerMoney.PlayerMoney);
        buySFX.Play();

        //Print Debug Statement
        Debug.Log($"Player bought Beretta! Player now has {playerInventory.PlayerOwnedPistols[2]} Berettas.");
    }
}
