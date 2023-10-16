
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

//This script is used to shoot the pistol using a Raycast
public class PistolShoot : UdonSharpBehaviour
{
    //Damage and Raycast Variables
    private readonly float Range = 30; //Range of the Raycast | For the pistol, this is 30 meters
    public int Damage; //Amount of damage the pistol does
    [SerializeField] private Transform barrel; //Variable used to find the barrel of the gun and later for raycast

    //Audio Variables
    public AudioSource GunShot; //Assigned in Unity inspector
    public AudioSource SuppressedShot; //Assigned in Unity inspector
    public AudioSource GunEmpty; //Assigned in Unity inspector
    public AudioSource ReloadSound; //Assigned in Unity inspector

    //Ammo Variables
    public int currentMagazineAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    private int currentTotalAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public int MagazineCapacity; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    private int MaxAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    private bool isReloading = false;

    //Ammo Text Fields
    public Text AmmoCount; //Assigned in Unity inspector | Used for displaying ammo count

    //Flags
    public bool isHeld = false; //This flag is used to check if the player is holding the pistol | Used for checking if the pistol should be returned to the ObjectPool

    //Particle System
    public ParticleSystem muzzleFlashFX; //Assigned in Unity | Used for playing the muzzle flash effect on weapons
    public ParticleSystem suppressedMuzzleFlashFX; //Assigned in Unity | Used for playing the muzzle flash effect on weapons

    //Animator
    public Animator pistolAnimator; //Assigned in Unity | Used for playing the pistol's animations

    //LayerMask Integer
    private readonly int layerNumber = 31;
    private int layerMask;

    //Object Pool
    [HideInInspector] public int ownerID; //Used for returning the AR to the Object Pool

    //Attachment System - Only used for toggling suppressed or unsuppressed fire
    public AttachmentSystem attachmentSystem; //Assigned in Unity inspector | Used for toggling suppressed or unsuppressed fire

    void Start()
    {
        //Define layerMask
        layerMask = 1 << layerNumber; //Bitwise left shift operator to represent layer number by single bit | 31st layer is the Enemy layer

        //Find the barrel of the gun if not already assigned in Inspector
        if(!barrel) barrel = this.transform.Find("Barrel");

        //Set ammo variables
        currentMagazineAmmo = MagazineCapacity;
        MaxAmmo = currentMagazineAmmo * 10;
        currentTotalAmmo = MaxAmmo;

        //By default, disable collider and gravity on the pistol
        this.GetComponent<Rigidbody>().useGravity = false;

        //Set Collider to trigger
        this.GetComponent<Collider>().isTrigger = true;

        //Disable the pistol's barrel
        barrel.gameObject.SetActive(false);

        //Set the ammo text
        UpdateText();
    }

    void Update()
    {
        //Define direction for Ray
        Vector3 direction = Vector3.left;

        //Draws Ray when in Debug mode
        Debug.DrawRay(barrel.position, barrel.TransformDirection(direction * Range));

        //Check to see if player is pressing R to reload
        if (Input.GetKeyDown(KeyCode.E) && currentMagazineAmmo < MaxAmmo && !isReloading)
        {
            Debug.Log("Player is reloading.");
            Reload();
        }
    }

    //This method will emit the muzzle flash particle effect
    public void PlayMuzzleFlash()
    {
        muzzleFlashFX.Play();
    }

    public void PlaySuppressedMuzzleFlash()
    {
        suppressedMuzzleFlashFX.Play();
    }

    public override void OnPickup()
    {
        //Set the isHeld flag to true
        isHeld = true;
    }

    public override void OnDrop()
    {
        //Set the isHeld flag to false
        isHeld = false;

        //The weapon goes back into the player's rig | This is handled in the PlayerRig.cs script
    }

    //Function to fire weapon
    public override void OnPickupUseDown()
    {
        if (currentMagazineAmmo > 0 && !isReloading)
        {
            Debug.Log("Player fired weapon.");
            Shoot();
        }
        else if (currentMagazineAmmo == 0 && !isReloading)
        {
            Debug.Log("Player is out of ammo.");
            PlayEmptySound();
        }
        else
        {
            Debug.Log("Player is reloading.");
        }
    }

    //Method used to facilitate bullet logic
    public void Shoot()
    {
        //Play Shoot animation
        pistolAnimator.Play("Shoot");
        
        //Play gunshot sound depending on if the weapon is suppressed or not
        bool isSuppressed = (bool) attachmentSystem.GetProgramVariable("suppressorEnabled");
        if(isSuppressed) 
        {
            SuppressedShot.PlayOneShot(SuppressedShot.clip); //Audio Source
            PlaySuppressedMuzzleFlash(); //Particle System
        }
        else
        {
            GunShot.PlayOneShot(GunShot.clip); //Audio Source
            PlayMuzzleFlash(); //Particle System
        }

        //Subtract 1 from AmmoCount
        currentMagazineAmmo -= 1;

        UpdateText();

        //Define direction for Ray
        Vector3 direction = Vector3.left;

        //Define RaycastHit for finding data on what the Ray hit | Used in "out" statement of Physics.Raycast method
        //Cast out Ray and output GameObject that the Ray hit
        //Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out HitData, Range) | This line of code returns true or false if the Ray hits something
        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out RaycastHit HitData, Range, layerMask, QueryTriggerInteraction.Ignore)) //Check to see if Ray hit any colliders
        {
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Define variable for enemy and assign it to the GameObject that the Ray hit | Will return null if Ray cannot get component of EnemyScript
            EnemyScript enemy = HitData.transform.gameObject.GetComponent<EnemyScript>(); //Define enemy variable
            if(enemy) enemy.TakeDamage(Damage);
        }
    }

    //Plays Empty Sound
    public void PlayEmptySound()
    {
        GunEmpty.Play();
    }

    //Reloads the weapon
    public void Reload()
    {        
        isReloading = true;

        //Play reload sound
        ReloadSound.PlayOneShot(ReloadSound.clip);

        //Reset AmmoCount
        if(currentTotalAmmo >= MagazineCapacity) //Checks if there is enough ammo to fill the magazine - otherwise, just fill the magazine with the remaining ammo
        {
            currentTotalAmmo -= MagazineCapacity - currentMagazineAmmo;
            currentMagazineAmmo = MagazineCapacity;
        }
        else
        {
            currentMagazineAmmo = currentTotalAmmo; //Fill the magazine with the remaining ammo - Currently loses the remaining ammo on last magazine
            currentTotalAmmo = 0;
        }

        UpdateText();

        //Reset reloading flag after 0.33 seconds
        SendCustomEventDelayedSeconds("ResetReloadingFlag", 0.33f);

        pistolAnimator.Play("BeginReload");
    }

    //Refill ammo method - Called by PlayerRaidManager on raid end
    public void RefillAmmo()
    {
        //Refill ammo
        currentTotalAmmo = MaxAmmo;
        currentMagazineAmmo = MagazineCapacity;

        UpdateText();
    }

    //Resets the reloading flag | Called by SendCustomEventDelayedSeconds in Reload method
    public void ResetReloadingFlag()
    {
        isReloading = false;
    }

    private void UpdateText()
    {
        AmmoCount.text = $"{currentMagazineAmmo}/{currentTotalAmmo}";
    }

    //Method to return the weapon to transfer ownership of this weapon back to the Object Pool once the player leaves
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if(Utilities.IsValid(player)) return; //If player is valid, return
        if(!Networking.IsMaster) return; //If player is not master, return
        if(ownerID != player.playerId) return; //If player is not owner, return

        VRCObjectPool weaponPool = this.transform.parent.gameObject.GetComponent<VRCObjectPool>(); //Get the weapon pool
        Networking.SetOwner(Networking.LocalPlayer, this.transform.parent.gameObject); //Sets the owner to the object pool as weapons are parented under the object pool
        weaponPool.Return(this.gameObject); //Return the weapon to the object pool
    }
}
