
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseUSP : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int USPPrice; //Assigned in Unity
    void Start()
    {
        
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
       }
    }

    private void PurchaseWeapon()
    {
        playerMoney.PlayerMoney -= USPPrice;
        playerInventory.PlayerOwnedPistols[3]++; //Add one to pistol counter
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
        Debug.Log($"Player bought USP! Player now has {playerInventory.PlayerOwnedPistols[3]} USPs.");
    }
}
