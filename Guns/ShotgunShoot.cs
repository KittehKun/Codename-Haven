
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class ShotgunShoot : UdonSharpBehaviour
{
    //Damage and Raycast Variables
    public float Range; //Assigned in Unity inspector based on the gun | Used for raycast
    public int Damage; //Assigned in Unity inspector based on the gun | Used for calculating damage
    private Transform barrel; //Variable used to find the barrel of the gun and later for raycast

    //Audio Variables
    public AudioSource GunShot; //Assigned in Unity inspector
    public AudioSource EmptyAudio; //Assigned in Unity inspector
    public AudioSource ReloadSound; //Assigned in Unity inspector
    public AudioSource PumpActionSFX; //Assigned in Unity inspector

    //Ammo Variables
    public int currentAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public int maxAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public Animator shotgunAnimator; //Assigned in Unity inspector | Used for playing shotgun animations and for shot delay

    //Flags
    public bool isReloading = false; //Used for checking if player is reloading or not
    public bool isHeld = false; //Used for checking if player is holding the shotgun or not
    public bool isPumpShotgun; //Assigned in Unity | Used for checking if PumpActionSFX should be played
    private bool isReady = true; //Used for checking if shotgun is ready to fire

    //Particle System
    public ParticleSystem muzzleFlashFX; //Assigned in Unity | Used for playing the muzzle flash animation

    //LayerMask Integer
    private int layerNumber = 31; //Used for raycast | 32nd layer is the Enemy layer
    private int layerMask; //Used for raycast | Defined in Start() method

    //Object Pool
    [HideInInspector] public VRCObjectPool objectPool; //Used for returning the AR to the Object Pool
    [HideInInspector] public int ownerID; //Used for returning the AR to the Object Pool

    void Start()
    {
        //Define layerMask
        layerMask = 1 << layerNumber; //Bitwise left shift operator to represent layer number by single bit | 31st layer is the Enemy layer

        //Find the barrel of the gun
        barrel = this.transform.Find("BarrelStart");
        currentAmmo = maxAmmo;

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
        if (Input.GetKeyDown(KeyCode.E) && currentAmmo < maxAmmo && !isReloading)
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
        if (currentAmmo > 0 && !isReloading && isReady) //Check to see if player has ammo and if the shotgun is not in a shot delay animation
        {
            Debug.Log("Player fired weapon.");
            Shoot();
        }
        else if (currentAmmo == 0 && !isReloading)
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

        currentAmmo = maxAmmo;

        //Set isReloading to false after 2 seconds
        SendCustomEventDelayedSeconds("ResetReloadingFlag", 1f);
    
        //Play reload animation
        shotgunAnimator.Play("BeginReload");
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

        //Subtract 1 from currentAmmo
        currentAmmo -= 1;

        //Play gunshot audio
        GunShot.Play();

        //Play muzzle flash
        PlayMuzzleFX();

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
            HitData.transform.gameObject.GetComponent<EnemyScript>().TakeDamage(Damage);
        }

        
    }
}
