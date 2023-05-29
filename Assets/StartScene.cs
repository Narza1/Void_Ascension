using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    public VisualElement selectCharacter, confirmationMessage;
    private bool entryTower;
    AvatarController player;
    void Start()
    {
        selectCharacter = GameObject.Find("SelectCharacter").GetComponent<UIDocument>().rootVisualElement;
        confirmationMessage = GameObject.Find("ConfirmationMessage").GetComponent<UIDocument>().rootVisualElement;
        confirmationMessage.style.display = DisplayStyle.None;
        gameManager = GetComponent<GameManager>();
        ReadyUI();
        player = GameObject.Find("Player").GetComponent<AvatarController>();

    }

    private void ReadyUI()
    {
        List<Button> selectCharacterButtons = selectCharacter.Query<Button>().ToList();
        selectCharacterButtons[0].RegisterCallback<ClickEvent>(LoadStats2);
        selectCharacterButtons[1].RegisterCallback<ClickEvent>(LoadStats1);
        selectCharacterButtons[2].RegisterCallback<ClickEvent>(LoadStats);
        selectCharacterButtons[3].RegisterCallback<ClickEvent>(LoadStats3);

        List<Button> messageButtons = confirmationMessage.Query<Button>().ToList();
        messageButtons[0].RegisterCallback<ClickEvent>(EnterTower);
        messageButtons[1].RegisterCallback<ClickEvent>(CloseMessageEvent);
    }

    private void CloseMessageEvent(ClickEvent evt)
    {
        CloseMessage();
    }private void CloseMessage()
    {
        confirmationMessage.style.display = DisplayStyle.None;
    }private void EnterTower(ClickEvent evt)
    {
        gameManager.playerData.currentFloor++;
        gameManager.SaveFile();
        SceneManager.LoadScene("SampleScene");
    }
    
    public void ShowMessage()
    {
        confirmationMessage.style.display = DisplayStyle.Flex;
    }

    private void LoadStats(ClickEvent evt) { ChangeCharacter(0); }
    private void LoadStats1(ClickEvent evt) { ChangeCharacter(1); }
    private void LoadStats2(ClickEvent evt) { ChangeCharacter(2); }
    private void LoadStats3(ClickEvent evt) { ChangeCharacter(3); }
    public void ShowChangeCaracter()
    {
        selectCharacter.style.display = DisplayStyle.Flex;

    }
    private void ChangeCharacter(int value)
    {
        Debug.Log(value);

        gameManager.playerData.currentCharacter = value;
        player.playerData.currentCharacter = value;
        player.LoadCharacters();
        selectCharacter.style.display = DisplayStyle.None;


    }
    //    btnStats.style.backgroundColor = new StyleColor(new Color32(59, 59, 59, 255));
    //    groupStats.style.display = DisplayStyle.Flex;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!entryTower)
        {
            entryTower = true;
            Debug.Log("entro");
            ShowMessage();
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CloseMessage();
        entryTower = false;
    }
}
