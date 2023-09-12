
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MoneyItem : UdonSharpBehaviour
{
    public PlayerRaidInventory playerRaidInventory; //Assigned in Unity
    public int minMoneyValue; //Assigned in Unity
    public int maxMoneyValue; //Assigned in Unity

    void Start()
    {
        maxMoneyValue++; //Ensures that maxMoneyValue is inclusive when setting value in Unity inspector
    }

    public override void Interact()
    {
        //Initialize int value that will be used to generate a random number from 0-100
        int moneyValue = Random.Range(minMoneyValue, maxMoneyValue); //10 inclusive | 101 exclusive
        //Output how much money the player received
        Debug.Log("Player received $" + moneyValue + " from money item.");

        //Add money to player's raid wallet using AddToRaidWallet
        playerRaidInventory.AddToRaidWallet(moneyValue);
        
        //Destroy money GameObject after function completes
        Destroy(this.transform.gameObject);
    }
}
