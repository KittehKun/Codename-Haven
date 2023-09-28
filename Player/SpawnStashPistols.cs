
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to spawn the pistols with a text UI field next to the pistol showing how many pistols a player owns.
//Players who attempt to spawn a zero'd pistol will not be able to spawn a pistol to the table for raid-
public class SpawnStashPistols : UdonSharpBehaviour
{
    public PlayerInventory playerInventoryScript; //Assigned in Unity
    public GameObject pistolPrefabs; //Assigned in Unity
    public GameObject playerScriptsContainer; //Assigned in Unity
    public GameObject pistolsScreen; //Assigned in Unity
    public GameObject stashTitle; //Assigned in Unity
    public GameObject[] pistolCountText; //Assigned in Unity
    public GameObject backButton; //Assigned in Unity
    private AudioSource uiSFX; //This is the UI SFX audio source | Assigned in Start()
    void Start()
    {
        //Find uiSFX in the PlayerScriptsContainer and the PlayerUISFX GameObject
        uiSFX = playerScriptsContainer.transform.Find("PlayerUISFX").GetComponent<AudioSource>();
    }

    public override void Interact()
    {        
        foreach(Transform child in this.transform.parent.transform)
        {
            child.gameObject.SetActive(false);
        }

        stashTitle.transform.GetComponent<Text>().text = "Your Pistols";
        stashTitle.gameObject.SetActive(true);
        pistolsScreen.SetActive(true);

        pistolPrefabs.gameObject.SetActive(true); //Expected: Displays all Pistols to User

        backButton.SetActive(true);

        //Get Player pistol inventory from script and update text
        for(int i = 0; i < playerInventoryScript.PlayerOwnedPistols.Length; i++)
        {
            pistolCountText[i].GetComponent<Text>().text = $"{playerInventoryScript.PlayerOwnedPistols[i]}"; //Updates stash screen with player owned pistols from array
        }

        //Play the UI SFX
        uiSFX.Play();
        
    }
}
