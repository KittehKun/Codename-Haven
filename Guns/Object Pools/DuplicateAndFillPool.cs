
using UdonSharp;
using UnityEngine;

//The purpose of this script is to create 120 copies of the first child of the object this script is attached to and place them in a pool | Should only be used for guns and once
public class DuplicateAndFillPool : UdonSharpBehaviour
{
    private GameObject[] pool; //Variable used to store the pool
    void Start()
    {
        pool = new GameObject[120]; //Create a pool of 120 objects
        pool[0] = this.transform.GetChild(0).gameObject; //Set the first index of the pool to the first child of the object this script is attached to
        pool[0].SetActive(false); //Disable the original object
        string childName = pool[0].name; //Get the name of the first child of the object this script is attached to
        for (int i = 1; i < pool.Length; i++) //Loop through the pool | Skips first index as it is the first child of the object this script is attached to
        {
            pool[i] = Instantiate(this.transform.GetChild(0).gameObject); //Create a copy of the first child of the object this script is attached to
            pool[i].SetActive(false); //Disable the object
            pool[i].transform.parent = this.transform; //Set the parent of the object to the object this script is attached to
            //Rename the object based on its index
            pool[i].name = $"{childName} ({i})";

            //Set the position of the object to the position of the first child of the object this script is attached to
            pool[i].transform.position = pool[0].transform.position;
        }

        Debug.Log($"Pool created with {pool.Length} objects.");

        //Get the VRC Object Pool component from the object this script is attached to and set the pool to the pool created in this script
        this.GetComponent<VRC.SDK3.Components.VRCObjectPool>().Pool = pool;
    }

}
