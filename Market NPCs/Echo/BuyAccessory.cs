
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//This script is an all purpose script for buying accessories from Echo the shopkeeper
//This script is attached to the shop items and set in the Unity editor
public class BuyAccessory : UdonSharpBehaviour
{
    //Shop Item Variables
    public int attachmentType; //Attachment type | 0 = Scope | 1 = Suppressor | 2 = Grip | 3 = Accessory | -1 = Remove Attachment
    public int attachmentID; //Attachment ID | Assigned in Unity | Used for setting attachments on guns depending on attachmentType
    public int attachmentPrice; //Attachment price | Assigned in Unity

    //Player Variables
    public PlayerStats playerStats; //PlayerStats | Assigned in Unity | Used for checking player money and updating money GUI
    public PlayerRig playerRig; //PlayerRig | Assigned in Unity | Used for getting the GameObject of the player's gun

    //Attachment System - Used for setting attachments on guns
    private AttachmentSystem attachmentSystem; //AttachmentSystem | Assigned during buy action | Used for setting attachments on guns

    //Audio Sources
    public AudioSource buySFX; //Buy sound | Assigned in Unity | Used for playing buy sound
    public AudioSource[] errorSFXs; //Cant buy sound | Assigned in Unity | Used for playing NPC fail purchase voicelines
    public AudioSource[] noWeaponSFX; //No weapon sound | Assigned in Unity | Used for playing NPC fail purchase voicelines
    public AudioSource[] successSFXs; //Can buy sound | Assigned in Unity | Used for playing NPC success purchase voicelines

    //When a player interacts with an attachment, depending on the attachment type, the player will be able to buy the attachment and enable it on their equipped weapon
    public override void Interact()
    {
        if(CheckIfPlayerCanBuy())
        {    
            //Enable attachment on gun
            EnableAttachment();
        } else if(!playerRig.WeaponAlreadyEquipped())
        {
            //Play Cant Buy Sound and play NPC fail dialogue
            noWeaponSFX[Random.Range(0, errorSFXs.Length)].Play();
        } else
        {
            //Play Cant Buy Sound and play NPC fail dialogue
            errorSFXs[Random.Range(0, errorSFXs.Length)].Play();
        }
        
    }

    private bool CheckIfPlayerCanBuy()
    {
        //Check if player has enough money to buy attachment
        if(playerStats.PlayerMoney >= attachmentPrice && playerRig.WeaponAlreadyEquipped())
        {
            //Player can buy attachment
            return true;
        } else
        {
            //Player cannot buy attachment
            return false;
        }
    }

    private void EnableAttachment()
    {
        //Get attachment system from player's gun
        attachmentSystem = playerRig.GetEquippedWeapon().GetComponent<AttachmentSystem>();

        switch (attachmentType)
        {
            //Scopes
            case 0:
                if(!attachmentSystem.SupportsScope) return; //Check if gun supports scopes
                //Enable the attachment on the gun
                attachmentSystem.SetScope(attachmentID);
                //Play Purchase Sound and play NPC success dialogue
                PlaySuccessAudios();
                break;
            //Suppressors
            case 1:
                attachmentSystem.ToggleSuppresor();
                //Play Purchase Sound and play NPC success dialogue
                PlaySuccessAudios();
                break;
            //Grips
            case 2:
                if(!attachmentSystem.SupportsGrip) return; //Check if gun supports grips
                attachmentSystem.SetGrip(attachmentID);
                //Play Purchase Sound and play NPC success dialogue
                PlaySuccessAudios();
                break;
            //Accessories
            case 3:
                attachmentSystem.SetAccessory(attachmentID);
                //Play Purchase Sound and play NPC success dialogue
                PlaySuccessAudios();
                break;
            //Broken Code
            default:
                Debug.LogError("Attachment type is invalid! KittehKun, your code is broken.");
                break;
        }
    }

    private void PlaySuccessAudios()
    {
        //Play Purchase Sound and play NPC success dialogue
        buySFX.Play();
        successSFXs[Random.Range(0, successSFXs.Length)].Play();
    }
}
