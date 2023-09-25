
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayRandomAudio : UdonSharpBehaviour
{
    public AudioSource GreetingOne {get;set;}
    public AudioSource GreetingTwo {get;set;}
    public AudioSource SecretGreetingOne {get;set;}
    public AudioSource SecretGreetingTwo {get;set;}

    void Start()
    {
        GameObject voiceLines = this.transform.parent.transform.Find("Voicelines").gameObject;
        Debug.Log($"Found {voiceLines.name}");
        GreetingOne = voiceLines.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        GreetingTwo = voiceLines.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        SecretGreetingOne = voiceLines.transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        SecretGreetingTwo = voiceLines.transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        Debug.Log($"Successfully assigned all Voicelines to merchant.");
    }

    public override void Interact()
    {
        int chooseGreeting = Random.Range(0, 4); //Chooses a random integer from 0-3 [4 is excluded]
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
                Debug.Log("Secret dialogue chosen.");
                SecretGreetingOne.Play();
                break;
            case 3:
                Debug.Log("Secret dialogue chosen.");
                SecretGreetingTwo.Play();
                break;
            default:
                Debug.Log("Kitteh your code is messed up.");
                break;
        }
    }
}
