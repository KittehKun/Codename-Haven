
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to handle the logic of attachments for guns by enabling/disabling the correct objects
//This script is attached to the gun itself
public class AttachmentSystem : UdonSharpBehaviour
{
    private int[] scopes = new int[3]; //0 = none, 1 = Red Dot, 2 = Holo, 3 = ACOG, 4 = Sniper Scope
    private int[] grips = new int[2]; //0 = none, 1 = Vertical, 2 = Angled
    private bool suppressorEnabled = false;
    private bool laserEnabled = false;
    private bool flashlightEnabled = false;
    public Transform attachmentsContainer; //The container for all the attachments | Assigned in Start() if not assigned in inspector
    void Start()
    {
        if(attachmentsContainer == null)
        {
            attachmentsContainer = transform.Find("_ATTACHMENTS");
        }
    }
}
