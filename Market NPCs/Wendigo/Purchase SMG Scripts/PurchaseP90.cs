
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseP90 : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int P90Price; //Assigned in Unity
    private AudioSource buySFX; //Assigned in Start()
    void Start()
    {
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= P90Price && playerInventory.PlayerOwnedSMGs[3] < 5)
       {
        playerMoney.PlayerMoney -= P90Price;
        playerInventory.PlayerOwnedSMGs[3]++; //Add one to pistol counter
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
        buySFX.Play();
       }
       else
       {
        Debug.Log("User attempted to buy gun but do not have enough money or their stash is full.");
       }
    }
}
