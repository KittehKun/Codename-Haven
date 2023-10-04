using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

//The purpose of this script is to handle the player's rig and handle spawning the equipped weapon

public class PlayerRig : UdonSharpBehaviour
{
    private GameObject spawnArea; //Spawn area for the player's rig | Assigned in Unity inspector
    private VRCPlayerApi localPlayer; //Local player | Assigned in Start()
    private VRCObjectPool objectPool; //ObjectPool for the player's rig | Assigned in Unity inspector
    private GameObject equippedWeapon = null; //Equipped weapon | Assigned in Unity inspector
    private VRCPlayerApi.TrackingData playerHead; //Player's head tracking data | Assigned in Update()
    public bool weaponInRig = false;

    //ID of the player who owns the pistol | Will be used for returning pistols to the ObjectPool when the player leaves
    public int ownerID = 0; //0 = First player to start world

    void Start()
    {
        localPlayer = Networking.LocalPlayer; //Assign localPlayer to the player who owns this script
        spawnArea = this.transform.gameObject; //Assign spawnArea to the player's rig

        //Disable the mesh renderer for the spawnArea
        spawnArea.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        //Assign the player's head tracking data to a variable
        playerHead = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

        //Update rotation of the spawnArea to match the player's head rotation
        spawnArea.transform.rotation = playerHead.rotation;
        
        //Check if the player is holding "H" on the keyboard
        if(Input.GetKey(KeyCode.H))
        {
            //Move the spawnArea in front of the player's view
            spawnArea.transform.position = playerHead.position + (playerHead.rotation * new Vector3(0, 0, 0.5f));
        } else
        {
            //Check if the player is not the local player
            if(Networking.LocalPlayer != localPlayer)
            {
                //Move the spawnArea to the player's back by taking the average position of the player's head and player origin
                Vector3 playerOriginPos = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Origin).position;

                //Offset the playerOriginPos by 0.5 on the x-axis
                playerOriginPos.x += 0.5f;

                spawnArea.transform.position = (playerHead.position + playerOriginPos) / 2;
            } else
            {
                //Move the spawnArea behind the player's head
                spawnArea.transform.position = playerHead.position - (playerHead.rotation * new Vector3(0, 0, 0.5f));
            }
            
        }
        
        if(weaponInRig && equippedWeapon != null && !(bool) equippedWeapon.GetComponent<UdonBehaviour>().GetProgramVariable("isHeld"))
        {
            //Set the position of the equippedWeapon to the spawnArea's position
            equippedWeapon.transform.position = spawnArea.transform.position;

            //Set only the y-axis rotation of the equippedWeapon to the spawnArea's rotation
            equippedWeapon.transform.rotation = Quaternion.Euler(0, spawnArea.transform.rotation.eulerAngles.y, 0);
        }
    }
    
    //Set the objectPool for the player's rig
    public void SetObjectPool(VRCObjectPool pool)
    {
        //Set the objectPool to the pool passed in
        objectPool = pool;
    }

    //Check if the player has a weapon equipped
    public bool WeaponAlreadyEquipped()
    {
        if(equippedWeapon != null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    //Return the equipped weapon to the objectPool
    public void ReturnWeaponToPool()
    {        
        //Check if there is a valid weapon equipped
        if(!WeaponAlreadyEquipped())
        {
            Debug.Log("No weapon equipped. Returning to pool cancelled.");
            return;
        }
        
        //Disable the weapon's collider
        equippedWeapon.GetComponent<Collider>().enabled = false;
        
        //Return the equippedWeapon to the objectPool
        Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject);
        objectPool.Return(equippedWeapon);

        Debug.Log($"Returned {equippedWeapon.name} to pool.");

        //Set the equippedWeapon to null
        equippedWeapon = null;
    }

    //Spawn the weapon from the objectPool if none is equipped | If one is, return that weapon to the objectPool and spawn a new one
    public void SpawnWeapon()
    {
        if(WeaponAlreadyEquipped())
        {
            Debug.Log("Weapon was already equipped. Returning equipped weapon to pool.");
            Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject);
            objectPool.Return(this.equippedWeapon);
            equippedWeapon = null;
            SpawnWeapon();
        }
        else
        {
            Debug.Log("No weapon equipped. Spawning weapon.");
            //Spawn the weapon from the objectPool
            equippedWeapon = objectPool.TryToSpawn();
            
            Networking.SetOwner(Networking.LocalPlayer, this.equippedWeapon);
            weaponInRig = true;
            this.equippedWeapon.GetComponent<Collider>().enabled = true;

            //Set the ownerID to that of the owner of the weapon
            ownerID = Networking.GetOwner(this.equippedWeapon).playerId;
            Debug.Log($"Assigned ownerID of spawned weapon to {ownerID}.");
        }
    }

    //This method will handle returning the weapon to the ObjectPool if the player leaves the world
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        //Once a player leaves, their weapon needs to be returned to the objectPool
        //Ownership of GameObjects SHOULD be transferred to the master of the world
        //If the player who left was the owner of the weapon, return the weapon to the objectPool
        if(player.playerId == ownerID)
        {
            Debug.Log($"Player {player.displayName} [ID: {player.playerId}] left. Returning weapon to pool.");
            ReturnWeaponToPool();
        }
    }
}
