
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to handle the leaderboard system displaying the top earners of the server
public class Leaderboard : UdonSharpBehaviour
{
    [UdonSynced] private int[] leaderBoard = {0, 0, 0}; //Array of the top 3 earners | Assigned in Start()
    public Text[] leaderboardNames = new Text[3]; //Array of the top 3 earners' names | Assigned in Unity inspector
    public Text[] moneyTexts = new Text[3]; //Array of the top 3 earners' money | Assigned in Unity inspector

    //Method called on PlayerRaidManager.cs when a player successfully extracts from the world
    public void UpdateLeaderboard(int playerMoney)
    {
        //Loop through array finding the lowest value
        for (int i = 0; i < leaderBoard.Length; i++)
        {
            //Check if the player's money is greater than the current value in the array
            if(playerMoney > leaderBoard[i])
            {
                //Set owner of Scoreboard to the localplayer
                Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
                //Set the player's money to the current value in the array
                leaderBoard[i] = playerMoney;
                //Update the leaderboard's text fields taking in the index
                UpdateLeaderboardText(i, playerMoney);
                //Break out of the loop
                break;
            }
        }
    }

    private void UpdateLeaderboardText(int placeIndex, int newPlayerMoney)
    {
        //Update the leaderboard's text fields taking in the index
        leaderboardNames[placeIndex].text = Networking.LocalPlayer.displayName;
        moneyTexts[placeIndex].text = $"$ {newPlayerMoney}";
    }
}
