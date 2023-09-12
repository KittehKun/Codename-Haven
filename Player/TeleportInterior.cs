
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to teleport the local player to the SpawnLocation set in the Unity inspector
public class TeleportInterior : UdonSharpBehaviour
{
    public Transform TargetLocation; //Assigned in Unity inspector | Used for teleporting the player to the interior

    //The player interacts with the door's collider to teleport to the interior
    public override void Interact()
    {
        //Teleport the player to the TargetLocation
        Networking.LocalPlayer.TeleportTo(TargetLocation.position, TargetLocation.rotation);
    }
}
