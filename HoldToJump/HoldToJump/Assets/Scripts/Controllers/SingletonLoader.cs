using UnityEngine;
using System.Collections;

/// <summary>
/// If GameManagerController is null, make an instance of it.
/// </summary>
public class SingletonLoader : MonoBehaviour
{

    GameObject gameManagerController;

    void Awake()
    {
        gameManagerController = Resources.Load<GameObject>("GameManager");       

        if (GameManagerController.Instance == null)        
           Instantiate(gameManagerController);
    }

}
