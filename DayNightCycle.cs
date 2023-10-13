
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to handle the day/night cycle where the skybox smoothly changes based on the time of day
public class DayNightCycle : UdonSharpBehaviour
{
    [Header("Skyboxes")]
    [SerializeField] private Material daySkyboxMat; //Assigned in Unity | Used to access the day skybox
    [SerializeField] private Material nightSkyboxMat; //Assigned in Unity | Used to access the night skybox

    [Header("Lighting")]
    [SerializeField] private Light dayLight; //Used to access the directional light in the scene | Assigned in Unity
    [SerializeField] private Light nightLight; //Used to access the directional light in the scene | Assigned in Unity
    private float rotationSpeed = 0.1f; //Used to set the rotation speed of the Skybox

    void Start()
    {
        if(!daySkyboxMat) Debug.LogError("Day Skybox not assigned in Unity.");
        if(!nightSkyboxMat) Debug.LogError("Night Skybox not assigned in Unity.");
    }

    void FixedUpdate()
    {
        
    }

    //Method is called by VRChat on remote players - Used for syncing the time of day with other players
    public override void OnDeserialization()
    {
        
    }
}
