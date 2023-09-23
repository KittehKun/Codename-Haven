
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerVRHUD : UdonSharpBehaviour
{
    private VRCPlayerApi localPlayer;
    private VRCPlayerApi.TrackingData playerHead;
    private Transform vrHUD; //Should be attached to the script's child object
    void Start()
    {
        //Assign the local player
        localPlayer = Networking.LocalPlayer;
        //Get the left hand HUD transform
        vrHUD = this.transform.GetChild(0); //Expected: LeftHandVRHUD Transform

        //Check if the player is in VR and disable HUD if not
        if (!localPlayer.IsUserInVR())
        {
            this.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        //Get the head tracking data
        playerHead = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

        //Update the HUD's position and rotation based on the player's head tracking data
        vrHUD.position = playerHead.position;
        vrHUD.rotation = playerHead.rotation;

        //Offset the HUD's position by 1 units away from the player's head
        vrHUD.position += vrHUD.forward * 1f;
    }

    public static void UpdateSPCount(int newStoragePointCount)
    {
        //Get the "SP Counter" GameObject and update the Text with the new StoragePoint count
        GameObject.Find("VR SP Counter").transform.GetComponent<TMPro.TextMeshProUGUI>().text = $"SP: {newStoragePointCount}/20";
    }

    public static void UpdateHPCount(int newHPCount, int newMaxHPCount)
    {
        //Get the "HP Counter" GameObject and update the Text with the new HealthPoint count
        GameObject.Find("VR HP Counter").transform.GetComponent<TMPro.TextMeshProUGUI>().text = $"HP: {newHPCount}/{newMaxHPCount}";
    }
}
