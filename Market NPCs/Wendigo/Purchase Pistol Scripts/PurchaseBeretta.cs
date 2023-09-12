
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseBeretta : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int BerettaPrice; //Assigned in Unity
    void Start()
    {
        
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
        playerMoney.PlayerMoney -= BerettaPrice;
        playerInventory.PlayerOwnedPistols[2]++; //Add one to pistol counter
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
        Debug.Log($"Player bought Beretta! Player now has {playerInventory.PlayerOwnedPistols[2]} Berettas.");
    }
}
