
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerHitbox : UdonSharpBehaviour
{
    private VRCPlayerApi player;
    private Vector3 headTrackingPos; //Head tracking position
    public GameObject hitbox; //Sphere Hitbox | Assigned in Unity
    
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
}
