
using UdonSharp;
using UnityEngine;
using VRC.Udon;

public class SniperShoot : UdonSharpBehaviour
{
    //Damage and Raycast Variables
    public float Range; //Assigned in Unity inspector based on the gun | Used for raycast
    public int Damage; //Assigned in Unity inspector based on the gun | Used for calculating damage
    private Transform barrel; //Variable used to find the barrel of the gun and later for raycast    

    //Audio Variables
    public AudioSource GunShot; //Assigned in Unity inspector
    public AudioSource emptySound; //Assigned in Unity inspector
    public AudioSource ReloadSound; //Assigned in Unity inspector

    //Ammo Variables
    public int currentAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public int maxAmmo; //Assigned in Unity inspector based on the gun | Used for checking ammo count
    public Animator sniperAnimator; //Assigned in Unity inspector | Used for playing sniper animations and for shot delay

    //Flags
    public bool isReloading = false; //Used for checking if player is reloading or not
    public bool isHeld = false; //Used for checking if player is holding the sniper or not
    public bool isReadyToFire = true; //Used for checking if the sniper is ready to fire or not

    //Particle System
    public ParticleSystem muzzleFlashFX; //Assigned in Unity | Used for playing the muzzle flash animation

    //LayerMask Integer
    private int layerNumber = 31; //Used for raycast | 32nd layer is the Enemy layer
    private int layerMask; //Used for raycast | Defined in Start() method

    void Start()
    {
        //Define layerMask
        layerMask = 1 << layerNumber; //Bitwise left shift operator to represent layer number by single bit | 31st layer is the Enemy layer

        //Find the barrel of the gun
        barrel = this.transform.Find("BarrelStart");

        //Set currentAmmo to maxAmmo
        currentAmmo = maxAmmo;

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
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading)
        {
            Debug.Log("Player is reloading.");
            Reload();
        }
    }

    public void PlayMuzzleFX()
    {
        muzzleFlashFX.Play();
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
        if (currentAmmo > 0 && !isReloading && isReadyToFire) //Check to see if player has ammo and if the sniper is not in a shot delay animation
        {
            Debug.Log("Player fired weapon.");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Shoot");
            this.isReadyToFire = false;
            SendCustomEventDelayedSeconds("ResetReadyToFireFlag", 0.66f);
        }
        else if (currentAmmo == 0 && !isReloading)
        {
            Debug.Log("Player is out of ammo.");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PlayEmpty");
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

        currentAmmo = maxAmmo;

        //Set isReloading to false after 2 seconds
        this.GetComponent<UdonBehaviour>().SendCustomEventDelayedSeconds("ResetReloadingFlag", 1f);
    
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
        
        //Play gunshot
        GunShot.Play();

        //Play muzzle effect
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
    }

}
