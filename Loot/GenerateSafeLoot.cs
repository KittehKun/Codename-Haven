
using UdonSharp;
using UnityEngine;

//The purpose of this script is to mimic the behavior of the GenerateLoot script, but only spawn extraordinary items. The player MUST have a key in their inventory to open the safe.
public class GenerateSafeLoot : UdonSharpBehaviour
{
    public Transform[] lootSpawnPositions; //Assigned in Unity | Every container NEEDS a spawn location inside of the GameObject for this to work
    private GameObject[] extraordinaryTable;
    private bool isLooted = false; //Used to check if the container has been looted or not
    private Animator safeAnimator; //Used to play the animation of the container opening
    private GameObject spawnedLoot; //Used to store the spawned object under the _SPAWNEDLOOT GameObject
    public AudioSource openSound; //Used to play the sound of the container opening | Assigned in Unity
    public PlayerInventory playerInventory; //Used for checking if the player has a safe key

    void Start()
    {
        safeAnimator = GetComponent<Animator>();

        //Initialize spawnedLoot by finding _SPAWNEDLOOT GameObject in Hierarchy
        spawnedLoot = GameObject.Find("_SPAWNEDLOOT");

        GameObject lootPrefabContainers = GameObject.Find("LootPrefabsContainer");

        //EXTRAORDINARY TABLE CODE BLOCK START
        //Initialize extraordinaryTable with extraordinary item count
        extraordinaryTable = new GameObject[lootPrefabContainers.transform.Find("Valueables").transform.GetChild(3).childCount];
        //Fill extraordinaryTable with all extraordinary item GameObjects
        int index = 0; //Used for the foreach loop
        foreach (Transform transform in lootPrefabContainers.transform.Find("Valueables").transform.GetChild(3).GetComponentInChildren<Transform>())
        {
            extraordinaryTable[index] = transform.gameObject;
            //Debug.Log($"Extraordinary Item: {transform.gameObject.name} added to Extraordinary Table.");
            index++;
        }
    }

    //Interact Method
    public override void Interact()
    {
        //Check if the player has a safe key and if the safe has not been looted
        if (!isLooted && playerInventory.SafeKeys > 0)
        {
            //Change looted to true
            isLooted = true;

            //Subtract a key from the player's inventory
            playerInventory.RemoveSafeKey();

            //Spawn the loot
            SpawnLoot(lootSpawnPositions[0].position);
            SpawnLoot(lootSpawnPositions[1].position);

            //Disable the collider
            GetComponent<Collider>().enabled = false;
        }
    }

    //SpawnLoot Method
    private void SpawnLoot(Vector3 spawnPos)
    {
        //Pick a random item from the extraordinaryTable
        int randomItem = Random.Range(0, extraordinaryTable.Length);

        //Spawn the item at the spawnPos
        GameObject itemToSpawn = extraordinaryTable[randomItem];
        GameObject spawnedItem = Instantiate(itemToSpawn);
        spawnedItem.transform.position = spawnPos;
        spawnedItem.transform.SetParent(spawnedLoot.transform);

        //Play the open animation
        safeAnimator.Play("OpenSafe");

        //Play the open sound
        openSound.Play();
    }
}
