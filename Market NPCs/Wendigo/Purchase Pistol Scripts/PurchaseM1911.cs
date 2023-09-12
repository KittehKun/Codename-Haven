
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseM1911 : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int M1911Price; //Assigned in Unity
    void Start()
    {
        
    }

    public override void Interact()
    {
       //Check if the Player has enough money for the weapon and check if the stash inventory for that gun is not full
       if(playerMoney.PlayerMoney >= M1911Price && playerInventory.PlayerOwnedPistols[0] < 5)
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
        playerMoney.PlayerMoney -= M1911Price;
        playerInventory.PlayerOwnedPistols[0]++; //Add one to pistol counter
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
        Debug.Log($"Player bought M1911! Player now has {playerInventory.PlayerOwnedPistols[0]} M1911s.");
    }
}
