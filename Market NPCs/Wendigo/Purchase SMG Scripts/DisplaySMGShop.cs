
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DisplaySMGShop : UdonSharpBehaviour
{
    public GameObject[] canvasChildren; //Assigned in Unity | Takes all children objects and disables them when button is pressed EXCEPT the ones needed
    public GameObject smgPriceScreen; //Assigned in Unity
    void Start()
    {
        
    }

    public override void Interact()
    {
        foreach(GameObject gameObject in canvasChildren)
        {
            gameObject.SetActive(false); //Disables all children objects within array
        }
       smgPriceScreen.SetActive(true);
    }
}
