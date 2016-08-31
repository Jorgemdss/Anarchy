using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Utility;


public class UserSessionInterfaceController : MonoBehaviour
{
    #region User Login Interface vars
    /// <summary>
    /// Name editor field.
    /// </summary>
    public Selectable editorForName;

    public GameObject nameUnicityError;

    public GameObject nameSuccessValidation;

    public GameObject newUserArea;

    public Text playerNameWelcomeValue;

    public UserSessionController userSessionController;

    Player player;
    #endregion

    #region public methods
    void Start()
    {
        if (userSessionController.userSessionInterfaceController == null)
        {
            userSessionController.userSessionInterfaceController = this;
        }

        player = userSessionController.Player;

        if (player != null && !string.IsNullOrEmpty(player.Name))
        {
            UpdateElementText(player.Name, playerNameWelcomeValue);
        }
        else
        {
            UpdateElementText("New player", playerNameWelcomeValue);
            Utilities.ActivateOrDeactivateGameObject(true, newUserArea);
        }

    }
    public void SetElementInteractable(bool isInteractable, Selectable element)
    {
        if (isInteractable)
        {
            element.interactable = true;
        }
        else
        {
            element.interactable = false;
        }
    }

    public void UpdateElementText<T>(string text, T element, bool isInputField = false)
    {
        var elementType = typeof(T);

        if (elementType == typeof(Text))
        {
            var textElement = element.ChangeType<Text>();
            textElement.text = text;            
        }
        else if (elementType == typeof(Selectable))
        {
            if (isInputField)
            {
                var inputElement = element.ChangeType<InputField>();
                inputElement.GetComponent<InputField>().text = text;
            }
            else
            {
                var selectableElement = element.ChangeType<Selectable>();
                selectableElement.GetComponent<Text>().text = text;                
            }
        }

    }

    #endregion

}
