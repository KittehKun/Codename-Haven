
using UdonSharp;
using UnityEngine;

public class DisableSpawnMeshRenderer : UdonSharpBehaviour
{
    private GameObject[] spawnPoints;
    private Transform spawnPoint; //Named _SPAWNS in Hierarchy | Script should be attached to GameObject named _SPAWNS

    void Start()
    {
        spawnPoint = this.transform;
        //Add all children of _SPAWNS to spawnPoints array
        spawnPoints = new GameObject[spawnPoint.transform.childCount];
        for (int i = 0; i < spawnPoint.transform.childCount; i++)
        {
            spawnPoints[i] = spawnPoint.transform.GetChild(i).gameObject;
            //Debug.Log("Added " + spawnPoints[i].name + " to spawnPoints array");
        }

        //Disable mesh render for all spawn points
        foreach (GameObject spawn in spawnPoints)
        {
            spawn.GetComponent<MeshRenderer>().enabled = false;
            //Rename all GameObjects in array to "Player Spawn Point"
            spawn.name = "Player Spawn Point";
        }
    }
}
