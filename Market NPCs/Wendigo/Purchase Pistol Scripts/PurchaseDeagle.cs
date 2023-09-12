
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseDeagle : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int DeaglePrice; //Assigned in Unity
    void Start()
    {
        
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
       }
    }

    private void PurchaseWeapon()
    {
        playerMoney.PlayerMoney -= DeaglePrice;
        playerInventory.PlayerOwnedPistols[4]++; //Add one to pistol counter
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
        Debug.Log($"Player bought Deagle! Player now has {playerInventory.PlayerOwnedPistols[4]} Deagles.");
    }
}
