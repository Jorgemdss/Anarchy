using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using SmartLocalization;
/// <summary>
/// Singleton Game Manager. This is the game's core class.
/// </summary>
public class GameManagerController : MonoBehaviour
{

    #region variables

    /// <summary>
    /// Game manager instance.
    /// </summary>
    public static GameManagerController Instance { get; protected set; }

    public Player Player { get; set; }

    //PlayerController playerController;

    /// <summary>
    /// The name of our current Level.
    /// </summary>
    //private static string currentLevel;


    private static GameState gameState;

    bool isPaused;
        
    #endregion

    #region Enums

    /// <summary>
    /// Game states.
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }
    
    #endregion

    void Awake()
    {
        /// Sets the name to "MainMenu" the first time we enter the game.
        UpdateLevelName(Application.loadedLevelName);
        SwitchGameState(GameState.Playing);

        /// Creates a singleton <see cref="GameManagerController"/>instance.
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Player = new Player();
    }

    void Update()
    {
        #region debug inputs

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log("current level: " + currentLevel);            
            //Debug.Log("game state: " + gameState.ToString());
            Debug.Log(PlayerPrefs.GetInt("HighScore"));
            //PlayerPrefs.DeleteAll();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //SceneManager.LoadScene("PrototypeLevel");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            Time.timeScale = (isPaused) ? 1 : 0;
        }
        #endregion
    }

    #region public methods

    /// <summary>
    /// Updates the level name.
    /// </summary>
    /// <param name="levelName">The name to update to, if empty update to current name.</param>
    public void UpdateLevelName(string levelName)
    {
        if (string.IsNullOrEmpty(levelName))
        {
            //currentLevel = Application.loadedLevelName;
        }
        else
        {                
            //currentLevel = levelName;
        }
    }

    /// <summary>
    /// Changes the <see cref="GameState"/>.
    /// </summary>
    /// <param name="state">The state to change for.</param>
    public void SwitchGameState(GameState state)
    {
        gameState = state;
    }

    /// <summary>
    /// Loads the selected level, Switches game state and updates the current level name.
    /// </summary>
    /// <param name="levelName">The level to load.</param>
    public void ChangeLevel(string levelName)
    {
        switch (levelName)
        {
            case "MainMenu":
            case "ScoreBoard":
                SwitchGameState(GameState.MainMenu);
                break;
            case "PrototypeLevel":
                SwitchGameState(GameState.Playing);
                break;
            default:                
                break;
        }       

        SceneManager.LoadScene(levelName);

        UpdateLevelName(levelName);
    }    

    public void GameOver(int currentScore)
    {
        SwitchGameState(GameState.GameOver);
        Debug.LogError("Game Over");
        
        var playerPrefsScore = PlayerPrefs.GetInt("HighScore");

        if (currentScore > playerPrefsScore)
        {
            HighScoreService.instance.UpdatePlayerHighScore(Player.PlayerId, Player.Name, currentScore);
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }
    #endregion
}
