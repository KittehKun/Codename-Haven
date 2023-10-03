
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

        vrHUD.Find("VR XP Bar").transform.GetComponent<UnityEngine.UI.Slider>().value = 0; //Set the XP Bar to 0 to clear it
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

    public static void UpdateMoneyCounter(int newMoneyCount)
    {
        //Get the "Money Counter" GameObject and update the Text with the new Money count
        GameObject.Find("VR Money Counter").transform.GetComponent<TMPro.TextMeshProUGUI>().text = $"$ {newMoneyCount}";
    }

    public static void UpdateXPBar(int expToAdd)
    {   
        //Get the VR XP Bar GameObject and update the Slider value according to the player level
        //Slider component only uses whole numbers, maximum value is updated in PlayerStats.cs
        GameObject.Find("VR XP Bar").transform.GetComponent<UnityEngine.UI.Slider>().value += expToAdd;
    }

    public static void UpdateLevel(int newLevel)
    {

        //Get the VR Level Counter GameObject and update the Text with the new Level
        GameObject.Find("VR XP Bar").transform.Find("PlayerLevel").transform.GetComponent<TMPro.TextMeshProUGUI>().text = $"{newLevel}";
    }

    public static void UpdateXPToNextLevel(int newXPToNextLevel)
    {
        //Get the VR Level Counter GameObject and update the maximum value of the Slider
        GameObject.Find("VR XP Bar").transform.GetComponent<UnityEngine.UI.Slider>().maxValue = newXPToNextLevel;
    }

    public static void ShowDeathScreen()
    {
        //Get the VR Death Screen transform's children and set them to active
        foreach(Transform child in GameObject.Find("VR Death Screen").transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public static void HideDeathScreen()
    {
        //Get the VR Death Screen GameObject and set it to inactive
        foreach(Transform child in GameObject.Find("VR Death Screen").transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
