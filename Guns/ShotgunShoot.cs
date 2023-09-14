
using UdonSharp;
using UnityEngine;
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

    void Start()
    {
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
    }

    void Update()
    {
        Vector3 direction = Vector3.left;
        Vector3 direction2 = Vector3.left + new Vector3(0, 0, 0.05f);
        Vector3 direction3 = Vector3.left + new Vector3(0, 0, -0.05f);
        Vector3 direction4 = Vector3.left + new Vector3(0, 0.05f, 0);
        Vector3 direction5 = Vector3.left + new Vector3(0, -0.05f, 0);

        Debug.DrawRay(barrel.position, barrel.TransformDirection(direction * Range));
        Debug.DrawRay(barrel.position, barrel.TransformDirection(direction2 * Range));
        Debug.DrawRay(barrel.position, barrel.TransformDirection(direction3 * Range));
        Debug.DrawRay(barrel.position, barrel.TransformDirection(direction4 * Range));
        Debug.DrawRay(barrel.position, barrel.TransformDirection(direction5 * Range));

        //Check to see if player is pressing R to reload
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading)
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
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Shoot");
        }
        else if (currentAmmo == 0 && !isReloading)
        {
            Debug.Log("Player is out of ammo.");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PlayEmptySound");
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
        this.GetComponent<UdonBehaviour>().SendCustomEventDelayedSeconds("ResetReloadingFlag", 1f);
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
            SendCustomEvent("ResetReadyFlag");
        }

        //Define 5 directions for Ray | Used for shotgun spread
        Vector3 direction = Vector3.left;
        Vector3 direction2 = Vector3.left + Vector3.up;
        Vector3 direction3 = Vector3.left + Vector3.down;
        Vector3 direction4 = Vector3.left + Vector3.right;
        Vector3 direction5 = Vector3.left + Vector3.left;

        //Check all Raycasts for hits
        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction * Range), out RaycastHit HitData, Range, LayerMask.GetMask("Enemy"))) //Check to see if Ray hit any colliders
        {
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Call TakeDamage method on enemy
            HitData.transform.gameObject.GetComponent<EnemyScript>().TakeDamage(Damage);
        }

        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction2 * Range), out RaycastHit HitData2, Range, LayerMask.GetMask("Enemy"))) //Check to see if Ray hit any colliders
        {
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Call TakeDamage method on enemy
            HitData2.transform.gameObject.GetComponent<EnemyScript>().TakeDamage(Damage);
        }

        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction3 * Range), out RaycastHit HitData3, Range, LayerMask.GetMask("Enemy"))) //Check to see if Ray hit any colliders
        {
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Call TakeDamage method on enemy
            HitData3.transform.gameObject.GetComponent<EnemyScript>().TakeDamage(Damage);
        }

        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction4 * Range), out RaycastHit HitData4, Range, LayerMask.GetMask("Enemy"))) //Check to see if Ray hit any colliders
        {
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Call TakeDamage method on enemy
            HitData4.transform.gameObject.GetComponent<EnemyScript>().TakeDamage(Damage);
        }

        if (Physics.Raycast(barrel.position, barrel.TransformDirection(direction5 * Range), out RaycastHit HitData5, Range, LayerMask.GetMask("Enemy"))) //Check to see if Ray hit any colliders
        {
            //With layer mask defined, we can now check to see if the Ray hit an enemy
            //Call TakeDamage method on enemy
            HitData5.transform.gameObject.GetComponent<EnemyScript>().TakeDamage(Damage);
        }
    }
}
