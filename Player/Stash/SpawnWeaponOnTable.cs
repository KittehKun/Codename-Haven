
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

//The purpose of this script is to spawn a weapon on the table when the player interacts a gun from their stash
public class SpawnWeaponOnTable : UdonSharpBehaviour
{
    //Initialize all variables needed for inventory checking
    public PlayerInventory playerInventory; //Assigned in Unity | Used for checking player's inventory
    public PlayerRig playerRig; //Assigned at Start | Used for spawning the weapon on the table

    // GameObject
    public Transform gunPrefabs; //Assigned in Unity | Used for spawning guns on table using Instantiate()
    private Transform pistols; //First child from gunPrefabs
    private Transform smgs; //Second child from gunPrefabs
    private Transform ars; //Third child from gunPrefabs
    private Transform shotguns; //Fourth child from gunPrefabs
    private Transform snipers; //Fifth child from gunPrefabs
    public Transform playerSpawnedWeapons; //Assigned in Unity | Acts as a container for all player spawns
    public Transform gunSpawnPoint; //Assigned in Unity | Position of where the gun will spawn on the table
    public int categoryChoice; //Assigned in Unity based on weaapon | Used for checking which weapon the player is trying to spawn
    // 0 = Pistol | 1 = SMG | 2 = AR | 3 = Shotgun | 4 = Sniper
    public VRCObjectPool objectPool; //NEEDS to be assigned in Unity based on the gun | Used for spawning guns on table using TryToSpawn()
    public int weaponChoice; //Assigned in Unity based on weapon | Used for checking which weapon the player is trying to spawn | NEEDS to follow the PlayerInventory array structure
    public AudioSource equipSound; //Assigned in Unity | Used for playing the equip sound when the player spawns a weapon

    void Start()
    {
        this.pistols = gunPrefabs.GetChild(0); //Get the first child from gunPrefabs
        this.smgs = gunPrefabs.GetChild(1); //Get the second child from gunPrefabs
        this.ars = gunPrefabs.GetChild(2); //Get the third child from gunPrefabs
        this.shotguns = gunPrefabs.GetChild(3); //Get the fourth child from gunPrefabs
        this.snipers = gunPrefabs.GetChild(4); //Get the fifth child from gunPrefabs
        this.playerRig = GameObject.Find("PlayerRig").GetComponent<PlayerRig>(); //Get the PlayerRig from the PlayerRig GameObject");
    }

    //When a player interacts with a certain weapon, the weapon will spawn on the table only if the player has the weapon in their stash inventory
    public override void Interact()
    {
        switch (categoryChoice)
        {
            case 0:
                SpawnPistol(weaponChoice);
                break;
            case 1:
                SpawnSMG(weaponChoice);
                break;
            case 2:
                SpawnAR(weaponChoice);
                break;
            case 3:
                SpawnShotgun(weaponChoice);
                break;
            case 4:
                SpawnSniper(weaponChoice);
                break;
            default:
                Debug.Log("Something went wrong. KittehKun fix your code.");
                break;
        }
    }

    public void SpawnPistol(int weaponChoice)
    {
        //ARRAY IS AS FOLLOWS:
        // M1911 | Glock | Beretta | USP | Deagle | Revolver | TEC9
        Debug.Log("Spawning pistol...");
        switch (weaponChoice)
        {
            //M1911
            case 0:
                if (playerInventory.PlayerOwnedPistols[0] > 0)
                {
                    playerInventory.PlayerOwnedPistols[0]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this pistol. Pool is untouched.");
                }
                break;
            //Glock
            case 1:
                if (playerInventory.PlayerOwnedPistols[1] > 0)
                {
                    playerInventory.PlayerOwnedPistols[1]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this pistol. Pool is untouched.");
                }
                break;
            //Beretta
            case 2:
                if (playerInventory.PlayerOwnedPistols[2] > 0)
                {
                    playerInventory.PlayerOwnedPistols[2]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this pistol. Pool is untouched.");
                }
                break;
            //USP
            case 3:
                if (playerInventory.PlayerOwnedPistols[3] > 0)
                {
                    playerInventory.PlayerOwnedPistols[3]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this pistol. Pool is untouched");
                }
                break;
            //Deagle
            case 4:
                if (playerInventory.PlayerOwnedPistols[4] > 0)
                {
                    playerInventory.PlayerOwnedPistols[4]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this pistol.");
                }
                break;
            //Revolver
            case 5:
                if (playerInventory.PlayerOwnedPistols[5] > 0)
                {
                    playerInventory.PlayerOwnedPistols[5]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this pistol. Pool is untouched.");
                }
                break;
            //TEC9
            case 6:
                if (playerInventory.PlayerOwnedPistols[6] > 0)
                {
                    playerInventory.PlayerOwnedPistols[6]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this pistol. Pool is untouched.");
                }
                break;
            default:
                Debug.Log("Attempted to spawn pistol but couldn't find the pistol.");
                break;
        }
    }

    private void SpawnSMG(int weaponChoice)
    {
        //ARRAY IS AS FOLLOWS:
        // UZI | MP5 | MP9 | P90 | Vector

        Debug.Log("Spawning SMG...");
        switch (weaponChoice)
        {
            //UZI
            case 0:
                if (playerInventory.PlayerOwnedSMGs[0] > 0)
                {
                    playerInventory.PlayerOwnedSMGs[0]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this SMG. Pool is untouched");
                }
                break;
            //MP5
            case 1:
                if (playerInventory.PlayerOwnedSMGs[1] > 0)
                {
                    playerInventory.PlayerOwnedSMGs[1]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn SMG in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this SMG. Pool is untouched.");
                }
                break;
            //MP9
            case 2:
                if (playerInventory.PlayerOwnedSMGs[2] > 0)
                {
                    playerInventory.PlayerOwnedSMGs[2]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn SMG in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this SMG. Pool is untouched.");
                }
                break;
            //P90
            case 3:
                if (playerInventory.PlayerOwnedSMGs[3] > 0)
                {
                    playerInventory.PlayerOwnedSMGs[3]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this SMG. Pool is untouched.");
                }
                break;
            //Vector
            case 4:
                if (playerInventory.PlayerOwnedSMGs[4] > 0)
                {
                    playerInventory.PlayerOwnedSMGs[4]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn pistol in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this SMG. Pool is untouched.");
                }
                break;
            default:
                Debug.Log("Attempted to spawn SMG but couldn't find the SMG.");
                break;
        }
    }

    private void SpawnAR(int weaponChoice)
    {
        //ARRAY IS AS FOLLOWS:
        // M4 | AK | G36 | LVOA | AUG | M16

        Debug.Log("Spawning AR...");
        switch (weaponChoice)
        {
            //M4
            case 0:
                if (playerInventory.PlayerOwnedARs[0] > 0)
                {
                    playerInventory.PlayerOwnedARs[0]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn AR in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this AR. Pool is untouched.");
                }
                break;
            //AK
            case 1:
                if (playerInventory.PlayerOwnedARs[1] > 0)
                {
                    playerInventory.PlayerOwnedARs[1]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn AR in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this AR. Pool is untouched.");
                }
                break;
            //G36
            case 2:
                if (playerInventory.PlayerOwnedARs[2] > 0)
                {
                    playerInventory.PlayerOwnedARs[2]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn AR in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();

                }
                else
                {
                    Debug.Log("Player does not own this AR. Pool is untouched.");
                }
                break;
            //LVOA
            case 3:
                if (playerInventory.PlayerOwnedARs[3] > 0)
                {
                    playerInventory.PlayerOwnedARs[3]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn AR in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this AR. Pool is untouched.");
                }
                break;
            //AUG
            case 4:
                if (playerInventory.PlayerOwnedARs[4] > 0)
                {
                    playerInventory.PlayerOwnedARs[4]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn AR in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this AR. Pool is untouched.");
                }
                break;
            //M16
            case 5:
                if (playerInventory.PlayerOwnedARs[5] > 0)
                {
                    playerInventory.PlayerOwnedARs[5]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn AR in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this AR. Pool is untouched.");
                }
                break;
            default:
                Debug.Log("Attempted to spawn AR but couldn't find the AR.");
                break;
        }
    }

    private void SpawnShotgun(int weaponChoice)
    {
        //ARRAY IS AS FOLLOWS:
        // M590A1 | DoubleBarrel | M1014 | SPAS12 | AA12

        Debug.Log("Spawning shotgun...");
        switch (weaponChoice)
        {
            //M590A1
            case 0:
                if (playerInventory.PlayerOwnedShotguns[0] > 0)
                {
                    playerInventory.PlayerOwnedShotguns[0]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn shotgun in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this shotgun. Pool is untouched.");
                }
                break;
            //DoubleBarrel
            case 1:
                if (playerInventory.PlayerOwnedShotguns[1] > 0)
                {
                    playerInventory.PlayerOwnedShotguns[1]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn shotgun in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this shotgun. Pool is untouched.");
                }
                break;
            //M1014
            case 2:
                if (playerInventory.PlayerOwnedShotguns[2] > 0)
                {
                    playerInventory.PlayerOwnedShotguns[2]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn shotgun in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this shotgun. Pool is untouched.");
                }
                break;
            //SPAS12
            case 3:
                if (playerInventory.PlayerOwnedShotguns[3] > 0)
                {
                    playerInventory.PlayerOwnedShotguns[3]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn shotgun in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();

                }
                else
                {
                    Debug.Log("Player does not own this shotgun. Pool is untouched.");
                }
                break;
            //AA12
            case 4:
                if (playerInventory.PlayerOwnedShotguns[4] > 0)
                {
                    playerInventory.PlayerOwnedShotguns[4]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn shotgun in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();

                }
                else
                {
                    Debug.Log("Player does not own this shotgun. Pool is untouched.");
                }
                break;
        }
    }

    private void SpawnSniper(int weaponChoice)
    {
        //ARRAY IS AS FOLLOWS:
        // HuntingRifle | Winchester | SVD | AWP | RSASS | 50Cal

        Debug.Log("Spawning sniper...");
        switch (weaponChoice)
        {
            //HuntingRifle
            case 0:
                if (playerInventory.PlayerOwnedSnipers[0] > 0)
                {
                    playerInventory.PlayerOwnedSnipers[0]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn sniper in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this sniper. Pool is untouched.");
                }
                break;
            //Winchester
            case 1:
                if (playerInventory.PlayerOwnedSnipers[1] > 0)
                {
                    playerInventory.PlayerOwnedSnipers[1]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn sniper in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this sniper. Pool is untouched.");
                }
                break;
            //SVD
            case 2:
                if (playerInventory.PlayerOwnedSnipers[2] > 0)
                {
                    playerInventory.PlayerOwnedSnipers[2]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn sniper in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this sniper. Pool is untouched.");
                }
                break;
            //AWP
            case 3:
                if (playerInventory.PlayerOwnedSnipers[3] > 0)
                {
                    playerInventory.PlayerOwnedSnipers[3]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped
                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool
                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn sniper in rig and set the objectPool in the PlayerRig
                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this sniper. Pool is untouched.");
                }
                break;
            //RSASS
            case 4:
                if (playerInventory.PlayerOwnedSnipers[4] > 0)
                {
                    playerInventory.PlayerOwnedSnipers[4]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun

                    //Check if player already has a weapon equipped

                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool

                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn sniper in rig and set the objectPool in the PlayerRig

                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this sniper. Pool is untouched.");
                }
                break;
            //50Cal
            case 5:
                if (playerInventory.PlayerOwnedSnipers[5] > 0)
                {
                    playerInventory.PlayerOwnedSnipers[5]--;

                    Networking.SetOwner(Networking.LocalPlayer, objectPool.gameObject); //Sets owner of the ObjectPool to LocalPlayer to allow anyone to spawn a gun


                    //Check if player already has a weapon equipped

                    if (playerRig.WeaponAlreadyEquipped())
                    {
                        //If player has a weapon equipped, return the weapon to the objectPool

                        playerRig.ReturnWeaponToPool();
                    }

                    //Spawn sniper in rig and set the objectPool in the PlayerRig

                    playerRig.SetObjectPool(objectPool);
                    playerRig.SpawnWeapon();

                    //Play the equip sound
                    PlayEquipSound();
                }
                else
                {
                    Debug.Log("Player does not own this sniper. Pool is untouched.");
                }
                break;
            default:
                Debug.Log("Attempted to spawn sniper but couldn't find the sniper.");
                break;
        }
    }

    //This sound will play when the player spawns a weapon
    public void PlayEquipSound()
    {
        equipSound.Play();
    }
}
