
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayTangoRandomGreeting : UdonSharpBehaviour
{
    public AudioSource GreetingOne {get; private set;}
    public AudioSource GreetingTwo {get; private set;}
    public AudioSource GreetingThree {get; private set;}
    void Start()
    {
        //Assign AudioSource objects to the variables
        GameObject voiceLines = this.transform.parent.Find("Voicelines").gameObject;
        GreetingOne = voiceLines.transform.GetChild(0).GetComponent<AudioSource>();
        GreetingTwo = voiceLines.transform.GetChild(1).GetComponent<AudioSource>();
        GreetingThree = voiceLines.transform.GetChild(2).GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        int chooseGreeting = Random.Range(0, 3); //Chooses a random integer from 0-2 [3 is excluded]
        switch(chooseGreeting)
        {
            case 0:
                Debug.Log("Generic greeting one chosen.");
                GreetingOne.Play();
                break;
            case 1:
                Debug.Log("Generic greeting two chosen.");
                GreetingTwo.Play();
                break;
            case 2:
                Debug.Log("Generic greeting three chosen.");
                GreetingThree.Play();
                break;
            default:
                Debug.Log("Kitteh your code is messed up.");
                break;
        }
    }
}
