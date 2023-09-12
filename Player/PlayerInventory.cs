
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerInventory : UdonSharpBehaviour
{   
    public int[] PlayerOwnedPistols {get; set;}
    private readonly int pistolCount = 7; //Counts base pistols without attachments in game
    //Current Pistols in order for array based on index
    // M1911 | Glock | Beretta | USP | Deagle | Revolver | TEC9

    public int[] PlayerOwnedSMGs{get; set;}
    private readonly int smgCount = 5; //Counts base SMGs without attachments in game
    //Current SMGs in order for array based on index
    // UZI | MP5 | MP9 | P90 | Vector

    public int[] PlayerOwnedARs{get; set;}
    private readonly int arCount = 6; //Counts base ARs without attachments in game
    //Current ARs in order for array
    // M4 | AK | G36 | LVOA | AUG | M16

    public int[] PlayerOwnedShotguns{get; set;}
    private readonly int shotgunCount = 5;
    //Current Shotguns in order for array
    // M590A1 | Double Barrel | M1014 | SPAS 12 | AA12

    public int[] PlayerOwnedSnipers{get; set;}
    private readonly int sniperCount = 6;
    //Current Snipers in order for array
    // HuntingRifle | Winchester | SVD | AWP | RSASS | 50Cal

    void Start()
    {
        //Default Pistol Stacks
        PlayerOwnedPistols = new int[pistolCount]; //Array is by default initialized with 0's
        PlayerOwnedPistols[0] = 2; //Grants 2 M1911 pistols by default
        PlayerOwnedPistols[1] = 1; //Grants 1 Glock by default

        //Default SMG Stacks
        PlayerOwnedSMGs = new int[smgCount];
        PlayerOwnedSMGs[0] = 1; //Grants 1 UZI by default

        //Default AR Stacks
        PlayerOwnedARs = new int[arCount];
        PlayerOwnedARs[0] = 2; //Grants 2 M4s by default
        PlayerOwnedARs[1] = 2; //Grants 2 AKs by default

        //Default Shotgun Stacks
        PlayerOwnedShotguns = new int[shotgunCount];
        PlayerOwnedShotguns[0] = 1; //Grants 1 M590A1 by default

        //Default Sniper Stacks
        PlayerOwnedSnipers = new int[sniperCount];
        PlayerOwnedSnipers[0] = 1; //Grants 1 HuntingRifle by default

        //Default LMG Stacks

        //Default Special Weapons

        //Default Melee Weapons - Knife will always be available as a free weapon
    }

}
