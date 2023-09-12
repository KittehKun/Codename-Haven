
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SpawnStashARs : UdonSharpBehaviour
{
    public PlayerInventory playerInventoryScript; //Assigned in Unity
    public GameObject arPrefabs; //Assigned in Unity
    public GameObject playerScriptsContainer; //Assigned in Unity
    public GameObject arScreen; //Assigned in Unity
    public GameObject stashTitle; //Assigned in Unity
    public GameObject[] arCountText; //Assigned in Unity
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

        stashTitle.transform.GetComponent<Text>().text = "Your Rifles";
        stashTitle.gameObject.SetActive(true);
        //GameObject pistolsScreen = this.transform.parent.transform.Find("PistolsScreen").gameObject; //Direct assignment of variable is way more performant
        arScreen.SetActive(true);

        arPrefabs.gameObject.SetActive(true); //Expected: Displays all ARs to User

        backButton.SetActive(true);

        //Get Player pistol inventory from script and update text
        for(int i = 0; i < playerInventoryScript.PlayerOwnedARs.Length; i++)
        {
            arCountText[i].GetComponent<Text>().text = $"{playerInventoryScript.PlayerOwnedARs[i]}"; //Updates stash screen with player owned ARs from array
        }
    }
}
