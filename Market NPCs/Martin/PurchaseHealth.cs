
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to add health to the player when they purchase health from the shopkeeper
public class PurchaseHealth : UdonSharpBehaviour
{
    public PlayerStats playerStats; //Assigned in Unity | Used for adding health to the player
    public int healthChoice; //Assigned in Unity | Used for selecting the health value from the array
    private int[] healthPrices; //Assigned in Start() | Used for selecting the price of the health value
    private int[] healthValues; //Assigned in Start() | Used for selecting the health value
    private AudioSource buySFX; //Assigned in Start() | Used for playing the buy sound
    private AudioSource healSFX; //Assigned in Start() | Used for playing the heal sound after purchase
    
    void Start()
    {
        //Assign healthPrices array
        healthPrices = new int[3] { 125, 200, 350 };
        //Assign healthValues array
        healthValues = new int[3] { 25, 50, 100 }; //First tier adds +25 health, second tier adds +50 health, third tier adds +100 health
        //Assign buySFX
        this.buySFX = GameObject.Find("PlayerBuyItemAudio").GetComponent<AudioSource>();
        //Assign healSFX
        this.healSFX = this.transform.Find("HealSFX").GetComponent<AudioSource>();
    }

    //This method returns true if the player will overheal from the purchase and false if they will not | If false, the newHealth value will be assigned
    private int HealPlayer()
    {
        //Check if the player's current health + the health value is greater than the player's maximum health
        if(playerStats.PlayerHealth + healthValues[healthChoice] > playerStats.MaximumHealth)
        {
            return playerStats.MaximumHealth;
        } else
        {
            //Set newHealth to the player's current health + the health value
            int newHealth = playerStats.PlayerHealth + healthValues[healthChoice];
            //Return false as the player will not overheal
            return newHealth;
        }
    }

    public override void Interact()
    {
        //Check if the player has enough money to buy the health
        if (playerStats.PlayerMoney >= healthPrices[healthChoice])
        {
            //Remove money from player
            playerStats.PlayerMoney -= healthPrices[healthChoice];
            //Play buy sound
            buySFX.Play();
            //Play heal sound
            healSFX.Play();
            //Check if the player will overheal
            playerStats.PlayerHealth = HealPlayer();
            
            //Update HUD
            PlayerVRHUD.UpdateHPCount(playerStats.PlayerHealth, playerStats.MaximumHealth);
            PlayerVRHUD.UpdateMoneyCounter(playerStats.PlayerMoney);
        }
    }
}
