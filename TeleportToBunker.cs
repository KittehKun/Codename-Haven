
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TeleportToBunker : UdonSharpBehaviour
{
    private Vector3 bunkerLocation;
    
    void Start()
    {
        bunkerLocation = GameObject.Find("VRCWorld").transform.position;
    }

    public override void Interact()
    {
        Networking.LocalPlayer.TeleportTo(bunkerLocation, Networking.LocalPlayer.GetRotation());
    }
}
