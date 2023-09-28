
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BeginRaid : UdonSharpBehaviour
{
    private Transform[] spawnPoints; //This is an array of all the spawn points for the players taken from _SPAWNS GameObject
    private AudioSource uiSFX; //This is the UI SFX audio source | Assigned in Start()
    void Start()
    {
        spawnPoints = GameObject.Find("_SPAWNS").GetComponentsInChildren<Transform>();
        Debug.Log($"Spawnpoints set with a total of {spawnPoints.Length} spawnpoints.");

        //Find uiSFX in the PlayerScriptsContainer and the PlayerUISFX GameObject
        uiSFX = GameObject.Find("PlayerScriptsContainer").transform.Find("PlayerUISFX").GetComponent<AudioSource>();
    }

    public void TeleportToRaid()
    {
        //Teleport the local player to a random spawnpoint
        Networking.LocalPlayer.TeleportTo(spawnPoints[Random.Range(0, spawnPoints.Length)].position, Networking.LocalPlayer.GetRotation());

        //Play the UI SFX
        uiSFX.Play();
    }
}
