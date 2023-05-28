using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StartScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    public VisualElement selectCharacter;
    private bool entryTower;
    AvatarController player;
    void Start()
    {
        selectCharacter= GameObject.Find("SelectCharacter").GetComponent<UIDocument>().rootVisualElement;
        gameManager = GetComponent<GameManager>();
        ReadyUI();
        player = GameObject.Find("Player").GetComponent<AvatarController>();

    }

    private void ReadyUI()
    {
        List<Button> groupStats = selectCharacter.Query<Button>().ToList();
        
            groupStats[0].RegisterCallback<ClickEvent>(LoadStats2);
            groupStats[1].RegisterCallback<ClickEvent>(LoadStats1);
            groupStats[2].RegisterCallback<ClickEvent>(LoadStats);
            groupStats[3].RegisterCallback<ClickEvent>(LoadStats3);
            

        

    }
    private void LoadStats(ClickEvent evt){ ChangeCharacter(0); }
    private void LoadStats1(ClickEvent evt){ ChangeCharacter(1); }
    private void LoadStats2(ClickEvent evt){ ChangeCharacter(2); }
    private void LoadStats3(ClickEvent evt){ ChangeCharacter(3); }
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
            Debug.Log("Let me in, Let meeee iiiiiiiiiiiiiiinnnnnnnnn");
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        entryTower= false;
    }
}
