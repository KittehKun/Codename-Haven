
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayRandomGreeting : UdonSharpBehaviour
{
    private AudioSource[] npcGreetings; //Array of all the greetings

    void Start()
    {
        GameObject voiceLines = this.transform.parent.Find("Voicelines").gameObject;
        npcGreetings = voiceLines.GetComponentsInChildren<AudioSource>(); //Get all the greetings
    }

    public override void Interact()
    {
        int randomGreeting = Random.Range(0, npcGreetings.Length); //Pick a random greeting
        npcGreetings[randomGreeting].Play(); //Play the greeting
    }
}
