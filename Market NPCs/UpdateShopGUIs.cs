//The purpose of this script is to update the shop keeper's GUI when a player purchases an item

using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class UpdateShopGUIs : UdonSharpBehaviour
{
    public PlayerStats playerStatsScript; //Assigned in Unity
    public GameObject shopKeeperMoneyCounter; //Assigned in Unity

    public void UpdateShopKeeper()
    {
        //Debug.Log($"Setting money to ${playerStatsScript.GetPlayerMoney()}");
        shopKeeperMoneyCounter.GetComponent<Text>().text = $"${playerStatsScript.GetPlayerMoney()}"; //Updates shop money counter with new value
    }
}
