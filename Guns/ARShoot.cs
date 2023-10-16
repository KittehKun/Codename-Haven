using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class ARShoot : UdonSharpBehaviour
{
    //Damage and Raycast Variables
    private readonly float Range = 75; //Assigned in Unity inspector based on the gun | Used for raycast
    public int Damage; //Assigned in Unity inspector based on the gun | Used for calculating damage
    [SerializeField] private Transform barrel; //Variable used to find the barrel of the gun and later for raycast

    //Audio Variables
    public AudioSource GunShot; //Assigned in Unity inspector
    public AudioSource SuppressedShot; //Assigned in Unity inspector
    public AudioSource GunEmpty; //Assigned in Unity inspector
    public AudioSource ReloadSound; //Assigned in Unity inspector

    //Ammo Variables
    public int currentMagazineAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    private int currentTotalAmmo;
    public int MagazineCapacity; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    private int MaxAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count

    //Ammo Text Fields
    public Text AmmoCount; //Assigned in Unity inspector | Used for displaying ammo count
    
    //Flags
    public bool fullAuto; //Used for checking if gun is full auto or not
    public bool isReloading = false; //Used for checking if player is reloading or not
    public float fullAutoDelay; //Used for setting full auto delay | Assigned in Unity inspector
    public bool isHeld = false; //Used for checking if player is holding the AR or not

    //Particle System
    public ParticleSystem muzzleFlashFX; //Assigned in Unity inspector | Used for playing muzzle flash particle system
    public ParticleSystem suppressedMuzzleFlashFX; //Assigned in Unity inspector | Used for playing muzzle flash particle system

    //Animator
    public Animator arAnimator; //Assigned in Unity inspector | Used for playing AR animations

    //LayerMask Integer
    private int layerNumber = 31; //Used for raycast | 32nd layer is the Enemy layer
    private int layerMask; //Used for raycast | Defined in Start() method

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

        //Set current ammo to magazine capacity
        currentMagazineAmmo = MagazineCapacity;
        MaxAmmo = MagazineCapacity * 10; //Set max ammo to 10x magazine capacity
        currentTotalAmmo = MaxAmmo; //Set current total ammo to max ammo

        UpdateText();

        //By default, disable gravity on the AR
        this.GetComponent<Rigidbody>().useGravity = false;

        //By default, make collider a trigger
        this.GetComponent<Collider>().isTrigger = true;

        //Disable the AR's barrel
        barrel.gameObject.SetActive(false);

        //Assign muzzle flash if it has not already been assigned in Unity
        if (muzzleFlashFX == null)
        {
            muzzleFlashFX = this.transform.Find("FX_Gunshot_01").transform.GetComponent<ParticleSystem>();
        }
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

    //Play Muzzle Flash effect
    public void PlayMuzzleFX()
    {
        muzzleFlashFX.Play();
    }

    //Play Suppressed Muzzle Flash effect
    public void PlaySuppressedMuzzleFX()
    {
        suppressedMuzzleFlashFX.Play();
    }

    public void Reload()
    {
        //Play reload sound
        ReloadSound.PlayOneShot(ReloadSound.clip);

        //Set isReloading to true
        isReloading = true;

        //Reset AmmoCount text
        if(currentTotalAmmo >= MagazineCapacity) //If currentTotalAmmo is greater than or equal to MagazineCapacity
        {
            currentTotalAmmo -= MagazineCapacity - currentMagazineAmmo;
            currentMagazineAmmo = MagazineCapacity;
        }
        else //If current total ammo is less than MagazineCapacity
        {
            currentMagazineAmmo = currentTotalAmmo;
            currentTotalAmmo = 0;
        }

        UpdateText();

        //Set isReloading to false after 2 seconds
        SendCustomEventDelayedSeconds("ResetReloadingFlag", 1f);
    
        //Play reload animation
        arAnimator.Play("BeginReload");
    }

    //Refill ammo method - Called by PlayerRaidManager on raid end
    public void RefillAmmo()
    {
        //Refill ammo
        currentTotalAmmo = MaxAmmo;
        currentMagazineAmmo = MagazineCapacity;

        UpdateText();
    }

    public void ResetReloadingFlag()
    {
        //Set isReloading to false
        isReloading = false;
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
            fullAuto = true;
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

    public void PlayEmptySound()
    {
        GunEmpty.PlayOneShot(GunEmpty.clip);
    }

    public override void OnPickupUseUp()
    {
        //Reset flags
        fullAuto = false; //Disables full auto
    }

    //Method used to facilitate bullet logic
    public void Shoot()
    {
        //Play Shoot animation
        arAnimator.Play("Shoot");
        
        //Play gunshot sound depending on if the weapon is suppressed or not
        bool isSuppressed = (bool) attachmentSystem.GetProgramVariable("suppressorEnabled"); //Get the isSuppressed flag from the attachment system
        if(isSuppressed)
        {
            SuppressedShot.PlayOneShot(SuppressedShot.clip); //Audio Source
            PlaySuppressedMuzzleFX(); //Particle System
        }
        else
        {
            GunShot.PlayOneShot(GunShot.clip); //Audio Source
            PlayMuzzleFX(); //Particle System
        }

        //Play Haptic Event for Object Owner
        Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Right, 0.2f, 0.4f, 0.2f);
        Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Left, 0.2f, 0.4f, 0.2f);

        //Subtract 1 from currentAmmo
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
            //Call TakeDamage method on enemy
            EnemyScript enemy = HitData.transform.gameObject.GetComponent<EnemyScript>();
            if(enemy) enemy.TakeDamage(Damage);
        }

        if (fullAuto && currentMagazineAmmo > 0) //Check to see if gun is full auto and if player has ammo
        {
            //Get this object's UdonBehaviour and do a SendCustomEventDelayedSeconds for Shoot()
            SendCustomEventDelayedSeconds("Shoot", this.fullAutoDelay);
        }
    }

    public void PlayGunShot()
    {
        GunShot.PlayOneShot(GunShot.clip);
    }

    private void UpdateText()
    {
        //Update AmmoCount text
        AmmoCount.text = $"{currentMagazineAmmo}/{currentTotalAmmo}";
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        Debug.Log("OnPlayerLeft was called."); //Event is called but does not work as expected | Need to try removing the Utilities.IsValid(player) check and see if that works
        if(Utilities.IsValid(player)) return; //If player is valid, return
        Debug.Log("Player is no longer valid.");
        if(!Networking.IsMaster) return; //If local player is not master, return
        Debug.Log("Local player is master.");
        if(ownerID != player.playerId) return; //If player is not owner, return

        Debug.Log($"Player is local player AND master. Returning {this.gameObject.name} to pool.");

        attachmentSystem.ResetAllAttachments(); //Reset all attachments on the AR
        Debug.Log($"Reset all attachments on {this.gameObject.name}.");
        VRCObjectPool weaponPool = this.transform.parent.gameObject.GetComponent<VRCObjectPool>(); //Get the weapon pool
        Networking.SetOwner(Networking.LocalPlayer, this.transform.parent.gameObject); //Sets the owner to the object pool as weapons are parented under the object pool
        weaponPool.Return(this.gameObject); //Return the weapon to the object pool

        Debug.Log($"Returned {this.gameObject.name} to pool.");
    }
}
