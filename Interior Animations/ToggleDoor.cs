
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//This is a general purpose script for opening and closing doors, cabinets, and other objects that can be opened and closed | All openable objects should have a collider
public class ToggleDoor : UdonSharpBehaviour
{
    //Audio Variables
    public AudioSource OpenSound; //Assigned in Unity inspector
    public AudioSource CloseSound; //Assigned in Unity inspector
    public Animator Animator; //Attached to the object that is being opened and closed
    private bool isOpen = false; //Used for checking if the object is open or not

    public override void Interact()
    {
        //Play the open animation if the object is closed
        if(!isOpen)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OpenDoor");
        } else
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "CloseDoor");
        }
    }

    public void OpenDoor()
    {
        OpenSound.Play(); //Play the open sound
        Animator.Play("Open"); //Play the open animation
        isOpen = true; //Set isOpen to true
    }

    public void CloseDoor()
    {
        CloseSound.Play(); //Play the close sound
        Animator.Play("Close"); //Play the close animation
        isOpen = false; //Set isOpen to false
    }
}
