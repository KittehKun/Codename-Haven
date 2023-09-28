
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class StashBackButton : UdonSharpBehaviour
{
    public GameObject[] stashScreens; //Assigned in Unity | Used for disabling all stash screens when user hits the back button. Might as well disable all instead of one lmao
    public GameObject[] stashButtons; //Assigned in Unity | Used for enabling all buttons when user hits the back button
    public GameObject[] stashModels; //Assigned in Unity | Used for disabling all stash models when user hits the back button
    private AudioSource uiSFX; //This is the UI SFX audio source | Assigned in Start()
    void Start()
    {
        //Find uiSFX in the PlayerScriptsContainer and the PlayerUISFX GameObject
        uiSFX = GameObject.Find("PlayerScriptsContainer").transform.Find("PlayerUISFX").GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        //Disables all stash screens within the array
        foreach(GameObject screen in stashScreens)
        {
            screen.SetActive(false);
        }

        //Enables all buttons within the array
        foreach(GameObject button in stashButtons)
        {
            button.SetActive(true);
        }

        //Disables all stash models within the array
        foreach(GameObject model in stashModels)
        {
            model.SetActive(false);
        }

        //Play the UI SFX
        uiSFX.Play();
    }
}
