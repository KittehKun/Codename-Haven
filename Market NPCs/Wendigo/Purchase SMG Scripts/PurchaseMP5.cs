
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseMP5 : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int MP5Price; //Assigned in Unity
    void Start()
    {
        
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= MP5Price && playerInventory.PlayerOwnedSMGs[1] < 5)
       {
        playerMoney.PlayerMoney -= MP5Price;
        playerInventory.PlayerOwnedSMGs[1]++; //Add one to pistol counter
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
       }
       else
       {
        Debug.Log("User attempted to buy gun but do not have enough money or their stash if full.");
       }
    }
}
