using UnityEngine;
using System.Collections;
using SmartLocalization;

public class Player
{
    
    #region Cosntructors

    public Player()
    {
    }
    public Player(PlayerController playerController)
    {
        GameObject = playerController.gameObject;
        Rigidbody2D = GameObject.GetComponent<Rigidbody2D>();
        IsAlive = true;        
    }
    #endregion

    #region Properties

    public GameObject GameObject { get; set; }

    public Rigidbody2D Rigidbody2D { get; set; }

    public bool IsAlive { get; set; }

    public string Name { get; set; }
    public string PlayerId { get; set; }
    public string Score { get; set; }

    public void UpdatePlayerProperties(PlayerController playerController)
    {
        GameObject = playerController.gameObject;
        Rigidbody2D = GameObject.GetComponent<Rigidbody2D>();
        IsAlive = true;
    }
    //public string PlayerName
    //{
    //    get
    //    {
    //        return LanguageManager.Instance.GetTextValue("PlayerName");
    //    }
    //    private set
    //    {
    //        PlayerName = LanguageManager.Instance.GetTextValue("PlayerName");
    //    }
    //}
    #endregion
}
