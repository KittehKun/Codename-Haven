
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//This script is used to shoot the pistol using a Raycast
public class PistolShoot : UdonSharpBehaviour
{
    //Damage and Raycast Variables
    public float Range; //10 Units
    public int Damage; //25 points to Health
    private Transform barrel; //Variable used to find the barrel of the gun and later for raycast

    //Audio Variables
    public AudioSource GunShot; //Assigned in Unity inspector
    public AudioSource GunEmpty; //Assigned in Unity inspector
    public AudioSource ReloadSound; //Assigned in Unity inspector

    //Ammo Variables
    public int currentAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public int MaxAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public bool isReloading = false;

    //Flags and Timers
    public bool isHeld = false; //This flag is used to check if the player is holding the pistol | Used for checking if the pistol should be returned to the ObjectPool

    //Particle System
    public ParticleSystem muzzleFlashFX; //Assigned in Unity | Used for playing the muzzle flash effect on weapons

    //Animator
    public Animator pistolAnimator; //Assigned in Unity | Used for playing the pistol's animations

    //LayerMask Integer
    private int layerNumber = 31;
    private int layerMask;

    void Start()
    {
        //Define layerMask
        layerMask = 1 << layerNumber; //Bitwise left shift operator to represent layer number by single bit | 31st layer is the Enemy layer

        //Find the barrel of the gun
        barrel = this.transform.Find("BarrelStart");
        currentAmmo = MaxAmmo;

        //By default, disable collider and gravity on the pistol
        /* this.GetComponent<Collider>().enabled = false; */ //This line was causing the issue where Collider could not be locally enabled by Network events
        this.GetComponent<Rigidbody>().useGravity = false;

        //Set Collider to trigger
        this.GetComponent<Collider>().isTrigger = true;

        //Disable the pistol's barrel
        barrel.gameObject.SetActive(false);
    }

    void Update()
    {
        //Define direction for Ray
        Vector3 direction = Vector3.left;

        //Draws Ray when in Debug mode
        Debug.DrawRay(barrel.position, barrel.TransformDirection(direction * Range));

        //Check to see if player is pressing R to reload
        if (Input.GetKeyDown(KeyCode.E) && currentAmmo < MaxAmmo && !isReloading)
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
        if (currentAmmo > 0 && !isReloading)
        {
            Debug.Log("Player fired weapon.");
            Shoot();
        }
        else if (currentAmmo == 0 && !isReloading)
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
        
        //Play gunshot sound
        GunShot.Play();

        //Play muzzle flash particle effect
        PlayMuzzleFlash();

        //Subtract 1 from AmmoCount
        currentAmmo -= 1;

        //Define direction for Ray
        Vector3 direction = Vector3.left;

        //Define RaycastHit for finding data on what the Ray hit | Used in "out" statement of Physics.Raycast method
        //Cast out Ray and output GameObject that the Ray hit
        //Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out HitData, Range) | This line of code returns true or false if the Ray hits something
        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out RaycastHit HitData, Range, layerMask, QueryTriggerInteraction.Ignore)) //Check to see if Ray hit any colliders
        {
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Call TakeDamage method on enemy
            HitData.transform.gameObject.GetComponent<EnemyScript>().TakeDamage(Damage);
        }
    }

    //Small method to use for network events
    public void PlayEmptySound()
    {
        GunEmpty.Play();
    }

    //Method used to reload the gun
    public void Reload()
    {
        isReloading = true;

        //Play reload sound
        ReloadSound.PlayOneShot(ReloadSound.clip);

        //Reset AmmoCount
        currentAmmo = MaxAmmo;

        this.SendCustomEventDelayedSeconds("ResetReloadingFlag", 0.33f);

        pistolAnimator.Play("BeginReload");
    }

    public void ResetReloadingFlag()
    {
        isReloading = false;
    }
}
