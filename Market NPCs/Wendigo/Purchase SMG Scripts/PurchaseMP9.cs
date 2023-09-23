
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseMP9 : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int MP9Price; //Assigned in Unity
    private AudioSource buySFX; //Assigned in Start()
    void Start()
    {
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= MP9Price && playerInventory.PlayerOwnedSMGs[2] < 5)
       {
        //Remove money from player and add one to the pistol counter
        playerMoney.PlayerMoney -= MP9Price;
        playerInventory.PlayerOwnedSMGs[2]++; //Add one to pistol counter

        //Update the money counter and play the buy sound
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";

        //Play Buy SFX
        buySFX.Play();
       }
       else
       {
        Debug.Log("User attempted to buy gun but do not have enough money or their stash if full.");
       }
    }
}
