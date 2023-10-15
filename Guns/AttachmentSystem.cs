
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to handle the logic of attachments for guns by enabling/disabling the correct objects
//This script is attached to the gun itself | Each weapon innately supports suppressors, lasers, and flashlights
public class AttachmentSystem : UdonSharpBehaviour
{
    public bool SupportsScope; //Check if gun supports scopes | Assigned in Unity based on gun
    public bool SupportsGrip; //Check if gun supports grips | Assigned in Unity based on gun
    public Transform attachmentsContainer; //Container for all attachments | Assigned in Unity
    [UdonSynced] public bool suppressorEnabled = false; //Check if suppressor is enabled | Used for enabling/disabling suppressor

    //Network Flags
    [UdonSynced] private int scopeID = -1; //ID of scope | Used for enabling/disabling scope
    [UdonSynced] private int gripID = -1; //ID of grip | Used for enabling/disabling grip
    [UdonSynced] private int accessoryID = -1; //ID of accessory | Used for enabling/disabling accessory

    //This method is used for setting the scope on the gun through the attachment system
    //Enables the correct scope and disables the rest
    public void SetScope(int scopeID)
    {
        this.scopeID = scopeID;
        if(scopeID != -1)
        {
            //Enable scope
            attachmentsContainer.GetChild(0).GetChild(scopeID).gameObject.SetActive(true);
            //Disable all other scopes
            for (int i = 0; i < attachmentsContainer.GetChild(0).childCount; i++)
            {
                if(i == scopeID) continue; //Skip the scope that is enabled (to prevent disabling it)
                attachmentsContainer.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
        } else
        {
            //Disable all scopes
            for (int i = 0; i < attachmentsContainer.GetChild(0).childCount; i++)
            {
                attachmentsContainer.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //This method is used for setting the grip on the gun through the attachment system
    //Enables the correct grip and disables the rest
    public void SetGrip(int gripID)
    {
        this.gripID = gripID;
        if(gripID != -1)
        {
            //Enable grip
            attachmentsContainer.GetChild(1).GetChild(gripID).gameObject.SetActive(true);
            //Disable all other grips
            for (int i = 0; i < attachmentsContainer.GetChild(1).childCount; i++)
            {
                if(i == gripID) continue; //Skip the grip that is enabled (to prevent disabling it)
                attachmentsContainer.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
        } else
        {
            //Disable all grips
            for (int i = 0; i < attachmentsContainer.GetChild(1).childCount; i++)
            {
                attachmentsContainer.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //This method is used for setting the accessory on the gun through the attachment system
    //Enables the correct accessory and disables the rest
    public void SetAccessory(int accessoryID)
    {
        this.accessoryID = accessoryID;
        if(accessoryID != -1)
        {
            //Enable accessory
            attachmentsContainer.GetChild(2).GetChild(accessoryID).gameObject.SetActive(true);
            //Disable all other accessories
            for (int i = 0; i < attachmentsContainer.GetChild(2).childCount; i++)
            {
                if(i == accessoryID) continue; //Skip the accessory that is enabled (to prevent disabling it)
                attachmentsContainer.GetChild(2).GetChild(i).gameObject.SetActive(false);
            }
        } else
        {
            //Disable all accessories
            for (int i = 0; i < attachmentsContainer.GetChild(2).childCount; i++)
            {
                attachmentsContainer.GetChild(2).GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //This method is used for setting the suppressor on the gun through the attachment system
    //Enables the correct suppressor and disables the rest
    public void ToggleSuppresor()
    {
        if(!suppressorEnabled)
        {
            suppressorEnabled = true;
            attachmentsContainer.GetChild(3).GetChild(0).gameObject.SetActive(true);
        } else
        {
            suppressorEnabled = false;
            attachmentsContainer.GetChild(3).GetChild(0).gameObject.SetActive(false);
        }
    }

    //This method will go through all children objects of the attachmentsContainer and disable them
    //Should only be needed when returning a weapon to the objectPool
    public void ResetAllAttachments()
    {
        //Disable all attachments
        for (int i = 0; i < attachmentsContainer.childCount; i++)
        {
            for(int j = 0; j < attachmentsContainer.GetChild(i).childCount; j++)
            {
                attachmentsContainer.GetChild(i).GetChild(j).gameObject.SetActive(false);
            }
        }

        Debug.Log("Attachments have been reset on weapon.");
    }

    //This method is used for syncing attachments across the network
    public override void OnDeserialization()
    {
        //Sync Scope
        SetScope(this.scopeID);
        //Sync Grip
        SetGrip(this.gripID);
        //Sync Accessory
        SetAccessory(this.accessoryID);
        //Sync Suppressor
        if(suppressorEnabled)
        {
            attachmentsContainer.GetChild(3).GetChild(0).gameObject.SetActive(true);
        } else
        {
            attachmentsContainer.GetChild(3).GetChild(0).gameObject.SetActive(false);
        }
    }
}
