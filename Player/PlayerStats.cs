
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerStats : UdonSharpBehaviour
{
    public int PlayerMoney{get; set;}
    public GameObject[] traderGUIs; //Assigned in Unity | Array needed for changing all shopkeeper displays on startup
    void Start()
    {
        PlayerMoney = 10000; //Starting money for Player
        foreach(GameObject traderScreen in traderGUIs)
        {
            traderScreen.GetComponent<Text>().text = $"${PlayerMoney}";
        }
    }

    //Gets PlayerMoney from Player
    public int GetPlayerMoney()
    {
        return PlayerMoney;
    }

    //Method to add money to player
    public void AddMoney(int amount)
    {
        PlayerMoney += amount;
    }

    /// <summary>
    /// Method is used for updating all GUIs that display the player's money.
    /// </summary>
    public void UpdateMenuMoneyGUI()
    {
        foreach(GameObject traderScreen in traderGUIs)
        {
            
            traderScreen.GetComponent<Text>().text = $"${PlayerMoney}";
        }
    }
}
