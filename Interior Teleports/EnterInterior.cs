
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to handle teleport players to the interior of a location. A general purpose script.
public class EnterInterior : UdonSharpBehaviour
{
    public Vector3 targetLocation; //Location of where the player will spawn inside an interior | Assigned in Unity inspector - different location needs to be set for every interior

    public override void Interact()
    {
        //Set the player's position to the target location
        Networking.LocalPlayer.TeleportTo(targetLocation, Networking.LocalPlayer.GetRotation());
    }
}
