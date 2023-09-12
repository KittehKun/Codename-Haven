
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DebugGenerateLoot : UdonSharpBehaviour
{
    public GameObject SpawnObject; //Assigned in Unity
    public Transform LootSpawnLocation; //Assigned in Unity
    public Animator containerAnimator; //Assigned in Unity
    private string animationState = "Office Cabinet Test"; //This is the state name that will be played from the Animator
    private bool isLooted;

    void Start()
    {
        SpawnObject = GameObject.Find("LootPrefabsContainer").transform.GetChild(0).transform.GetChild(0).gameObject; //Expected: SmallCashPile
    }

    public override void Interact()
    {
        if(!isLooted)
        {
            GameObject loot = Instantiate(SpawnObject);
            loot.SetActive(true);
            loot.transform.parent = this.transform;
            loot.transform.position = LootSpawnLocation.position;
            containerAnimator.Play(animationState);
            isLooted = true;
        }
        Debug.Log("Container has already been looted~!");
    }
}
