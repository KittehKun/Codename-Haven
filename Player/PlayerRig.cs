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
    private VRCPlayerApi.TrackingData playerOrigin; //Player's origin tracking data | Assigned in Update()
    public bool weaponInRig = false;

    void Start()
    {
        localPlayer = Networking.LocalPlayer; //Assign localPlayer to the player who owns this script
        spawnArea = this.transform.gameObject; //Assign spawnArea to the player's rig

        //Rotate the spawnArea 90 degrees on the y axis
        spawnArea.transform.Rotate(0, 90, 0);

        //Disable the mesh renderer for the spawnArea
        spawnArea.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        playerHead = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
        playerOrigin = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Origin);
        float halfYPos = (playerHead.position.y - playerOrigin.position.y) / 2 + 0.25f;

        //Set rig to player's hip bone
        if(localPlayer.GetBonePosition(HumanBodyBones.Hips) == Vector3.zero)
        {
            //Set the spawnArea's position to the player's head position
            spawnArea.transform.position = new Vector3(playerHead.position.x, halfYPos, playerHead.position.z);
            spawnArea.transform.rotation = playerHead.rotation;

            //Offset the spawnArea's position by 0.25 units away from the spawnArea's forward direction
            spawnArea.transform.position += spawnArea.transform.forward * 0.25f;
            //This is a temporary fix for Avatar's without a hip bone
        } else
        {
            spawnArea.transform.position = localPlayer.GetBonePosition(HumanBodyBones.Hips);
            spawnArea.transform.rotation = localPlayer.GetBoneRotation(HumanBodyBones.Hips);
            //Offset the spawnArea's position by 0.25 units away from the spawnArea's forward direction
            spawnArea.transform.position += spawnArea.transform.forward * 0.25f;
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
        //Disable the weapon's collider
        equippedWeapon.GetComponent<Collider>().enabled = false;
        
        //Return the equippedWeapon to the objectPool
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
        }
    }
}
