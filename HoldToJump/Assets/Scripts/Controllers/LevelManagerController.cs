using UnityEngine;
using System.Collections;
using SmartLocalization;
using Utility;

public class LevelManagerController : MonoBehaviour
{
    #region Vars

    //PlatformPooler platformPooler;
    LevelItemController levelItemController;

    public Player player;

    byte minPlatFormGap;
    byte maxPlatFormGap;

    float scoreCounter;
    float platformSpawnCounter;
    int xpCounter;
    int xpMultiplier;
    #endregion

    #region Properties

    public int PlatformSpawnInterval { get; set; }
    public float ScoreIncrementInterval { get; set; }
    public int CurrentScore { get; set; }

    public int CurrentLevel { get; set; }

    void Start()
    #endregion
    {
        InstantiatePlayer();
        CurrentLevel = 1;
        minPlatFormGap = 1;
        maxPlatFormGap = 4;
        PlatformSpawnInterval = 3;
        ScoreIncrementInterval = 1.25f;
        xpCounter = 10;
        xpMultiplier = 10;        
    }   

    void Update()
    {
        /// Set Time delta time Counters
        scoreCounter += Time.deltaTime;
        platformSpawnCounter += Time.deltaTime;
        /// End counters        

        if (Utilities.IsCounterGreaterThanInterval(scoreCounter, ScoreIncrementInterval))
        {
            AddScore(1);
            if (CurrentScore == xpCounter)
            {
                LevelUp();
                //if (CurrentLevel == 5)
                //{
                //    xpMultiplier += 5;
                //}
                xpCounter = CurrentScore + xpMultiplier;
            }
        }
    }

    /// <summary>
    /// Instaniates a new <see cref="PlayerController"/> and a <see cref="Player"/>
    /// </summary>
    public void InstantiatePlayer()
    {
        if (GameObject.FindObjectOfType<PlayerController>() == null)
        {
            var path = "Player/" + LanguageManager.Instance.GetTextValue("PlayerName");
            Vector3 spawnPoint = GameObject.Find("PlayerSpawnPoint").transform.position;

            PlayerController playerController =
                Instantiate(Resources.Load<PlayerController>(path));

            playerController.transform.position = spawnPoint;

            player = GameManagerController.Instance.Player;
            player.UpdatePlayerProperties(playerController);
            //player = new Player(playerController);
        }
    }

    /// <summary>
    /// Add score if player is alive.
    /// </summary>
    /// <param name="ammount"></param>
    public void AddScore(int ammount)
    {
        if (player.IsAlive)
        {
            scoreCounter = 0;
            CurrentScore += ammount;
            //player.Score = CurrentScore.ToString();
            GameManagerController.Instance.Player.Score = CurrentScore.ToString();
        }
    }
    
    public void LevelUp()
    {
        //TODO: spawn diferent platforms (start with floatig)
        CurrentLevel++;
    }    
}
