
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to store all active weapons and their respective owners
public class WeaponDictionary : UdonSharpBehaviour
{
    //Weapon Dictionary
    private DataDictionary weaponDictionary; //Dictionary for storing all active weapons and their respective owners | Assigned in Unity inspector

    public void RegisterWeapon(DataToken playerName, DataToken playerWeapon)
    {
        weaponDictionary.Add(playerName, playerWeapon); //Stores the player's name and their equipped GameObject weapon in the dictionary
        Debug.Log($"Successfully registered {playerName} with weapon {playerWeapon}");
    }

    public bool UnregisterWeapon(DataToken playerName)
    {
        return weaponDictionary.Remove(playerName); //Returns true if the key was removed successfully else false
    }

    public void UpdateWeapon(DataToken playerName, DataToken playerWeapon)
    {
        weaponDictionary.SetValue(playerName, playerWeapon); //Updates the player's weapon in the dictionary | Creates a new entry if the player does not exist in the dictionary
    }

    public DataToken GetPlayerWeapon(DataToken playerName)
    {
        return weaponDictionary[playerName]; //Returns the player's weapon DataToken
    }
}
