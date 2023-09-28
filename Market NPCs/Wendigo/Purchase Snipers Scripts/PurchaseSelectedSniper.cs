
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseSelectedSniper : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity    
    public int SelectedSniper; //Assigned in Unity | Value will be used to select index from array
    private int[] SniperPrices; // HuntingRifle | Winchester | SVD | AWP | RSASS | 50Cal

    private AudioSource buySFX; //Assigned in Start()
    private AudioSource errorSFX; //Assigned in Start()

    void Start()
    {
        SniperPrices = new int[] {500 , 600, 700, 4750, 5000, 7500};
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();

        //Find errorSFX in the PlayerScriptsContainer and the PlayerUIError GameObject
        this.errorSFX = GameObject.Find("PlayerScriptsContainer").transform.Find("PlayerErrorAudio").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= SniperPrices[SelectedSniper] && playerInventory.PlayerOwnedSnipers[SelectedSniper] < 5)
       {
        playerMoney.PlayerMoney -= SniperPrices[SelectedSniper];
        playerInventory.PlayerOwnedSnipers[SelectedSniper]++; //Add one to pistol counter
        PlayerVRHUD.UpdateMoneyCounter(playerMoney.PlayerMoney);
        buySFX.Play();
       }
       else
       {
        Debug.Log("User attempted to buy gun but do not have enough money or their stash is full.");
        errorSFX.Play();
       }
    }
}
