
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GenerateLoot : UdonSharpBehaviour
{
    public Transform lootSpawnPosition; //Assigned in Unity | Every container NEEDS a spawn location inside of the GameObject for this to work
    private GameObject[] rarityTables; //Used for picking the rarity object | Assigned at Startup
    private GameObject[] commonTable;
    private GameObject[] uncommonTable;
    private GameObject[] rareTable;
    private GameObject[] extraordinaryTable;
    private GameObject[] specialTable;
    private bool isLooted = false; //Used to check if the container has been looted or not
    private Animator openContainer; //Used to play the animation of the container opening
    private GameObject spawnedLoot; //Used to store the spawned object under the _SPAWNEDLOOT GameObject
    public bool hasOpenAnimation; //Used to check if the container has an open animation or not | Assigned in Unity
    public AudioSource openSound; //Used to play the sound of the container opening | Assigned in Unity
    // Drops rates by rarity
    // (Range 0-100)
    // 0-50 (Common)
    // 51-80 (Uncommon)
    // 81-95 (Rare)
    // 96-99 (Extraordinary)
    // 100 (Special)
    
    void Start()
    {        
        //Initialize spawnedLoot by finding _SPAWNEDLOOT GameObject in Hierarchy
        spawnedLoot = GameObject.Find("_SPAWNEDLOOT");
        
        rarityTables = new GameObject[5]; //Initializes array with 5 slots for the different loot tables
        GameObject lootPrefabContainers = GameObject.Find("LootPrefabsContainer");

        for(int i = 0; i < rarityTables.Length; i++)
        {
            rarityTables[i] = lootPrefabContainers.transform.Find("Valueables").transform.GetChild(i).gameObject; //Expected: Common, Uncommon, Rare, Extraordinary, Special of type GameObject
            Debug.Log($"Found {rarityTables[i].transform.gameObject} loot table!");
        }
        
        //COMMON TABLE CODE BLOCK START
        //Initialize commonTable with common item count
        commonTable = new GameObject[rarityTables[0].transform.childCount];
        //Fill commonTable with all common item GameObjects
        int index = 0; //Used for the foreach loop
        foreach(Transform transform in rarityTables[0].transform.GetComponentInChildren<Transform>())
        {
            commonTable[index] = transform.gameObject;
            Debug.Log($"Common Item: {transform.gameObject.name} added to Common Table.");
            index++;
        }

        //UNCOMMON TABLE CODE BLOCK START
        //Initialize uncommonTable with uncommon item count
        uncommonTable = new GameObject[rarityTables[1].transform.childCount];
        //Fill uncommonTable with all uncommon item GameObjects
        index = 0; //Reset index
        foreach(Transform transform in rarityTables[1].transform.GetComponentInChildren<Transform>())
        {
            uncommonTable[index] = transform.gameObject;
            Debug.Log($"Uncommon Item: {transform.gameObject.name} added to Uncommon Table.");
            index++;
        }

        //RARE TABLE CODE BLOCK START
        //Initialize rareTable with rare item count
        rareTable = new GameObject[rarityTables[2].transform.childCount];
        //Fill rareTable with all rare item GameObjects
        index = 0; //Reset index
        foreach(Transform transform in rarityTables[2].transform.GetComponentInChildren<Transform>())
        {
            rareTable[index] = transform.gameObject;
            Debug.Log($"Rare Item: {transform.gameObject.name} added to Rare Table.");
            index++;
        }

        //EXTRAORDINARY TABLE CODE BLOCK START
        //Initialize extraordinaryTable with extraordinary item count
        extraordinaryTable = new GameObject[rarityTables[3].transform.childCount];
        //Fill extraordinaryTable with all extraordinary item GameObjects
        index = 0; //Reset index
        foreach(Transform transform in rarityTables[3].transform.GetComponentInChildren<Transform>())
        {
            extraordinaryTable[index] = transform.gameObject;
            Debug.Log($"Extraordinary Item: {transform.gameObject.name} added to Extraordinary Table.");
            index++;
        }

        //SPECIAL TABLE CODE BLOCK START
        //Initialize specialTable with special item count
        specialTable = new GameObject[rarityTables[4].transform.childCount];
        //Fill specialTable with all special item GameObjects
        index = 0; //Reset index
        foreach(Transform transform in rarityTables[4].transform.GetComponentInChildren<Transform>())
        {
            specialTable[index] = transform.gameObject;
            Debug.Log($"Special Item: {transform.gameObject.name} added to Special Table.");
            index++;
        }

        //Output that all tables have been filled with loot items
        Debug.Log("All loot tables have been filled with loot items!");
    }

    public override void Interact()
    {
        int chosenLootTable = Random.Range(0, 101); //Generates int from 0 - 100
        
        //Select table from rarityTables array then select item to spawn | Clones item and places the item in the spawn location
        Vector3 spawnPos = lootSpawnPosition.position;
        if(chosenLootTable <= 50 && !isLooted)
        {
            SpawnCommonItem(spawnPos);
            PlayOpenAnimation();
            isLooted = true;
        } else if(chosenLootTable > 50 && chosenLootTable <= 80 && !isLooted)
        {
            SpawnUncommonItem(spawnPos);
            PlayOpenAnimation();
            isLooted = true;
        } else if(chosenLootTable > 80 && chosenLootTable <= 97 && !isLooted)
        {
            SpawnRareItem(spawnPos);
            PlayOpenAnimation();
            isLooted = true;
        } else if(chosenLootTable > 97 && chosenLootTable <= 99 && !isLooted)
        {
            SpawnExtraordinaryItem(spawnPos);
            PlayOpenAnimation();
            isLooted = true;
        } else if(chosenLootTable == 100 && !isLooted)
        {
            SpawnSpecialItem(spawnPos);
            PlayOpenAnimation();
            isLooted = true;
        } else
        {
            Debug.Log("Container has already been looted by Player.");
        }

        //Disable collider so the container can't be looted again
        GetComponent<Collider>().enabled = false;
    }

    private void SpawnCommonItem(Vector3 spawnPos)
    {
        int chosenItem = Random.Range(0, commonTable.Length);
        GameObject itemToSpawn = commonTable[chosenItem];
        GameObject spawnedItem = Instantiate(itemToSpawn);
        spawnedItem.transform.position = spawnPos;
        //Set spawnedItem as a child of the _SPAWNEDLOOT parent
        spawnedItem.transform.SetParent(spawnedLoot.transform);
    }

    private void SpawnUncommonItem(Vector3 spawnPos)
    {
        int chosenItem = Random.Range(0, uncommonTable.Length);
        GameObject itemToSpawn = uncommonTable[chosenItem];
        GameObject spawnedItem = Instantiate(itemToSpawn);
        spawnedItem.transform.position = spawnPos;
        //Set spawnedItem as a child of the _SPAWNEDLOOT parent
        spawnedItem.transform.SetParent(spawnedLoot.transform);
    }

    private void SpawnRareItem(Vector3 spawnPos)
    {
        int chosenItem = Random.Range(0, rareTable.Length);
        GameObject itemToSpawn = rareTable[chosenItem];
        GameObject spawnedItem = Instantiate(itemToSpawn);
        spawnedItem.transform.position = spawnPos;
        //Set spawnedItem as a child of the _SPAWNEDLOOT parent
        spawnedItem.transform.SetParent(spawnedLoot.transform);   
    }

    private void SpawnExtraordinaryItem(Vector3 spawnPos)
    {
        int chosenItem = Random.Range(0, extraordinaryTable.Length);
        GameObject itemToSpawn = extraordinaryTable[chosenItem];
        GameObject spawnedItem = Instantiate(itemToSpawn);
        spawnedItem.transform.position = spawnPos;
        //Set spawnedItem as a child of the _SPAWNEDLOOT parent
        spawnedItem.transform.SetParent(spawnedLoot.transform);
    }

    private void SpawnSpecialItem(Vector3 spawnPos)
    {
        int chosenItem = Random.Range(0, specialTable.Length);
        GameObject itemToSpawn = specialTable[chosenItem];
        GameObject spawnedItem = Instantiate(itemToSpawn);
        spawnedItem.transform.position = spawnPos;
        //Set spawnedItem as a child of the _SPAWNEDLOOT parent
        spawnedItem.transform.SetParent(spawnedLoot.transform);
    }

    private void PlayOpenAnimation()
    {
        if(hasOpenAnimation)
        {
            openContainer = GetComponent<Animator>();
            openContainer.Play("OpenContainer");
        }
        openSound.Play();
    }

    public void ResetLootContainer()
    {
        //Reset isLooted
        isLooted = false;
        //Enable collider so the container can be looted again
        GetComponent<Collider>().enabled = true;
    }
}
