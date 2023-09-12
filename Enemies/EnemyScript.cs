
using UdonSharp;
using UnityEngine;
using UnityEngine.AI;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to handle enemy logic - will be attached to enemy prefabs
public class EnemyScript : UdonSharpBehaviour
{
    //Enemy Movement and Health
    public NavMeshAgent agent; //NavMeshAgent component | Assigned in Unity
    public float range; //Range of enemy | Assigned in Unity
    public Transform centerPoint; //Center of the area the agents wants to move around | Assigned in Unity
    public int enemyHealth = 100; //Used to track enemy health | Variable is public to display in inspector
    private Transform spawnLocation; //Used to track the enemy's spawn location | Assigned in Unity
    
    //Flags
    public bool isMoving = false; //Used to check if enemy is moving | Variable is public to display in inspector
    public bool isWaiting = false; //Used to check if enemy is waiting | Variable is public to display in inspector
    public bool isFighting = false; //Used to check if enemy is fighting | Variable is public to display in inspector
    
    //Triggers
    public Collider playerHitbox; //Used to check if player is in range of enemy | Assigned in Unity
    
    void Awake()
    {
        //Set spawnLocation to the enemy's spawn location
        spawnLocation = this.transform;
    }
    
    void Update()
    {
        if(!isMoving && !isWaiting && !isFighting)
        {
            //Move to random point
            MoveToRandomPoint();
        }

        if(isMoving)
        {
            //Check to see if agent is at the destination
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                //Set isMoving to false
                isMoving = false; //Agent reached the destination
                isWaiting = true; //Agent will wait for 5 seconds before moving again
                AgentWait();
            }
        }

    }

    //Method is used to find a random point on the NavMesh | Returns the closest point on the NavMesh to the random point and returns true if it finds a point
    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        //Get random point in range
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        //Check to see if point is on NavMesh
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            //Set result to randomPoint
            result = hit.position;
            //Return true
            return true;
        }
        else
        {
            //Set result to Vector3.zero
            result = Vector3.zero;
            //Return false
            return false;
        }
    }

    //Method is used to move the agent to a random point on the NavMesh
    public void MoveToRandomPoint()
    {
        //Define Vector3 for random point
        Vector3 randomPoint;
        //Check to see if RandomPoint method returns true
        if (RandomPoint(centerPoint.position, range, out randomPoint))
        {
            //Set agent's destination to randomPoint
            agent.SetDestination(randomPoint);
            isMoving = true;
        }
    }

    public void AgentWait()
    {
        this.SendCustomEventDelayedSeconds("MoveToRandomPoint", 5.0f); //Call MoveToRandomPoint after 5 seconds
        this.SendCustomEventDelayedSeconds("ResetWaitFlag", 5.0f); //Call ResetWaitFlag after 5 seconds
    }

    public void ResetWaitFlag()
    {
        isWaiting = false;
    }

    public void TakeDamage(int damageAmount)
    {
        //Subtract damageAmount from enemyHealth
        enemyHealth -= damageAmount;
        //Check to see if enemyHealth is less than or equal to 0
        if(enemyHealth <= 0)
        {
            //Call Die method
            Die();
        }
    }

    private void Die()
    {
        //Destroy enemy
        Destroy(this.gameObject);
    }
}
