
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to rotate the Global Sun object to simulate day and night
public class DayNightManager : UdonSharpBehaviour
{
    void Update()
    {
        //Rotate the Global Sun object by 0.1 degrees every frame
        this.transform.Rotate(0.1f, 0, 0);
    }
}
