using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ARShoot : UdonSharpBehaviour
{
    //Damage and Raycast Variables
    public float Range; //Assigned in Unity inspector based on the gun | Used for raycast
    public int Damage; //Assigned in Unity inspector based on the gun | Used for calculating damage
    private Transform barrel; //Variable used to find the barrel of the gun and later for raycast

    //Audio Variables
    public AudioSource GunShot; //Assigned in Unity inspector
    public AudioSource GunEmpty; //Assigned in Unity inspector
    public AudioSource ReloadSound; //Assigned in Unity inspector

    //Ammo Variables
    public int currentAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public int MaxAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count

    //Flags
    public bool fullAuto; //Used for checking if gun is full auto or not
    public bool isReloading = false; //Used for checking if player is reloading or not
    public float fullAutoDelay; //Used for setting full auto delay | Assigned in Unity inspector
    public bool isHeld = false; //Used for checking if player is holding the AR or not

    //Particle System
    public ParticleSystem muzzleFlashFX; //Assigned in Unity inspector | Used for playing muzzle flash particle system

    //Animator
    public Animator arAnimator; //Assigned in Unity inspector | Used for playing AR animations

    //LayerMask Integer
    private int layerNumber = 31; //Used for raycast | 32nd layer is the Enemy layer
    private int layerMask; //Used for raycast | Defined in Start() method

    void Start()
    {
        //Define layerMask
        layerMask = 1 << layerNumber; //Bitwise left shift operator to represent layer number by single bit | 31st layer is the Enemy layer

        //Find the barrel of the gun
        barrel = this.transform.Find("BarrelStart");
        currentAmmo = MaxAmmo;

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
        if (Input.GetKeyDown(KeyCode.E) && currentAmmo < MaxAmmo && !isReloading)
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

    public void Reload()
    {
        //Play reload sound
        ReloadSound.PlayOneShot(ReloadSound.clip);

        //Set isReloading to true
        isReloading = true;

        currentAmmo = MaxAmmo;

        //Set isReloading to false after 2 seconds
        SendCustomEventDelayedSeconds("ResetReloadingFlag", 1f);
    
        //Play reload animation
        arAnimator.Play("BeginReload");
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
        if (currentAmmo > 0 && !isReloading)
        {
            Debug.Log("Player fired weapon.");
            fullAuto = true;
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
        
        //Play gunshot sound
        PlayGunShot();

        //Play muzzle effect FX
        PlayMuzzleFX();

        //Subtract 1 from currentAmmo
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

        if (fullAuto && currentAmmo > 0) //Check to see if gun is full auto and if player has ammo
        {
            //Get this object's UdonBehaviour and do a SendCustomEventDelayedSeconds for Shoot()
            SendCustomEventDelayedSeconds("Shoot", this.fullAutoDelay);
        }
    }

    public void PlayGunShot()
    {
        GunShot.PlayOneShot(GunShot.clip);
    }
}
