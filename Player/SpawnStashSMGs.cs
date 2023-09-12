
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SpawnStashSMGs : UdonSharpBehaviour
{
    public PlayerInventory playerInventoryScript; //Assigned in Unity
    public GameObject smgPrefabs; //Assigned in Unity
    public GameObject playerScriptsContainer; //Assigned in Unity
    public GameObject smgScreen; //Assigned in Unity
    public GameObject stashTitle; //Assigned in Unity
    public GameObject[] smgCountText; //Assigned in Unity
    public GameObject backButton; //Assigned in Unity
    void Start()
    {
        
    }

    public override void Interact()
    {
        //this.transform.parent.transform.gameObject.SetActive(false); //Expected: Canvas Disables
        
        foreach(Transform child in this.transform.parent.transform)
        {
            child.gameObject.SetActive(false);
            //Debug.Log($"Disabling {child.gameObject.name}");
        }

        stashTitle.transform.GetComponent<Text>().text = "Your SMGs";
        stashTitle.gameObject.SetActive(true);
        //GameObject pistolsScreen = this.transform.parent.transform.Find("PistolsScreen").gameObject; //Direct assignment of variable is way more performant
        smgScreen.SetActive(true);

        smgPrefabs.gameObject.SetActive(true); //Expected: Displays all SMGs to User

        backButton.SetActive(true);

        //Get Player pistol inventory from script and update text
        for(int i = 0; i < playerInventoryScript.PlayerOwnedSMGs.Length; i++)
        {
            smgCountText[i].GetComponent<Text>().text = $"{playerInventoryScript.PlayerOwnedSMGs[i]}"; //Updates stash screen with player owned smgs from array
        }
    }
}
