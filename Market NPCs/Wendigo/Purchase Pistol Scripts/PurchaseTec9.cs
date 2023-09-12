
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseTec9 : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int Tec9Price; //Assigned in Unity
    void Start()
    {
        
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= Tec9Price && playerInventory.PlayerOwnedPistols[6] < 5)
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
        playerMoney.PlayerMoney -= Tec9Price;
        playerInventory.PlayerOwnedPistols[6]++; //Add one to pistol counter
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
        Debug.Log($"Player bought Tec9! Player now has {playerInventory.PlayerOwnedPistols[6]} Tec9s.");
    }
}
