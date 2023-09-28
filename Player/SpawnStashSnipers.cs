
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SpawnStashSnipers : UdonSharpBehaviour
{
    public PlayerInventory playerInventoryScript; //Assigned in Unity
    public GameObject sniperPrefabs; //Assigned in Unity
    public GameObject playerScriptsContainer; //Assigned in Unity
    public GameObject sniperScreen; //Assigned in Unity
    public GameObject stashTitle; //Assigned in Unity
    public GameObject[] sniperCountText; //Assigned in Unity
    public GameObject backButton; //Assigned in Unity
    private AudioSource uiSFX; //This is the UI SFX audio source | Assigned in Start()
    void Start()
    {
        //Find uiSFX in the PlayerScriptsContainer and the PlayerUISFX GameObject
        uiSFX = playerScriptsContainer.transform.Find("PlayerUISFX").GetComponent<AudioSource>();   
    }

    public override void Interact()
    {
        //this.transform.parent.transform.gameObject.SetActive(false); //Expected: Canvas Disables
        
        foreach(Transform child in this.transform.parent.transform)
        {
            child.gameObject.SetActive(false);
            //Debug.Log($"Disabling {child.gameObject.name}");
        }

        stashTitle.transform.GetComponent<Text>().text = "Your Snipers";
        stashTitle.gameObject.SetActive(true);
        //GameObject pistolsScreen = this.transform.parent.transform.Find("PistolsScreen").gameObject; //Direct assignment of variable is way more performant
        sniperScreen.SetActive(true);

        sniperPrefabs.gameObject.SetActive(true); //Expected: Displays all Snipers to User

        backButton.SetActive(true);

        //Get Player pistol inventory from script and update text
        for(int i = 0; i < playerInventoryScript.PlayerOwnedSnipers.Length; i++)
        {
            sniperCountText[i].GetComponent<Text>().text = $"{playerInventoryScript.PlayerOwnedSnipers[i]}"; //Updates stash screen with player owned Snipers from array
        }

        //Play the UI SFX
        uiSFX.Play();
    }
}
