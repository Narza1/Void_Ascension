using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    public VisualElement selectCharacter, confirmationMessage;
    AvatarController player;
    private AudioSource ost;

    private void Awake()
    {
        AvatarController.selectCharacter = true;
        ost = GameObject.Find("OST").GetComponent<AudioSource>();
        ost.Stop();
    }
    void Start()
    {
        StartCoroutine(Window());
       
    }

    private IEnumerator Window()
    {
       
        yield return new WaitForEndOfFrame();
        selectCharacter = GameObject.Find("SelectCharacter").GetComponent<UIDocument>().rootVisualElement;
        confirmationMessage = GameObject.Find("ConfirmationMessage").GetComponent<UIDocument>().rootVisualElement.Q("Message");
        confirmationMessage.style.display = DisplayStyle.None;
        gameManager = GetComponent<GameManager>();
        ReadyUI();
        //Debug.Log(gameManager.playerData.characters[0].isDead);
        //Debug.Log(gameManager.playerData.characters[1].isDead);
        //Debug.Log(gameManager.playerData.characters[2].isDead);
        //Debug.Log(gameManager.playerData.characters[3].isDead);
        player = GameObject.Find("Player").GetComponent<AvatarController>();
        if (!gameManager.playerData.characters[gameManager.playerData.currentCharacter].isDead && GameManager.SaveFileExists())
        { 
            if (!player.characters[player.currentCharacter].isDead)
            {
                ost.Play();
                selectCharacter.style.display = DisplayStyle.None;
                AvatarController.selectCharacter = false;
            }
        }
    }


    private void ReadyUI()
    {
        List<Button> selectCharacterButtons = selectCharacter.Query<Button>().ToList();
        gameManager = GetComponent<GameManager>();
        int deadCount = 0;
        foreach (var character in gameManager.playerData.characters)
        {
            if (character.isDead)
            {
                deadCount++;
                if (character.GetType() == typeof(WarriorCharacter))
                {
                    ChangeDisplay("Warrior");
                    selectCharacterButtons[2].style.backgroundColor = Color.black;
                    selectCharacterButtons[2].RegisterCallback<ClickEvent>(NoEvent);
                }
                if (character.GetType() == typeof(MinionCharacter))
                {
                    ChangeDisplay("Minion");
                    selectCharacterButtons[0].style.backgroundColor = Color.black;
                    selectCharacterButtons[0].RegisterCallback<ClickEvent>(NoEvent);

                }
                if (character.GetType() == typeof(MageCharacter))
                {
                    selectCharacterButtons[1].style.backgroundColor = Color.black;

                    ChangeDisplay("Mage");
                    selectCharacterButtons[1].RegisterCallback<ClickEvent>(NoEvent);
                }
                if (character.GetType() == typeof(ArcherCharacter))
                {
                    selectCharacterButtons[3].style.backgroundColor = Color.black;

                    ChangeDisplay("Archer");
                    selectCharacterButtons[3].RegisterCallback<ClickEvent>(NoEvent);

                }
            }
            else
            {
                if (character.GetType() == typeof(MinionCharacter))
                    selectCharacterButtons[0].RegisterCallback<ClickEvent>(LoadStats);

                if (character.GetType() == typeof(MageCharacter))
                    selectCharacterButtons[1].RegisterCallback<ClickEvent>(LoadStats1);

                if (character.GetType() == typeof(WarriorCharacter))
                    selectCharacterButtons[2].RegisterCallback<ClickEvent>(LoadStats2);

                if (character.GetType() == typeof(ArcherCharacter))              
                    selectCharacterButtons[3].RegisterCallback<ClickEvent>(LoadStats3);
                
            }
        }  
        
        if(deadCount==4)
        {
            GameManager.DeleteSaveFile();
            Application.Quit();
        }

        List<Button> messageButtons = confirmationMessage.Query<Button>().ToList();
        messageButtons[0].RegisterCallback<ClickEvent>(EnterTower);
        messageButtons[1].RegisterCallback<ClickEvent>(CloseMessageEvent);
    }
    private void NoEvent(ClickEvent evt) { }
    private void CloseMessageEvent(ClickEvent evt)
    {
        CloseMessage();
    }
    private void CloseMessage()
    {
        confirmationMessage.style.display = DisplayStyle.None;
    }
    private void EnterTower(ClickEvent evt)
    {
        gameManager.playerData.currentFloor++;
        gameManager.SaveFile();
        SceneManager.LoadScene("SampleScene");
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
        ost.Play();
        AvatarController.selectCharacter = false;

        gameManager.playerData.currentCharacter = value;
        player.currentCharacter = value;
        player.ChangeCharacter(value);
        player.ChangeStats(value);
        selectCharacter.style.display = DisplayStyle.None;


    }
    //    btnStats.style.backgroundColor = new StyleColor(new Color32(59, 59, 59, 255));
    //    groupStats.style.display = DisplayStyle.Flex;

   

    private void ChangeDisplay(string name)
    {
        var character = GameObject.Find(name);

        List<GameObject> equipmentList = Resources.FindObjectsOfTypeAll<GameObject>().ToList();


        // Iterar sobre los objetos secundarios
        foreach (GameObject childObject in equipmentList)
        {
            if (childObject.transform.IsChildOf(character.transform))
            {
                // Hacer algo con cada objeto secundario
                if (childObject.CompareTag("Revenant"))
                {
                    childObject.SetActive(true);
                }
                
                if (childObject.CompareTag(name))
                {
                    childObject.SetActive(false);
                }
                
            }
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        confirmationMessage.style.display = DisplayStyle.Flex;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CloseMessage();
    }
}
