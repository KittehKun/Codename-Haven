
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AmethystPlayRandomGreeting : UdonSharpBehaviour
{
    private AudioSource[] greetings; //Assigned in Start()
    void Start()
    {
        //Get all the greetings from the parent object
        greetings = this.transform.parent.Find("Voicelines").GetComponentsInChildren<AudioSource>();
    }

    //Play a random greeting when the player interacts with the NPC
    public override void Interact()
    {
        //Play a random greeting
        greetings[Random.Range(0, greetings.Length)].Play();
    }
}
