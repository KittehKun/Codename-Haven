
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to centralize all loot generation logic into one script - this script is attached to the LootManager GameObject
public class LootManager : UdonSharpBehaviour
{
    private GameObject[] commonTable;
    private GameObject[] uncommonTable;
    private GameObject[] rareTable;
    private GameObject[] extraordinaryTable;
    private GameObject[] specialTable;

    [Header("Loot Tables")]
    [SerializeField] private Transform commonLootContainer; //Assigned in Unity inspector | Used for storing all common loot items
    [SerializeField] private Transform uncommonLootContainer; //Assigned in Unity inspector | Used for storing all uncommon loot items
    [SerializeField] private Transform rareLootContainer; //Assigned in Unity inspector | Used for storing all rare loot items
    [SerializeField] private Transform extraordinaryLootContainer; //Assigned in Unity inspector | Used for storing all extraordinary loot items
    [SerializeField] private Transform specialLootContainer; //Assigned in Unity inspector | Used for storing all special loot items
    
    void Start()
    {        
        commonTable = new GameObject[commonLootContainer.childCount]; //Initializes array with the number of common loot items
        foreach(Transform child in commonLootContainer) //Populates the commonTable array with all common loot items
        {
            commonTable[child.GetSiblingIndex()] = child.gameObject;
        }

        uncommonTable = new GameObject[uncommonLootContainer.childCount]; //Initializes array with the number of uncommon loot items
        foreach(Transform child in uncommonLootContainer) //Populates the uncommonTable array with all uncommon loot items
        {
            uncommonTable[child.GetSiblingIndex()] = child.gameObject;
        }

        rareTable = new GameObject[rareLootContainer.childCount]; //Initializes array with the number of rare loot items
        foreach(Transform child in rareLootContainer) //Populates the rareTable array with all rare loot items
        {
            rareTable[child.GetSiblingIndex()] = child.gameObject;
        }

        extraordinaryTable = new GameObject[extraordinaryLootContainer.childCount]; //Initializes array with the number of extraordinary loot items
        foreach(Transform child in extraordinaryLootContainer) //Populates the extraordinaryTable array with all extraordinary loot items
        {
            extraordinaryTable[child.GetSiblingIndex()] = child.gameObject;
        }

        specialTable = new GameObject[specialLootContainer.childCount]; //Initializes array with the number of special loot items
        foreach(Transform child in specialLootContainer) //Populates the specialTable array with all special loot items
        {
            specialTable[child.GetSiblingIndex()] = child.gameObject;
        }
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
