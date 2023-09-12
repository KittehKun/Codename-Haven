﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DisplayPistolShop : UdonSharpBehaviour
{
    public GameObject[] canvasChildren; //Assigned in Unity | Takes all children objects and disables them when button is pressed EXCEPT the ones needed
    public GameObject pistolPriceScreen; //Assigned in Unity
    void Start()
    {
        
    }

    public override void Interact()
    {
        foreach(GameObject gameObject in canvasChildren)
        {
            gameObject.SetActive(false); //Disables all children objects within array
        }
       pistolPriceScreen.SetActive(true);
    }
}
