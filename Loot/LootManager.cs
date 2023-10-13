
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to centralize all loot generation logic into one script - this script is attached to the LootManager GameObject
public class LootManager : UdonSharpBehaviour
{
    [SerializeField] private GameObject lootPrefabContainers; //Assigned in Unity | Used for finding the loot tables
    private GameObject[] rarityTables; //Used for picking the rarity object | Assigned at Startup
    private GameObject[] commonTable;
    private GameObject[] uncommonTable;
    private GameObject[] rareTable;
    private GameObject[] extraordinaryTable;
    private GameObject[] specialTable;
    
    
    void Start()
    {        
        rarityTables = new GameObject[5]; //Initializes array with 5 slots for the different loot tables

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

    //Returns a random loot item from the specified rarity table | Called by GenerateLoot.cs
    public GameObject GetLootItem(int rarity)
    {
        switch(rarity)
        {
            case 0:
                return commonTable[Random.Range(0, commonTable.Length)];
            case 1:
                return uncommonTable[Random.Range(0, uncommonTable.Length)];
            case 2:
                return rareTable[Random.Range(0, rareTable.Length)];
            case 3:
                return extraordinaryTable[Random.Range(0, extraordinaryTable.Length)];
            case 4:
                return specialTable[Random.Range(0, specialTable.Length)];
            default:
                return null;
        }
    }
}
