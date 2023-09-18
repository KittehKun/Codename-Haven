
using UdonSharp;
using UnityEngine;
using UnityEngine.AI;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to handle enemy logic - will be attached to enemy prefabs
public class EnemyScript : UdonSharpBehaviour
{
    //public Collider enemyRangeCollider; //This collider will be used to detect players using the OnPlayerTriggerEnter() event from the VRC Api
    public NavMeshAgent agent; //This is the navmesh agent that will be used to move the enemy
    public Vector3 targetDestination; //This is the target destination that the enemy will move to
    public Vector3 spawnLocation; //This is the spawn location of the enemy
    public float enemyRange = 10f; //This is the range that the enemy will attempt to move to
    public int enemyHealth = 100; //This is the health of the enemy

    //Flags
    public bool isMoving = false; //This flag will be used to determine if the enemy is moving
    public bool hasDestination = false; //This flag will be used to determine if the enemy has a target destination
    public bool attackingPlayer = false; //This flag will be used to determine if the enemy is attacking a player
    public bool isWaiting = false; //This flag will be used to determine if the enemy is waiting at a destination
    private bool waitStarted = false; //This flag will be used to determine if the wait has started

    //PlayerHitbox Script
    public PlayerStats playerStats; //Assigned in Unity | This script will be used to damage the player

    //VRCPlayer API
    private VRCPlayerApi localPlayer; //This is the local player | Assigned by Collider trigger

    void Start()
    {
        //Set the spawnLocation to the enemy's current position
        spawnLocation = this.transform.position;
    }

    void Update()
    {
        //If the enemy does not have a target destination, generate a random one
        if (!hasDestination && !isMoving && !attackingPlayer && !isWaiting)
        {
            GenerateRandomDestination();
        }

        //If the enemy has a target destination, check if it has reached it
        if (hasDestination)
        {
            //If the enemy has reached its destination, set the hasDestination flag to false
            if (agent.remainingDistance < 1f || agent.acceleration < 0.01f)
            {
                hasDestination = false;
                isMoving = false;
                isWaiting = true;

                if(!waitStarted)
                {
                    SendCustomEventDelayedSeconds("ResetMovementFlags", 5f);
                    waitStarted = true;
                }
            }
        }

        if(attackingPlayer)
        {
            hasDestination = false;
            isMoving = false;

            //Look at the player
            transform.LookAt(localPlayer.GetPosition());
        }
    }

    public void ResetMovementFlags()
    {
        hasDestination = false;
        isMoving = false;
        isWaiting = false;
    }

    //This function will generate a random destination for the enemy to move to
    public void GenerateRandomDestination()
    {
        //Generate a random position within a 10 unit radius of the enemy's spawn location
        Vector3 randomPosition = Random.insideUnitSphere * enemyRange;
        randomPosition += spawnLocation;

        //Set the target destination to the random position
        targetDestination = randomPosition;
        hasDestination = true;

        //Tell the agent to look at the target destination
        agent.SetDestination(targetDestination);
        agent.updateRotation = true;

        //Set the isMoving flag to true
        isMoving = true;

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

        //Set owner of enemy to player
        Networking.SetOwner(player, this.gameObject);
    }

    //This function will be called when a player stays in the collider
    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        //If the player is not the local player, return
        if (!player.isLocal) return;
        
        if(!attackingPlayer)
        {
            AttackPlayer();
        } else
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

        //Stop the agent from moving
        agent.isStopped = true;

        //Set the attackingPlayer flag to true
        attackingPlayer = true;

        //Damage the player from the PlayerHitbox script
        playerStats.TakeDamage(10);

        Debug.Log($"Player has been attacked and has {playerStats.PlayerHealth} health remaining.");

        SendCustomEventDelayedSeconds("ResetAttackFlag", 1f);
    }

    //This function will be called by the agent to reset the attackingPlayer flag | Should be delayed by 1 second
    public void ResetAttackFlag()
    {
        attackingPlayer = false;
    }

    public void TakeDamage(int damage)
    {
        //Subtract the damage from the enemy's health
        enemyHealth -= damage;

        //If the enemy's health is less than or equal to 0, destroy the enemy
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
