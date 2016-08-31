using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
public class Scoreboard : MonoBehaviour
{
    public Text scoreBoard;
    public GameObject scoreContainer;
    public GameObject scoreEntry;
    public List<Player> allPlayersList;

    void Start ()
    {
        allPlayersList = new List<Player>();
        // Update the list of highscores
        //StartCoroutine(HighScoreService.instance.FillListWithHighScores());

        StartCoroutine(
            HighScoreService.instance.FillListWithHighScores
            (
                scoresWWW: (scoresWWW) => 
                {
                    FillPlayerList(scoresWWW);
                    PostScoresToScreen();
                }
            )
        );        
    }

    private void FillPlayerList(WWW scoresWWW)
    {
        //The WWW text
           var usernameScoreChainString = scoresWWW.text;

        var playerIdNameScoreplits = usernameScoreChainString.Split(";"[0]);

        if (playerIdNameScoreplits.Length != default(int))
        {
            // square7 web host (dont know why)
            var lenght = playerIdNameScoreplits.Length - 1;

            //other 
            //var lenght = playerIdNameScoreplits.Length;
            for (int i = 0; i < lenght; i += 3)
            {
                if (!string.IsNullOrEmpty(playerIdNameScoreplits[i]))
                {

                    ///  | ID = i | NAME = i+1 | SCORE = i+2 |
                    allPlayersList.Add(
                        new Player
                        {
                            PlayerId = playerIdNameScoreplits[i],
                            Name = playerIdNameScoreplits[i + 1],
                            Score = playerIdNameScoreplits[i + 2]
                        });

                    var isUserCurrentPlayer = playerIdNameScoreplits[i + 1] == GameManagerController.Instance.Player.Name;
                    // Get the Id of our player.
                    if (isUserCurrentPlayer)
                    {
                        GameManagerController.Instance.Player.PlayerId = playerIdNameScoreplits[i];
                    }
                }
            }
        }
    }

    private void PostScoresToScreen()
    {
        for (int i = 0; i < allPlayersList.Count; i++)
        {
            var playerRow = Instantiate(scoreEntry) as GameObject;
            playerRow.transform.SetParent(scoreContainer.transform);
            playerRow.transform.localScale = scoreContainer.transform.localScale;
            playerRow.GetComponent<Text>().text = string.Format("{0}. {1}.....{2}", i + 1, allPlayersList[i].Name, allPlayersList[i].Score);

            // If the current player is "me" paint it green
            if (allPlayersList[i].Name == GameManagerController.Instance.Player.Name)
            {
                var colorGreen = new Color(0.34f, 0.54f, 0.07f, 1f);
                playerRow.GetComponent<Text>().color = colorGreen;
            }
        }        
    }
}
