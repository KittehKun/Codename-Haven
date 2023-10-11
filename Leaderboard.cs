
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
                if(leaderboardNames[i].text == Networking.LocalPlayer.displayName)
                {
                    //Check if the player has beaten their own score
                    if(playerMoney > leaderBoard[i])
                    {
                        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
                        leaderBoard[i] = playerMoney;
                        UpdateLeaderboardText(i, playerMoney);
                        RequestSerialization();
                        return; //Early return to prevent the player from being added to leaderboard multiple times
                    }
                }
                //Set owner of Scoreboard to the localplayer
                Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
                //Set the player's money to the current value in the array
                leaderBoard[i] = playerMoney;
                //Update the leaderboard's text fields taking in the index
                UpdateLeaderboardText(i, playerMoney);
                //Request Serialization
                RequestSerialization();
                return;
            }
        }
    }

    private void UpdateLeaderboardText(int placeIndex, int newPlayerMoney)
    {
        //Update the leaderboard's text fields taking in the index
        leaderboardNames[placeIndex].text = Networking.GetOwner(this.gameObject).displayName;
        moneyTexts[placeIndex].text = $"$ {newPlayerMoney}";
    }

    //Call OnDeserialization() when the leaderboard is updated on the network
    public override void OnDeserialization()
    {
        //Update the leaderboard text for all remote players
        UpdateLeaderboardText(0, leaderBoard[0]);
        UpdateLeaderboardText(1, leaderBoard[1]);
        UpdateLeaderboardText(2, leaderBoard[2]);
    }
}
