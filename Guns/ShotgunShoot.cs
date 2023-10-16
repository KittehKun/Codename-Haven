
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class ShotgunShoot : UdonSharpBehaviour
{
    //Damage and Raycast Variables
    private readonly float Range = 30; //Assigned in Unity inspector based on the gun | Used for raycast
    public int Damage; //Assigned in Unity inspector based on the gun | Used for calculating damage
    [SerializeField] private Transform barrel; //Variable used to find the barrel of the gun and later for raycast

    //Audio Variables
    public AudioSource GunShot; //Assigned in Unity inspector
    public AudioSource SuppressedShot; //Assigned in Unity inspector
    public AudioSource EmptyAudio; //Assigned in Unity inspector
    public AudioSource ReloadSound; //Assigned in Unity inspector
    public AudioSource PumpActionSFX; //Assigned in Unity inspector

    //Ammo Variables
    public int currentMagazineAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    private int currentTotalAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public int MagazineCapacity; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    private int MaxAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public Animator shotgunAnimator; //Assigned in Unity inspector | Used for playing shotgun animations and for shot delay

    //Ammo Text Fields
    public Text AmmoCount; //Assigned in Unity inspector | Used for displaying ammo count
    
    //Flags
    public bool isReloading = false; //Used for checking if player is reloading or not
    public bool isHeld = false; //Used for checking if player is holding the shotgun or not
    public bool isPumpShotgun; //Assigned in Unity | Used for checking if PumpActionSFX should be played
    private bool isReady = true; //Used for checking if shotgun is ready to fire

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

        //Set the current ammo to the magazine capacity
        currentMagazineAmmo = MagazineCapacity;
        MaxAmmo = MagazineCapacity * 10; //Set the max ammo to 10x the magazine capacity
        currentTotalAmmo = MaxAmmo; //Set the current total ammo to the max ammo

        //By default, disable collider and gravity on the shotgun
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Collider>().isTrigger = true;

        //Disable the barrel
        barrel.gameObject.SetActive(false);

        //Check if Muzzle Flash was already assigned in Unity
        if (muzzleFlashFX == null)
        {
            this.transform.Find("FX_Gunshot_01").transform.GetComponent<ParticleSystem>();
        }

        Damage *= 5; //Shotgun does 5x damage simulating 5 pellets. Damage in inspector is for 1 pellet.
    }

    void Update()
    {
        Vector3 direction = Vector3.left;

        Debug.DrawRay(barrel.position, barrel.TransformDirection(direction * Range));

        //Check to see if player is pressing R to reload
        if (Input.GetKeyDown(KeyCode.E) && currentMagazineAmmo < MaxAmmo && !isReloading)
        {
            Debug.Log("Player is reloading.");
            Reload();
        }
    }

    //Method to play Muzzle Flash
    public void PlayMuzzleFX()
    {
        muzzleFlashFX.Play();
    }

    //Method to play suppressed Muzzle Flash
    public void PlaySuppressedMuzzleFX()
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

        //The weapon goes back into the player's rig | This is handled by the PlayerInventory script   
    }

    //Function to fire weapon
    public override void OnPickupUseDown()
    {
        if (currentMagazineAmmo > 0 && !isReloading && isReady) //Check to see if player has ammo and if the shotgun is not in a shot delay animation
        {
            Debug.Log("Player fired weapon.");
            Shoot();
        }
        else if (currentMagazineAmmo == 0 && !isReloading)
        {
            Debug.Log("Player is out of ammo.");
            PlayEmptySound();
        }
        {
            Debug.Log("Player is reloading.");
        }
    }

    public void PlayEmptySound()
    {
        EmptyAudio.Play();
    }

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
        shotgunAnimator.Play("BeginReload");
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

    public void ResetReadyFlag()
    {
        //Set isReady to true
        isReady = true;
    }

    public void PlayPumpSFX()
    {
        PumpActionSFX.Play();
    }

    //Method used to facilitate bullet logic
    public void Shoot()
    {
        //Play Shoot animation
        shotgunAnimator.Play("Shoot");
        
        //Set isReady to false
        isReady = false;

        //Play gunshot sound depending on if the weapon is suppressed or not
        bool isSuppressed = (bool) attachmentSystem.GetProgramVariable("suppressorEnabled");
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

        //Subtract 1 from AmmoCount
        currentMagazineAmmo -= 1;

        UpdateText();

        //Send a custom event delayed by 0.5f
        if (isPumpShotgun)
        {
            SendCustomEventDelayedSeconds("PlayPumpSFX", 0.5f);
            SendCustomEventDelayedSeconds("ResetReadyFlag", 0.6f);
        }
        else
        {
            SendCustomEventDelayedSeconds("ResetReadyFlag", 0.1f);
        }

        //Define 5 directions for Ray | Used for shotgun spread
        Vector3 direction = Vector3.left;

        //Check all Raycasts for hits
        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out RaycastHit HitData, Range, layerMask, QueryTriggerInteraction.Ignore)) //Check to see if Ray hit any colliders
        {            
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Call TakeDamage method on enemy
            EnemyScript enemy = HitData.transform.gameObject.GetComponent<EnemyScript>(); //Define enemy variable
            if(enemy) enemy.TakeDamage(Damage);
        }

    }

    private void UpdateText()
    {
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
