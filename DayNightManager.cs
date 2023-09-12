
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to manage the day/night cycle of the world
public class DayNightManager : UdonSharpBehaviour
{
    public Light sun; //The directional sun light | Assigned in Unity
    public float secondsInFullDay = 120f; //The amount of seconds in a full day
    [Range(0, 1)]
    public float currentTimeOfDay = 0; //The current time of day
    [HideInInspector]
    public float timeMultiplier = 1f; //The multiplier for the time of day

    public float sunInitialIntensity; //The initial intensity of the sun
    
    void Start()
    {
        this.sunInitialIntensity = sun.intensity; //Set the initial intensity of the sun
    }

    void Update()
    {
        UpdateSun(); //Update the sun
        this.currentTimeOfDay += (Time.deltaTime / this.secondsInFullDay) * this.timeMultiplier; //Update the current time of day
        if (this.currentTimeOfDay >= 1) //If the current time of day is greater than or equal to 1
        {
            this.currentTimeOfDay = 0; //Reset the current time of day
        }
    }

    public void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((this.currentTimeOfDay * 360f) - 90, 170, 0); //Update the rotation of the sun

        float intensityMultiplier = 1; //The intensity multiplier of the sun

        //If the current time of day is less than or equal to 0.23 or greater than or equal to 0.75
        if (this.currentTimeOfDay <= 0.23f || this.currentTimeOfDay >= 0.75f) //If the current time of day is less than or equal to 0.23 or greater than or equal to 0.75
        {
            intensityMultiplier = 0; //Set the intensity multiplier to 0
        }
        //If the current time of day is greater than or equal to 0.25
        else if (this.currentTimeOfDay <= 0.25f) //If the current time of day is less than or equal to 0.25
        {
            intensityMultiplier = Mathf.Clamp01((this.currentTimeOfDay - 0.23f) * (1 / 0.02f)); //Set the intensity multiplier to the current time of day minus 0.23 multiplied by 1 divided by 0.02
        }
        //If the current time of day is less than or equal to 0.73
        else if (this.currentTimeOfDay >= 0.73f) //If the current time of day is greater than or equal to 0.73
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((this.currentTimeOfDay - 0.73f) * (1 / 0.02f))); //Set the intensity multiplier to 1 minus the current time of day minus 0.73 multiplied by 1 divided by 0.02
        }

        sun.intensity = this.sunInitialIntensity * intensityMultiplier; //Set the intensity of the sun to the initial intensity of the sun multiplied by the intensity multiplier
    }
}
