using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Label = UnityEngine.UIElements.Label;

public class Stairs : MonoBehaviour
{
    private VisualElement m_Root;
    List<Button> messageButtons;
    Label label;
    // Start is called before the first frame update
    void Start()
    {
        m_Root = GameObject.Find("ConfirmationWindow").GetComponent<UIDocument>().rootVisualElement.Q("Message");

        messageButtons = m_Root.Query<Button>().ToList();
        label = m_Root.Q("Text") as Label;

        messageButtons[1].RegisterCallback<ClickEvent>(CloseMessageEvent);
    }

    private void CloseMessageEvent(ClickEvent evt)
    {
        CloseMessage();
    }
    private void CloseMessage()
    {
        m_Root.style.display = DisplayStyle.None;
    }
    private void EnterTower(ClickEvent evt)
    {
        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.playerData.currentFloor++;
        gameManager.SaveFile();
        int[] a = new int[] { 10, 20, 30 };
        Debug.Log("piso num"+gameManager.playerData.currentFloor);
        if (a.Contains(gameManager.playerData.currentFloor))
            SceneManager.LoadScene("SampleScene1");
        if(gameManager.playerData.currentFloor == 31)
            SceneManager.LoadScene("IntroScene");

        SceneManager.LoadScene("SampleScene");
    }
    private void GoBack(ClickEvent evt)
    {
        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.playerData.currentFloor=0;
        gameManager.SaveFile();
        SceneManager.LoadScene("NewGameScene");
    }

    public void ShowMessage()
    {
        m_Root.style.display = DisplayStyle.Flex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            if (gameObject.name.Equals("DownStairs"))
            {
                label.text = "Go Back?";
                messageButtons[0].RegisterCallback<ClickEvent>(GoBack);
            }
            else
            {
                label.text = "Go to next floor?";
                messageButtons[0].RegisterCallback<ClickEvent>(EnterTower);
            }
            m_Root.style.display = DisplayStyle.Flex;

        }
    }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name.Equals("Player"))
                CloseMessage();

        }
}
