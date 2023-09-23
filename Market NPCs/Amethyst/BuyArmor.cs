
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BuyArmor : UdonSharpBehaviour
{
    public int armorChoice; //Assigned in Unity | Value will be used to select index from array
    private int[] armorPrices; //Assigned in Start()
    public PlayerStats playerStats; //Assigned in Unity
    private AudioSource buySFX; //Assigned in Start()
    void Start()
    {
        armorPrices = new int[3] { 1000, 2000, 3500 };
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        //Check if the player has enough money to buy the armor
        if (playerStats.PlayerMoney >= armorPrices[armorChoice])
        {
            //Remove money from player
            playerStats.PlayerMoney -= armorPrices[armorChoice];

            //Set maximum health to the new armor value
            switch(armorChoice)
            {
                case 0:
                    playerStats.SetMaximumHealth(150);
                    break;
                case 1:
                    playerStats.SetMaximumHealth(175);
                    break;
                case 2:
                    playerStats.SetMaximumHealth(200);
                    break;
            }

            //Play the buy sound
            buySFX.Play();
        }
        else
        {
            Debug.Log("User attempted to buy armor but does not have enough money.");
        }
    }
}
