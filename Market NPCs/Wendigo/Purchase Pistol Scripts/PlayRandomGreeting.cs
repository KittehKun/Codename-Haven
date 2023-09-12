
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayRandomGreeting : UdonSharpBehaviour
{
    public AudioSource GreetingOne {get;set;}
    public AudioSource GreetingTwo {get;set;}

    void Start()
    {
        GameObject voiceLines = this.transform.parent.transform.Find("Voicelines").gameObject;
        Debug.Log($"Found {voiceLines.name}");
        GreetingOne = voiceLines.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        Debug.Log($"Found {GreetingOne} as {voiceLines.transform.GetChild(0).gameObject}");
        GreetingTwo = voiceLines.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        Debug.Log($"Found {GreetingOne} as {voiceLines.transform.GetChild(1).gameObject}");
    }

    public override void Interact()
    {
        int chooseGreeting = Random.Range(0, 2); //Chooses a random integer from 0-1 [2 is excluded]
        if(chooseGreeting != 1)
        {
            GreetingOne.Play(); //Play Original Dialogue
        } else
        {
            GreetingTwo.Play(); //Play Variation
        }
    }
}
