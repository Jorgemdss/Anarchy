using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Utility;

public class UserSessionController : MonoBehaviour
{
    #region Other vars
    private const byte UserID = 0;
    private const byte UserName = 1;
    private const byte UserScore = 2;

//    public static UserSessionController instance;
    public UserSessionInterfaceController userSessionInterfaceController;

    /// <summary>
    /// Name editor field.
    /// </summary>
    //public GameObject editorForName;

    //public GameObject nameUnicityError;
    //public GameObject nameSuccessValidation;

    //public GameObject newUserArea;    

    //public Text playerNameWelcomeValue;

    private string secretKey = "123456789"; // Edit this value and make sure it's the same as the one stored on the server    
    //string addScoreURL = "clawindiegames.bplaced.net/addscore.php?"; //be sure to add a ? to your url
    //string highscoreURL = "clawindiegames.bplaced.net/display.php";
    string addScoreURL = "clawindiegames.square7.ch/addscore.php?"; //be sure to add a ? to your url
    string highscoreURL = "clawindiegames.square7.ch/display.php";    
    
    public List<Player> AllPlayersList = new List<Player>();
    #endregion

    #region Properties
    public Player Player { get; set; }
    #endregion    

    #region Awake Start Update

    void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else if (instance != this)
        //{
        //    Destroy(gameObject);
        //}

        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Player = GameManagerController.Instance.Player;
        LoginOrCreateNewUser();
        StartCoroutine(GetScoresAndFillPlayerList());        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            //Debug.Log(Player.Name);                            
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            //StartCoroutine(PostScores(0,"jojo",50));
            //PlayerPrefs.DeleteAll();            
        }        
    }

    #endregion



    #region Public Mehtods

    /// <summary>
    /// Sets the name in PlayerPrefs and posts a new entry on the scores table with the chosen name and 0 score.
    /// </summary>
    /// <param name="name">The player name</param>
    public void SubmitName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            if (IsValidName(name))
            {
                // Set the player prefs and prop PlayerName
                SetNewPlayerName(name);

                // Display success message and hide error
                //nameSuccessValidation.SetActive(true);                                
                Utilities.ActivateOrDeactivateGameObject(true, userSessionInterfaceController.nameSuccessValidation);                
                //nameUnicityError.SetActive(false);
                Utilities.ActivateOrDeactivateGameObject(false, userSessionInterfaceController.nameUnicityError);

                // Interface
                userSessionInterfaceController.UpdateElementText(Player.Name,userSessionInterfaceController.playerNameWelcomeValue);
                //playerNameWelcomeValue.text = Player.Name;

                //editorForName.GetComponent<InputField>().interactable = false;
                userSessionInterfaceController.SetElementInteractable(false, userSessionInterfaceController.editorForName);
                
                AllPlayersList.Add(new Player { PlayerId = Player.PlayerId, Name = Player.Name, Score = "0" });
                GameManagerController.Instance.Player = Player;
                // Post the new entry.
                StartCoroutine(PostScores("0", name, 0));
            }
            else
            {
                //nameUnicityError.SetActive(true);
                Utilities.ActivateOrDeactivateGameObject(true, userSessionInterfaceController.nameUnicityError);
            }

        }
    }

    /// <summary>
    /// Updates the highscore if the user has a name.
    /// </summary>
    /// <param name="score">the score</param>
    public void UpdatePlayerHighScore(int score)
    {
        if (!string.IsNullOrEmpty(Player.Name))
        {
            StartCoroutine(PostScores(Player.PlayerId, Player.Name, score));
        }
    }

    #endregion

    #region Private methods

    private void LoginOrCreateNewUser()
    {
        // If there is an existing user, display welcome message
        // Else display the user creation area.

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            Player.Name = PlayerPrefs.GetString("PlayerName");
            userSessionInterfaceController.UpdateElementText(Player.Name, userSessionInterfaceController.playerNameWelcomeValue);
            //playerNameWelcomeValue.text = Player.Name;
            userSessionInterfaceController.SetElementInteractable(false, userSessionInterfaceController.editorForName);
            //    editorForName.GetComponent<InputField>().text = Player.Name;
            userSessionInterfaceController.UpdateElementText(Player.Name,userSessionInterfaceController.editorForName,true);            

        }
        else
        {
            // Interface
            userSessionInterfaceController.UpdateElementText("New user, please chose a name.", userSessionInterfaceController.playerNameWelcomeValue);
            //playerNameWelcomeValue.text = "New user, please chose a name.";
            userSessionInterfaceController.UpdateElementText(string.Empty, userSessionInterfaceController.editorForName, true);
            Utilities.ActivateOrDeactivateGameObject(true,userSessionInterfaceController.newUserArea);
            //newUserArea.SetActive(true);            
        }
    }


    /// <summary>
    /// Sets the new player name in playerprefs and property PlayerName.
    /// </summary>
    /// <param name="playerName">The chosen player name.</param>
    private void SetNewPlayerName(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        Player.Name = playerName;
    }

    private bool IsValidName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            if (AllPlayersList.Any(p => p.Name == name))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator GetScoresAndFillPlayerList()
    {
        WWW hs_get = new WWW("http://" + highscoreURL);
        yield return hs_get;
        
        if (hs_get.error != null)
        {
            Debug.LogError("There was an error getting the high score: " + hs_get.error);
        }
        else
        {

            // The WWW text
            var usernameScoreChainString = hs_get.text;

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
                        AllPlayersList.Add(
                            new Player
                            {
                                PlayerId = playerIdNameScoreplits[i],
                                Name = playerIdNameScoreplits[i + 1],
                                Score = playerIdNameScoreplits[i + 2]
                            });

                        var isUserCurrentPlayer = playerIdNameScoreplits[i + 1] == Player.Name;
                        // Get the Id of our player.
                        if (isUserCurrentPlayer)
                        {
                            Player.PlayerId = playerIdNameScoreplits[i];
                        }
                    }
                }
            }
        }

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            // if playerList contains the current name, display button, else delete playerprefs            
            if (AllPlayersList.Any(p => p.Name == Player.Name))
            {
                // Interface
                //scene2.GetComponent<Button>().interactable = true;
                //LoginOrCreateNewUser(userExists: true);
            }
            else
            {
                PlayerPrefs.DeleteKey("PlayerName");
                PlayerPrefs.DeleteKey("HighScore");
                Player.Name = string.Empty;
                LoginOrCreateNewUser();
            }
        }        
    }

    private IEnumerator PostScores(string uniqueID, string name, int score)
    {
        //updateOnlineHighscoreData ();

        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = Md5Sum(name + score + secretKey);
        //string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
        string post_url = addScoreURL + "uniqueID=" + uniqueID + "&name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
        //Debug.Log ("post url " + post_url);
        // Post the URL to the site and create a download object to get the result.

        WWW hs_post = new WWW("http://" + post_url);
        yield return hs_post; // Wait until the download is done

        if (hs_post.error != null)
        {
            Debug.LogError("There was an error posting the high score: " + hs_post.error);
        }
    }

    private string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    #endregion
}
