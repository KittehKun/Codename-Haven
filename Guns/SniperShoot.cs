
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class SniperShoot : UdonSharpBehaviour
{
    //Damage and Raycast Variables
    private readonly float Range = 150; //Assigned in Unity inspector based on the gun | Used for raycast
    public int Damage; //Assigned in Unity inspector based on the gun | Used for calculating damage
    [SerializeField] private Transform barrel; //Variable used to find the barrel of the gun and later for raycast    

    //Audio Variables
    public AudioSource GunShot; //Assigned in Unity inspector
    public AudioSource SuppressedShot; //Assigned in Unity inspector
    public AudioSource emptySound; //Assigned in Unity inspector
    public AudioSource ReloadSound; //Assigned in Unity inspector

    //Ammo Variables
    public int currentMagazineAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    private int currentTotalAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public int MagazineCapacity; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    private int MaxAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public Animator sniperAnimator; //Assigned in Unity inspector | Used for playing sniper animations and for shot delay

    //Ammo Text Fields
    public Text AmmoCount; //Assigned in Unity inspector | Used for displaying ammo count
    
    //Flags
    public bool isReloading = false; //Used for checking if player is reloading or not
    public bool isHeld = false; //Used for checking if player is holding the sniper or not
    public bool isReadyToFire = true; //Used for checking if the sniper is ready to fire or not

    //Particle System
    public ParticleSystem muzzleFlashFX; //Assigned in Unity | Used for playing the muzzle flash animation
    public ParticleSystem suppressedMuzzleFlashFX; //Assigned in Unity | Used for playing the muzzle flash animation

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

        //Find the barrel of the gun
        if(!barrel) barrel = this.transform.Find("Barrel");

        //Set currentAmmo to maxAmmo
        currentMagazineAmmo = MagazineCapacity;
        MaxAmmo = MagazineCapacity * 10; //10 magazines worth of ammo
        currentTotalAmmo = MaxAmmo; //Set currentTotalAmmo to MaxAmmo

        UpdateText();

        //By default, disable collider and gravity on the sniper
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Collider>().isTrigger = true;

        //Disable the barrel
        barrel.gameObject.SetActive(false);

        //Find particle system if it wasn't already assigned in Unity
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

    public void PlayMuzzleFX()
    {
        muzzleFlashFX.Play();
    }

    public void PlaySuppressedMuzzleFlash()
    {
        suppressedMuzzleFlashFX.Play();
    }

    public override void OnPickup()
    {
        //Set isHeld to true
        isHeld = true;
    }

    public override void OnDrop()
    {
        //Set isHeld to false
        isHeld = false;

        //The weapon goes back into the player's rig | This is handled in the PlayerRig.cs script
    }

    //Function to fire weapon
    public override void OnPickupUseDown()
    {
        if (currentMagazineAmmo > 0 && !isReloading && isReadyToFire) //Check to see if player has ammo and if the sniper is not in a shot delay animation
        {
            Debug.Log("Player fired weapon.");
            Shoot();
            isReadyToFire = false;
        }
        else if (currentMagazineAmmo == 0 && !isReloading)
        {
            Debug.Log("Player is out of ammo.");
            PlayEmpty();
        }
        else
        {
            Debug.Log("Player is reloading.");
        }
    }

    public void PlayEmpty()
    {
        emptySound.PlayOneShot(emptySound.clip);
    }

    //Reload Method
    public void Reload()
    {
        //Play reload sound
        ReloadSound.PlayOneShot(ReloadSound.clip);

        //Set isReloading to true
        isReloading = true;

        //Reset AmmoCount
        if(currentTotalAmmo >= MagazineCapacity) //If current total ammo is greater than or equal to magazine capacity
        {
            currentTotalAmmo -= MagazineCapacity - currentMagazineAmmo; //Subtract the difference between magazine capacity and current magazine ammo from current total ammo
            currentMagazineAmmo = MagazineCapacity; //Set current magazine ammo to magazine capacity
        }
        else //If current total ammo is less than magazine capacity
        {
            currentMagazineAmmo += currentTotalAmmo; //Add current total ammo to current magazine ammo
            currentTotalAmmo = 0; //Set current total ammo to 0
        }

        UpdateText();

        //Set isReloading to false after 2 seconds
        SendCustomEventDelayedSeconds("ResetReloadingFlag", 1f);
    
        //Play reload animation
        sniperAnimator.Play("BeginReload");
    }

    public void ResetReloadingFlag()
    {
        //Set isReloading to false
        isReloading = false;
    }

    public void ResetReadyToFireFlag()
    {
        //Set isReadyToFire to false
        isReadyToFire = true;
    }

    //Method used to facilitate bullet logic
    public void Shoot()
    {
        //Play Shoot animation
        sniperAnimator.Play("Shoot");
        
        bool isSuppressed = (bool) attachmentSystem.GetProgramVariable("suppressorEnabled"); //Get isSuppressed variable from attachment system
        if(isSuppressed) //If isSuppressed is true
        {
            //Play suppressed shot sound
            SuppressedShot.PlayOneShot(SuppressedShot.clip);

            //Play suppressed muzzle flash
            PlaySuppressedMuzzleFlash();
        }
        else //If isSuppressed is false
        {
            //Play unsuppressed shot sound
            GunShot.PlayOneShot(GunShot.clip);

            //Play unsuppressed muzzle flash
            PlayMuzzleFX();
        }

        //Subtract 1 from currentAmmo
        currentMagazineAmmo -= 1;

        //Define direction for Ray
        Vector3 direction = Vector3.left;

        //Define RaycastHit for finding data on what the Ray hit | Used in "out" statement of Physics.Raycast method
        //Cast out Ray and output GameObject that the Ray hit
        //Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out HitData, Range) | This line of code returns true or false if the Ray hits something
        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out RaycastHit HitData, Range, layerMask, QueryTriggerInteraction.Ignore)) //Check to see if Ray hit any colliders
        {
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Call TakeDamage method on enemy
            EnemyScript enemy = HitData.transform.gameObject.GetComponent<EnemyScript>(); //Define enemy variable
            if(enemy) enemy.TakeDamage(Damage);
        }

        //Reset ready to fire flag after 0.66 secondsq
        SendCustomEventDelayedSeconds("ResetReadyToFireFlag", 0.66f);
    }

    private void UpdateText()
    {
        //Update AmmoCount text
        AmmoCount.text = $"{currentMagazineAmmo}/{currentTotalAmmo}";
    }

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
