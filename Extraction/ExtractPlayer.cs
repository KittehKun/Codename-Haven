
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ExtractPlayer : UdonSharpBehaviour
{
    private VRCPlayerApi player;
    public Transform extractionPoint; // The point where the player will be teleported to | Assigned in Unity
    private Vector3 extractionPointPosition; // The position of the extraction point
    public PlayerRaidManager playerRaidManager; // PlayerRaidManager script from GameObject | Assigned in Unity
    public bool playerExtractedFrom; //Used for checking if a player has extracted from this point
    private Transform[] extractionPoints;
    
    void Start()
    {
        player = Networking.LocalPlayer;
        extractionPointPosition = extractionPoint.position;
        extractionPoints = playerRaidManager.extractionPoints;
        //Print the name of all extraction points
        foreach(Transform extractionPoint in extractionPoints)
        {
            Debug.Log(extractionPoint.gameObject.name);
        }
    }

    /// <summary>
    /// Method is used for extracting the player from the raid. Adds raid wallet into player inventory.
    /// </summary>
    public override void Interact()
    {
        Debug.Log("Player extracted!");
        player.TeleportTo(extractionPointPosition, player.GetRotation());
        playerRaidManager.AddRaidWalletToInventory();

        //Call the method to enable other extraction points | Method must be called first as the current extraction point is disabled in this method
        EnableOtherExtractionPoints();

        //Disable the extraction point's collider so it can't be interacted with again
        this.GetComponent<Collider>().enabled = false;
        //Find the Canvas GameObject in the children
        GameObject extractionCanvas = this.transform.Find("Canvas").gameObject;
        //Change the TextMeshPro text to say "Extraction Point Used"
        extractionCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Extract Unavailable";
        //Set the playerExtractedFrom bool to true as Player extracted from this point. Extraction will be unavailable until player interacts with another extraction point.
        this.playerExtractedFrom = true;
    }

    //This method is used for enabling other extraction points after the player has extracted from one
    private void EnableOtherExtractionPoints()
    {
        this.extractionPoints = playerRaidManager.extractionPoints;
        foreach(Transform extractionPoint in extractionPoints)
        {
            //Find if the GameObject name is "Extraction Point"
            if(extractionPoint.gameObject.name.Equals("Alpha Extract") || extractionPoint.gameObject.name.Equals("Beta Extract"))
            {
                Debug.Log("Checkpoint Selected!");
                Transform radioInteractable = extractionPoint.GetChild(0);
                //Get the first child of the radioInteractable GameObject and set the text to "Extraction Available"
                radioInteractable.Find("Canvas").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Extract Available";
                //Enable the collider
                radioInteractable.GetComponent<Collider>().enabled = true;
                //Set the playerExtractedFrom bool to false to reset extraction point
                radioInteractable.GetComponent<ExtractPlayer>().playerExtractedFrom = false;
            } else
            {
                Debug.Log("Radio Interactable Selected!");
                //Set the text to "Extraction Available"
                extractionPoint.Find("Canvas").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Extraction Available";
                //Enable the collider
                extractionPoint.GetComponent<Collider>().enabled = true;
                //Set the playerExtractedFrom bool to false to reset extraction point
                extractionPoint.GetComponent<ExtractPlayer>().playerExtractedFrom = false;
            }
        }
    }
}
