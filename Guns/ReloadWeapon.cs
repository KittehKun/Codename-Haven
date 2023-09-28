
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to call the reload function on the equipped weapon when the player interacts with the weapon's magazine
public class ReloadWeapon : UdonSharpBehaviour
{
    private GameObject equippedWeapon; //Equipped Weapon
    public bool hasMagazine; //This flag will be set to true if the weapon has a magazine child object | Assigned in Unity inspector
    
    void Start()
    {
        //This script will be attached to the weapon's magazine GameObject | If no magazine found as child, empty GameObject will be used and weapon's collider will be used for reload interaction
        //Magazine object will be a child of the gun's parent object
        //Assign the equippedWeapon to the parent object of the magazine
        if(hasMagazine)
        {
            equippedWeapon = this.transform.parent.gameObject;
        }

        //Enable Collider locally | Disabled in Unity inspector by default
        this.GetComponent<Collider>().enabled = false;
    }

    public override void Interact()
    {
        //Call the reload function on the equipped weapon | Triggered by the Collider of the magazine
        equippedWeapon.GetComponent<UdonBehaviour>().SendCustomEvent("Reload");
    }
}
