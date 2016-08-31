using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonInputManager : MonoBehaviour
{


    public static ButtonInputManager instance;
    public UserSessionController userSessionController;
    public Text editorForName;
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

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ValidateName()
    {
        if (editorForName != null)
        {
            if (!string.IsNullOrEmpty(editorForName.text))
            {
                userSessionController.SubmitName(editorForName.text);
            }
        }
    }
}
