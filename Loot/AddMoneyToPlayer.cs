
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class AddMoneyToPlayer : UdonSharpBehaviour
{
    
    public PlayerStats playerStats; //Create variable to access PlayerStats script | Assigned in Unity
    public GameObject mainMenu; //Assigned in Unity

    public override void Interact()
    {
        //Debug.Log($"Player had {playerStats.GetPlayerMoney()}");
        playerStats.AddMoney(Random.Range(10, 51)); //Adds money to PlayerStats script
        Debug.Log($"Player now has {playerStats.GetPlayerMoney()}");
        mainMenu.transform.GetChild(0).GetComponent<Text>().text = $"${playerStats.GetPlayerMoney()}"; //Updates Main Menu GUI

        Destroy(this.transform.gameObject); //Removes money pile
    }

}
