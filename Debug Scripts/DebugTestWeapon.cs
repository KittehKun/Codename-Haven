
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DebugTestWeapon : UdonSharpBehaviour
{
    public float Range = 10; //10 Units
    public int Damage = 25; //25 points to Health
    public AudioSource GunShot; //Assigned in Unity inspector
    private DebugEnemyHealth EnemyScript; //Purposely not assigned in Unity
    private Transform GunBarrel; //Variable used in place of separate Raycast Script | Consoldiated to one script to work with VRC Pickups script

    void Start()
    {
        GunBarrel = this.transform.Find("RaycastBeginLocation").transform;
        Debug.Log("Gun barrel transform successfully found.");
    }

    void Update()
    {
        //Define direction for Ray
        Vector3 direction = Vector3.left;

        //Draws Ray when in Debug mode
        Debug.DrawRay(GunBarrel.position, GunBarrel.TransformDirection(direction * Range));
    }

    //Function to fire weapon
    public override void OnPickupUseDown()
    {
        Debug.Log("Player fired weapon.");
        Shoot();
        GunShot.Play();        
    }

    //Method used to facilitate bullet logic
    private void Shoot()
    {
        //Define direction for Ray
        Vector3 direction = Vector3.left;

        //Define RaycastHit for finding data on what the Ray hit | Used in "out" statement of Physics.Raycast method
        //Cast out Ray and output GameObject that the Ray hit
        //Physics.Raycast(GunBarrel.position, GunBarrel.TransformDirection(direction * Range), out HitData, Range) | This line of code returns true or false if the Ray hits something
        if (Physics.Raycast(GunBarrel.position, GunBarrel.TransformDirection(direction * Range), out RaycastHit HitData, Range)) //Check to see if Ray hit any colliders
        {
            //Try/Catch not possible with Udon.
            //If above is true, check to see if RaycastHit object is an enemy with EnemyScript
            if(HitData.transform.GetComponent(typeof(UdonBehaviour)) != null)
            {
                EnemyScript = (DebugEnemyHealth)HitData.transform.GetComponent(typeof(UdonBehaviour)); //Assigns script variable after it's been found on Enemy
                Debug.Log($"{HitData.transform.gameObject.name} was found and hit."); //Successfully finds BasicEnemy
                EnemyScript.TakeDamage(Damage);
            }
        }
    }
}
