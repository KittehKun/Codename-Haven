
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PurchaseVector : UdonSharpBehaviour
{
    public PlayerStats playerMoney; //Assigned in Unity
    public PlayerInventory playerInventory; //Assigned Unity
    public GameObject wendigoMoneyCounter; //Assigned in Unity
    public int VectorPrice; //Assigned in Unity
    void Start()
    {
        
    }

    public override void Interact()
    {
       if(playerMoney.PlayerMoney >= VectorPrice && playerInventory.PlayerOwnedSMGs[4] < 5)
       {
        playerMoney.PlayerMoney -= VectorPrice;
        playerInventory.PlayerOwnedSMGs[4]++; //Add one to pistol counter
        wendigoMoneyCounter.GetComponent<Text>().text = $"${playerMoney.PlayerMoney}";
       }
       else
       {
        Debug.Log("User attempted to buy gun but do not have enough money or their stash if full.");
       }
    }
}
