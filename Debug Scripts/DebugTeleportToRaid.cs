
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DebugTeleportToRaid : UdonSharpBehaviour
{
    public VRCPlayerApi Player {get; set;}
    public Transform TargetLocation {get; set;}
    public Transform BunkerTeleport {get; set;}
    void Start()
    {
        //Get Local Player
        Player = Networking.LocalPlayer;

        //Get TargetLocation Transform
        TargetLocation = GameObject.Find("TestingArea").transform;

        //Get BunkerTeleport Transform
        BunkerTeleport = GameObject.Find("VRCWorld").transform;

        Debug.Log("Successfully set all transform values");
    }

    //Created method for the button to work
    public override void Interact()
    {
        //Teleport Player to TestingArea
        Debug.Log($"Attempting to teleport player to {TargetLocation.position}");
        Player.TeleportTo(TargetLocation.position, TargetLocation.rotation);
    }
}
