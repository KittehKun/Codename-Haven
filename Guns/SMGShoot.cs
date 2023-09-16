using UdonSharp;
using UnityEngine;
using VRC.Udon;

public class SMGShoot : UdonSharpBehaviour
{
    //Damage and Raycast Variables
    public float Range; //Assigned in Unity inspector based on the gun | Used for raycast
    public int Damage; //Assigned in Unity inspector based on the gun | Used for calculating damage
    private Transform barrel; //Variable used to find the barrel of the gun and later for raycast

    //Audio Variables
    public AudioSource GunShot; //Assigned in Unity inspector
    public AudioSource EmptySound; //Assigned in Unity inspector
    public AudioSource ReloadSound; //Assigned in Unity inspector

    //Ammo Variables
    public int currentAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public int maxAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public bool isReloading = false; //Used for checking if player is reloading or not
    public bool fullAuto; //Used for checking if gun is full auto or not
    public float fullAutoDelay; //Used for setting full auto delay | Assigned in Unity inspector

    //Flags
    public bool isHeld = false; //Used for checking if player is holding the SMG or not

    //Particle System
    public ParticleSystem muzzleFlashFX; //Assigned in Unity | Used for playing the muzzle flash animation

    //Animator
    public Animator smgAnimator; //Assigned in Unity | Used for playing the SMG's animations
    
    void Start()
    {
        //Find the barrel of the gun
        barrel = this.transform.Find("BarrelStart");
        currentAmmo = maxAmmo;

        //By default, disable gravity on the SMG
        this.GetComponent<Rigidbody>().useGravity = false;

        //By default, make collider a trigger
        this.GetComponent<Collider>().isTrigger = true;

        //Disable the SMG's barrel
        barrel.gameObject.SetActive(false);

        //Check if already assigned in Unity
        if (muzzleFlashFX == null)
        {
            Debug.Log("Unable to find assigned Particle System");
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
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading)
        {
            Debug.Log("Player is reloading.");
            Reload();
        }
    }

    //Function to play muzzle flash
    public void PlayMuzzleFlash()
    {
        muzzleFlashFX.Play();
    }

    public void Reload()
    {
        //Play reload sound
        ReloadSound.PlayOneShot(ReloadSound.clip);

        //Set isReloading to true
        isReloading = true;

        currentAmmo = maxAmmo;

        //Set isReloading to false after 2 seconds
        this.GetComponent<UdonBehaviour>().SendCustomEventDelayedSeconds("ResetReloadingFlag", 1f);

        //Play reload animation
        smgAnimator.Play("BeginReload");
    }

    public void ResetReloadingFlag()
    {
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
        if (currentAmmo > 0 && !isReloading) //Check to see if player has ammo and if the SMG is not in a shot delay animation
        {
            Debug.Log("Player fired weapon.");
            fullAuto = true;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Shoot");
        }
        else if (currentAmmo == 0 && !isReloading)
        {
            Debug.Log("Player is out of ammo.");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PlayEmptySound");
        }
        else
        {
            Debug.Log("Player is reloading.");
        }
    }

    public void PlayEmptySound()
    {
        EmptySound.Play();
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
        smgAnimator.Play("Shoot");

        //Play gunshot sound
        GunShot.Play();

        //Play muzzle flash
        PlayMuzzleFlash();

        //Subtract 1 from AmmoCount
        currentAmmo -= 1;

        //Define direction for Ray
        Vector3 direction = Vector3.left;

        //Define RaycastHit for finding data on what the Ray hit | Used in "out" statement of Physics.Raycast method
        //Cast out Ray and output GameObject that the Ray hit
        //Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out HitData, Range) | This line of code returns true or false if the Ray hits something
        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out RaycastHit HitData, Range, LayerMask.GetMask("Enemy"))) //Check to see if Ray hit any colliders
        {
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Call TakeDamage method on enemy
            HitData.transform.gameObject.GetComponent<EnemyScript>().TakeDamage(Damage);
        }

        //If fullAuto is true, call Shoot method again after fullAutoDelay seconds
        if (fullAuto && currentAmmo > 0)
        {
            this.GetComponent<UdonBehaviour>().SendCustomEventDelayedSeconds("Shoot", fullAutoDelay);
        }

    }

}
