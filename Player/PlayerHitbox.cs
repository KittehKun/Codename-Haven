
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerHitbox : UdonSharpBehaviour
{
    private VRCPlayerApi player;
    private Vector3 headTrackingPos; //Head tracking position
    public GameObject hitbox; //Sphere Hitbox | Assigned in Unity
    public int health = 150; //Player health | Assigned in Unity
    public PlayerRaidManager playerRaidManager; //Player Raid Manager | Assigned in Unity | Used for resetting player inventory
    
    void Start()
    {
        //Get local player
        player = Networking.LocalPlayer;
        //Scale sphere to a 1/3 of its size
        hitbox.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
    }

    void Update()
    {
        //Get head tracking position
        headTrackingPos = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        //Set hitbox position to player's head position
        hitbox.transform.position = headTrackingPos;
    }

    //Function to handle player taking damage
    public void TakeDamage(int damage)
    {
        //Subtract damage from health
        health -= damage;
        //Update HP counter
        PlayerHUD.UpdateHPCount(health);
        //Check to see if player is dead
        if(health <= 0)
        {
            //Kill player
            Die();
        }
    }

    //Function will call the reset function on the PlayerRaidInventory script and teleport the player back to the spawn point
    private void Die()
    {
        //Reset Player and teleport to spawn point
        playerRaidManager.ResetPlayer(true);
    }
}
