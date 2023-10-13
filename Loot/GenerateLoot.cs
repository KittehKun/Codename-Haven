
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GenerateLoot : UdonSharpBehaviour
{
    public LootManager lootManager; //Used to access the LootManager script and to generate loot
    [SerializeField] private Transform lootSpawnPosition; //Assigned in Unity | Every container NEEDS a spawn location inside of the GameObject for this to work
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
        spawnedLoot = GameObject.Find("_SPAWNEDLOOT");
    }

    public override void Interact()
    {
        int chosenLootTable = Random.Range(0, 101); //Generates int from 0 - 100

        //Select table from rarityTables array then select item to spawn | Clones item and places the item in the spawn location
        if (chosenLootTable <= 50 && !isLooted)
        {
            SpawnLootItem(Rarity.Common);
        }
        else if (chosenLootTable > 50 && chosenLootTable <= 80 && !isLooted)
        {
            SpawnLootItem(Rarity.Uncommon);
        }
        else if (chosenLootTable > 80 && chosenLootTable <= 97 && !isLooted)
        {
            SpawnLootItem(Rarity.Rare);
        }
        else if (chosenLootTable > 97 && chosenLootTable <= 99 && !isLooted)
        {
            SpawnLootItem(Rarity.Extraordinary);
        }
        else if (chosenLootTable == 100 && !isLooted)
        {
            SpawnLootItem(Rarity.Special);
        }
        else
        {
            Debug.Log("Container has already been looted by Player.");
        }

        //Disable collider so the container can't be looted again
        GetComponent<Collider>().enabled = false;
    }

    private void SpawnLootItem(Rarity rarity)
    {
        GameObject lootItem = Instantiate(lootManager.GetLootItem((int)rarity));
        lootItem.transform.position = lootSpawnPosition.position;
        lootItem.transform.parent = spawnedLoot.transform;

        PlayOpenAnimation();
        isLooted = true;
    }

    private void PlayOpenAnimation()
    {
        if (hasOpenAnimation)
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

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Extraordinary,
    Special
}
