using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Player player;
    LevelManagerController levelManager;


    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManagerController>();

        if (levelManager != null &&
            levelManager.player != null
            )
        {
            player = GameManagerController.Instance.Player;
            //player = levelManager.player;
        }
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {       
        
        bool collidedWithObstacle = (col.GetComponent<IObstacle>() != null) ? true : false;

        if (collidedWithObstacle)
        {            
            player.IsAlive = false;
            Destroy(gameObject);            
            GameManagerController.Instance.GameOver(int.Parse(player.Score));
        }

    }
}
