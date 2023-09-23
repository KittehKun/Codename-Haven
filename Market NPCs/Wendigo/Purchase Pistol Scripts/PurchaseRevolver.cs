
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseRevolver : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int RevolverPrice; //Assigned in Unity
    private AudioSource buySFX; //Assigned in Start()
    void Start()
    {
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= RevolverPrice && playerInventory.PlayerOwnedPistols[5] < 5)
       {
        PurchaseWeapon();
       }
       else
       {
        Debug.Log("User attempted to buy gun but do not have enough money.");
       }
    }

    private void PurchaseWeapon()
    {
        //Remove money from player and add one to the pistol counter
        playerMoney.PlayerMoney -= RevolverPrice;
        playerInventory.PlayerOwnedPistols[5]++; //Add one to pistol counter

        //Update the money counter and play the buy sound
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
        buySFX.Play();

        //Print Debug Statement
        Debug.Log($"Player bought Revolver! Player now has {playerInventory.PlayerOwnedPistols[5]} Revolvers.");
    }
}
