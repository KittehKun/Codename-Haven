
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to enable NavMeshAgents on all enemies when the game starts
public class EnableNavAgents : UdonSharpBehaviour
{
    public GameObject[] enemies; //Array of enemies | Assigned in Unity | Used for enabling NavMeshAgents on all enemies
    void Start()
    {
        //Enable all GameObjects within the array
        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
}
