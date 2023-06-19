using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    
    void Start()
    {

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.Q("btnMainMenu").RegisterCallback<ClickEvent>(MainMenu);
        root.Q("btnExit").RegisterCallback<ClickEvent>(Exit);

    }
    private void MainMenu(ClickEvent evt) { SceneManager.LoadScene("IntroScene");}
    private void Exit(ClickEvent evt) { Application.Quit(); }
    // Update is called once per frame
    void Update()
    {
        
    }
}
