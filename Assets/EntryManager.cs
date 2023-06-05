using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EntryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject confirmationMessage;
    private VisualElement window;

    void Start()
    {
        window = confirmationMessage.GetComponent<UIDocument>().rootVisualElement;
        if (!GameManager.SaveFileExists())
        {
            GameObject.Find("btnContinue").SetActive(false);
        }
    }

    public void NewGame()
    {
        if (GameManager.SaveFileExists())
        {
            window.style.display = DisplayStyle.Flex;

            List<Button> messageButtons = window.Query<Button>().ToList();
            messageButtons[0].RegisterCallback<ClickEvent>(LoadNewGame);
            messageButtons[1].RegisterCallback<ClickEvent>(CloseMessage);
        }
    }
    private void LoadNewGame(ClickEvent evt)
    {
        GameManager.DeleteSaveFile();
        SceneManager.LoadScene("NewGameIntro");
    }
    private void CloseMessage(ClickEvent evt)
    {
        window.style.display = DisplayStyle.None;
    }
    public void LoadGame()
    {
        switch (GameManager.GetSavedFloor())
        {
            case 0:
                SceneManager.LoadScene("NewGameScene");
                break;

            case 10:
            case 20:
            case 30:
                SceneManager.LoadScene("SampleScene1");
                break;

            default:
                SceneManager.LoadScene("SampleScene");
                break;
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}

