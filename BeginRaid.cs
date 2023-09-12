
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BeginRaid : UdonSharpBehaviour
{
    private Transform[] spawnPoints; //This is an array of all the spawn points for the players taken from _SPAWNS GameObject
    void Start()
    {
        spawnPoints = GameObject.Find("_SPAWNS").GetComponentsInChildren<Transform>();
        Debug.Log($"Spawnpoints set with a total of {spawnPoints.Length} spawnpoints.");
    }

    public void TeleportToRaid()
    {
        //Teleport the local player to a random spawnpoint
        Networking.LocalPlayer.TeleportTo(spawnPoints[Random.Range(0, spawnPoints.Length)].position, Networking.LocalPlayer.GetRotation());
    }
}
