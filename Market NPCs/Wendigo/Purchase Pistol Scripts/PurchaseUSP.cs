
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseUSP : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public int USPPrice; //Assigned in Unity
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
       if(playerMoney.PlayerMoney >= USPPrice && playerInventory.PlayerOwnedPistols[3] < 5)
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
        playerMoney.PlayerMoney -= USPPrice;
        playerInventory.PlayerOwnedPistols[3]++; //Add one to pistol counter

        //Update the money counter and play the buy sound
        PlayerVRHUD.UpdateMoneyCounter(playerMoney.PlayerMoney);
        buySFX.Play();

        //Print Debug Statement
        Debug.Log($"Player bought USP! Player now has {playerInventory.PlayerOwnedPistols[3]} USPs.");
    }
}
