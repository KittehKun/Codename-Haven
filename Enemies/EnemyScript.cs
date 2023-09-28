
using UdonSharp;
using UnityEngine;
using UnityEngine.AI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Wrapper.Modules;

//The purpose of this script is to handle enemy logic - will be attached to enemy prefabs
public class EnemyScript : UdonSharpBehaviour
{
    public Collider attackCollider; //This collider will be used to detect players using the OnPlayerTriggerEnter() event from the VRC Api | Assigned in Unity
    private NavMeshAgent agent; //This is the navmesh agent that will be used to move the enemy
    public Vector3 targetDestination; //This is the target destination that the enemy will move to
    public Vector3 spawnLocation; //This is the spawn location of the enemy
    public float enemyRange = 10f; //This is the range that the enemy will attempt to move to
    public int enemyHealth = 100; //This is the health of the enemy

    //Damage Values
    public int damageRangeMin; //This is the minimum damage that the enemy can deal | Assigned in Unity
    public int damageRangeMax; //This is the maximum damage that the enemy can deal | Assigned in Unity
    public float attackResetTime; //This is the time that the enemy will wait before attacking again | Assigned in Unity
    public int damageChance; //This is the chance that the enemy will deal damage to the player | Assigned in Unity

    //Flags
    public bool isMoving = false; //This flag will be used to determine if the enemy is moving
    public bool hasDestination = false; //This flag will be used to determine if the enemy has a target destination
    public bool attackingPlayer = false; //This flag will be used to determine if the enemy is attacking a player
    public bool isWaiting = false; //This flag will be used to determine if the enemy is waiting at a destination
    private bool alreadyAttacked = false; //This flag will be used to determine if the enemy has already attacked the player
    
    public bool isDead = false; //This flag will be used to determine if the enemy is dead

    //PlayerHitbox Script
    public PlayerStats playerStats; //Assigned in Unity | This script will be used to damage the player

    //VRCPlayer API
    private VRCPlayerApi localPlayer; //This is the local player | Assigned by Collider trigger

    //Blood Particle Systems
    public ParticleSystem muzzleFlashFX; //This is the particle system that will be used to spawn muzzle flash particles from the gun | Assigned in Unity
    public ParticleSystem headBloodSplatter; //This is the particle system that will be used to spawn blood particles | Assigned in Unity
    public ParticleSystem bodyBloodSplatter; //This is the particle system that will be used to spawn blood particles | Assigned in Unity

    //Enemy Animator
    public Animator enemyAnimator; //This is the animator that will be used to animate the enemy | Assigned in Unity

    //Audio Sources
    public Transform spottedSFXContainer; //Transform container for the spotted audio sources | Assigned in Unity
    public Transform deathSFXContainer; //Transform container for the death audio sources | Assigned in Unity
    private AudioSource[] spottedSFXs; //Array of spotted audio sources | Assigned in Start()
    private AudioSource[] deathSFXs; //Array of death audio sources | Assigned in Start()
    public AudioSource[] hitSFXs; //Array of hit audio sources | Assigned in Unity
    public AudioSource[] missSFXs; //Array of miss audio sources | Assigned in Unity
    public AudioSource weaponSFX; //Weapon audio source for enemy | Assigned in Unity

    //Spawn Locations on Death
    public Transform spawnLocationContainer; //Transform with all enemy spawn locations | Assigned in Unity
    private Transform[] spawnLocations; //Array of spawn locations | Assigned in Start()

    void Start()
    {
        //Set the agent variable
        agent = GetComponent<NavMeshAgent>(); //Just in case it's not set in Unity or it builds incorrectly

        //Set the spawnLocation to the enemy's current position
        spawnLocation = this.transform.position;

        //Get the spotted and death audio sources
        this.spottedSFXs = spottedSFXContainer.GetComponentsInChildren<AudioSource>();
        this.deathSFXs = deathSFXContainer.GetComponentsInChildren<AudioSource>();

        //Set the spawnLocations array to the spawnLocationContainer's childCount
        spawnLocations = spawnLocationContainer.GetComponentsInChildren<Transform>();

        //Guard Code - Check if the NavMesh was baked properly and if the Agent is on the mesh. If not disable the GameObject.
        if (!agent.isOnNavMesh)
        {
            Debug.Log("Agent not on a NavMesh!");
            //Disable the GameObject
            this.gameObject.SetActive(false);
        }

        agent.updatePosition = true; //We will not be updating the agent's position manually | Explicitly set to true here for code visibility
    }

    void Update()
    {
        //If the enemy does not have a target destination, generate a random one
        if (!hasDestination && !isMoving && !attackingPlayer && !isWaiting && !isDead)
        {
            GenerateRandomDestination();
        }

        //If the enemy has a target destination, check if it has reached it
        if (hasDestination)
        {
            //If the enemy has reached its destination, set the hasDestination flag to false
            if (agent.remainingDistance < 1f || agent.acceleration < 0.05f)
            {
                hasDestination = false;
                isMoving = false;

                if (!isWaiting)
                {
                    SendCustomEventDelayedSeconds("ResetMovementFlags", 2f);
                    isWaiting = true;

                    //Play Idle Animation if it's not playing
                    enemyAnimator.Play("Idle");
                }
            }
        }

        if (attackingPlayer && !isDead)
        {
            hasDestination = false;
            isMoving = false;

            //Rotate the enemy to face the player
            Vector3 targetDirection = localPlayer.GetPosition() - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f, 0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    public void ResetMovementFlags()
    {
        hasDestination = false;
        isMoving = false;
        isWaiting = false;
        isWaiting = false;
        //Debug.Log("Agent has reset movement flags.");
    }

    //This function will generate a random destination for the enemy to move to
    public void GenerateRandomDestination()
    {        
        //Generate a random position within a 10 unit radius of the enemy's spawn location
        Vector3 randomPosition = Random.insideUnitSphere * enemyRange;
        randomPosition += spawnLocation;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, enemyRange, 1);

        randomPosition = hit.position;

        //Set the target destination to the random position
        targetDestination = randomPosition;
        hasDestination = true;

        //Tell the agent to look at the target destination
        agent.SetDestination(targetDestination);

        //Set the isMoving flag to true
        isMoving = true;

        //Play the enemy walk animation
        enemyAnimator.Play("Move"); //Transitions into MoveLoop

        Debug.Log("Enemy is moving to a random destination.");
    }


    //This function will be called when a player enters the enemy range collider
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        Debug.Log("Player entered enemy range!");
        hasDestination = false;
        isMoving = false;

        //If the player is not the local player, return
        if (!player.isLocal) return;

        //Set the local player to the player that entered the collider
        localPlayer = player;

        //Play the spotted audio source
        GetSpottedSFX().Play();

        //Set owner of enemy to player
        Networking.SetOwner(player, this.gameObject);
    }

    //This function will be called when a player stays in the collider
    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        //If the player is not the local player, return
        if (!player.isLocal) return;

        if (!alreadyAttacked && !isDead)
        {
            AttackPlayer();
        }
        else
        {
            Debug.Log("Agent is already attacking player.");
        }
    }

    //This function will be called when a player exits the collider
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        //If the player is not the local player, return
        if (!player.isLocal) return;
        attackingPlayer = false;
        agent.isStopped = false;
        GenerateRandomDestination();
    }

    //This function will be called by Agent to attack player
    public void AttackPlayer()
    {
        //If the player is not the local player, return
        if (!Networking.IsOwner(this.gameObject)) return;

        //Play the enemy attack animation
        enemyAnimator.Play("Shoot", -1, 0f);

        //Play the weapon audio source
        weaponSFX.Play();

        //Play the muzzle flash particle system
        muzzleFlashFX.Play();

        //Stop the agent from moving
        agent.isStopped = true;

        //Set the attackingPlayer flag to true
        attackingPlayer = true;

        //Set the alreadyAttacked flag to true
        alreadyAttacked = true;

        //Damage the player based on the damageChance percentage
        if (Random.Range(0, 100) <= damageChance)
        {
            Debug.Log("Enemy has hit the player!");

            //Damage the player from the PlayerHitbox script
            playerStats.TakeDamage(Random.Range(damageRangeMin, damageRangeMax));

            //Play Hit Audio Source
            PlayHitSFX().Play();
        }
        else
        {
            //Play Miss Audio Source
            PlayMissSFX().Play();

            Debug.Log("Enemy has missed the player!");
        }



        Debug.Log($"Player has been attacked and has {playerStats.PlayerHealth} health remaining.");

        SendCustomEventDelayedSeconds("ResetAttackFlag", attackResetTime);
    }

    //This function will be called by the agent to reset the attackingPlayer flag | Should be delayed by 1 second
    public void ResetAttackFlag()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        //Subtract the damage from the enemy's health
        enemyHealth -= damage;

        //If the enemy's health is less than or equal to 0, kill the enemy locally
        if (enemyHealth <= 0 && !isDead)
        {
            //Disable the Capsule Collider
            this.GetComponent<CapsuleCollider>().enabled = false;
            attackCollider.enabled = false;

            //Send a local event to kill the agent
            KillAgent();
        }
    }

    //This function will be used to kill the agent
    public void KillAgent()
    {
        isDead = true; //Set the isDead flag to true
        attackingPlayer = false; //Set the attackingPlayer flag to false
        hasDestination = false; //Set the hasDestination flag to false
        isMoving = false; //Set the isMoving flag to false
        isWaiting = false; //Set the isWaiting flag to false
        isWaiting = false; //Set the waitStarted flag to false
        alreadyAttacked = false; //Set the alreadyAttacked flag to false

        //Stop the Agent
        agent.isStopped = true;

        switch (Random.Range(0, 3)) //Chooses between 0 and 2 | Currently have 3 death animations
        {
            case 0:
                enemyAnimator.Play("DeathOne"); //Headshot Animation
                                                //Play Headshot Blood Particle System
                headBloodSplatter.Play();
                //Play Death Audio Source
                GetDeathSFX().Play();
                break;
            case 1:
                enemyAnimator.Play("DeathTwo"); //Bodyshot Animation
                                                //Play Bodyshot Blood Particle System
                bodyBloodSplatter.Play();
                //Play Death Audio Source
                GetDeathSFX().Play();
                break;
            case 2:
                enemyAnimator.Play("DeathThree"); //Bodyshot Animation
                                                  //Play Bodyshot Blood Particle System
                bodyBloodSplatter.Play();
                //Play Death Audio Source
                GetDeathSFX().Play();
                break;
            default:
                enemyAnimator.Play("DeathOne"); //Headshot Animation
                                                //Play Headshot Blood Particle System
                headBloodSplatter.Play();
                //Play Death Audio Source
                GetDeathSFX().Play();
                break;
        }

        //Respawn the enemy after 180 seconds using the RespawnEnemy() function
        SendCustomEventDelayedSeconds("RespawnEnemy", 180f);
    }

    //This function will be used to respawn the enemy
    public void RespawnEnemy()
    {
        //Reset the attack colliders
        attackCollider.enabled = true;
        this.GetComponent<CapsuleCollider>().enabled = true;

        //Reset agent flags
        hasDestination = false;
        isMoving = false;
        isWaiting = false;
        isWaiting = false;
        alreadyAttacked = false;
        attackingPlayer = false;
        isDead = false;
        
        //Reset enemy health
        enemyHealth = 100;

        //Reset the enemy's position to a random spawn location
        this.transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)].position;

        //Reset the enemy's rotation
        this.transform.rotation = spawnLocations[Random.Range(0, spawnLocations.Length)].rotation;

        //Reset the agent's destination to the enemy's current position
        agent.SetDestination(this.transform.position);

        //Enable the agent
        agent.isStopped = false;

        //Play the enemy idle animation
        enemyAnimator.Play("Idle");

        //Set the spawnLocation to the enemy's current position
        spawnLocation = this.transform.position;

        Debug.Log("Enemy has respawned.");
    }

    //This function will be called by the agent to play the spotted audio source
    private AudioSource GetSpottedSFX()
    {
        //Get a random audio source from the spottedSFXs array
        AudioSource spottedSFX = spottedSFXs[Random.Range(0, spottedSFXs.Length)];

        //Return the spotted audio source
        return spottedSFX;
    }

    //This function will be called by the agent to play the death audio source
    private AudioSource GetDeathSFX()
    {
        //Get a random audio source from the deathSFXs array
        AudioSource deathSFX = deathSFXs[Random.Range(0, deathSFXs.Length)];

        //Return the death audio source
        return deathSFX;
    }

    //This function will be called by the agent to play the hit audio source
    private AudioSource PlayHitSFX()
    {
        //Get a random audio source from the hitSFXs array
        AudioSource hitSFX = hitSFXs[Random.Range(0, hitSFXs.Length)];

        return hitSFX;
    }

    //This function will be called by the agent to play the miss audio source
    private AudioSource PlayMissSFX()
    {
        //Get a random audio source from the missSFXs array
        AudioSource missSFX = missSFXs[Random.Range(0, missSFXs.Length)];

        return missSFX;
    }
}
