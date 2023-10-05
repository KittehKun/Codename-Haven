
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[Obsolete("This script is no longer used as PlayerVRHUD.cs handles all HUD elements regardless of VR or not. Please use PlayerVRHUD.cs instead.", true)]
public class PlayerHUD : UdonSharpBehaviour
{
    private GameObject playerHUD; //The player's HUD which contains the UI elements. Should be attached to the player's head.
    private VRCPlayerApi player; //Variable will be used for getting the local player and attaching the HUD to the player's head.
    private float playerHUDOffset = 2f; //The offset of the HUD from the player's head. This is used to prevent the HUD from clipping into the player's head.
    
    void Start()
    {
        //Get the Canvas object from the child of playerHUD
        playerHUD = this.transform.GetChild(0).gameObject;
        Debug.Log($"{playerHUD.name} has been initialized successfully.");
        player = Networking.LocalPlayer; //Expected: Local Player
    }

    void Update()
    {
        //Update the HUD's position and rotation based on the player's head tracking data
        playerHUD.transform.position = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        playerHUD.transform.rotation = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;

        //Offset the HUD's position by 1 units away from the player's head
        playerHUD.transform.position += playerHUD.transform.forward * playerHUDOffset;
    }

    public static void UpdateSPCount(int newStoragePointCount)
    {
        //Get the "SP Counter" GameObject and update the Text with the new StoragePoint count
        GameObject.Find("SP Counter").transform.GetComponent<TextMeshProUGUI>().text = $"SP: {newStoragePointCount}/20";
    }

    public static void UpdateHPCount(int newHPCount, int newMaxHPCount)
    {
        //Get the "HP Counter" GameObject and update the Text with the new HealthPoint count
        GameObject.Find("HP Counter").transform.GetComponent<TextMeshProUGUI>().text = $"HP: {newHPCount}/{newMaxHPCount}";
    }
}
