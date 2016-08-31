using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameInterfaceController : MonoBehaviour
{

    public Text distanceText;
    public Text levelText;
    public Text facingText;
    public Text fullRotationCounterText;


    GameManagerController gameManager;
    LevelManagerController levelManager;
    JumpController jumpController;

    void Start()
    {
        gameManager = GameManagerController.Instance;
        levelManager = GameObject.FindObjectOfType<LevelManagerController>();
        jumpController = GameObject.FindObjectOfType<JumpController>();
    }


    void Update()
    {
        distanceText.text = levelManager.CurrentScore.ToString();
        levelText.text = levelManager.CurrentLevel.ToString();
        fullRotationCounterText.text = jumpController.FullRotationCounter.ToString();

        //switch (jumpController.PlayerFacingDirection)
        //{
        //    case JumpController.FacingDirection.FacingUp:
        //        facingText.text = "up";
        //        break;
        //    case JumpController.FacingDirection.FacingRight:
        //        facingText.text = "right";
        //        break;
        //    case JumpController.FacingDirection.FacingDown:
        //        facingText.text = "down";
        //        break;
        //    case JumpController.FacingDirection.FacingLeft:
        //        facingText.text = "left";
        //        break;
        //    default:
        //        facingText.text = "error";
        //        break;
        //}
    }    

    public static void UpdatePlayerRotationUI(Text t, string c)
    {
        t.text = c;        
    }
}

