using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterWindowUIController : MonoBehaviour
{
    // Start is called before the first frame update
    private VisualElement m_Root, statsWindow, setWindow, set1, set2;
    private Button btnStats, btnSet1, btnSet2;
    private GroupBox groupStats, groupSet;
    void Awake()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;

        //Search the root for the SlotContainer Visual Element
        //You can use either Q or Query to search for a specific element within the hierarchy.
        //You can return the first result of the query by appending.First() at the end
        //and the last by appending .Last().For example: m_SlotContainer = m_Root.Query("SlotContainer").First();
        groupStats = m_Root.Query<GroupBox>("groupStats");
       
        groupSet = m_Root.Query<GroupBox>("groupSet");
        statsWindow = m_Root.Q("groupStats");
        setWindow = m_Root.Q("groupSet");
        set1 = m_Root.Q("Set1");
        set2 = m_Root.Q("Set2");
        btnStats = m_Root.Query<Button>("btnStats");
        btnSet1 = m_Root.Query<Button>("btn1");
        btnSet2 = m_Root.Query<Button>("btn2");
        btnStats.RegisterCallback<ClickEvent>(LoadStats);
        btnSet1.RegisterCallback<ClickEvent>(LoadSet1);
        btnSet2.RegisterCallback<ClickEvent>(LoadSet2);
    }


    private void LoadStats(ClickEvent evt)
    {
        ResetColors();
        ButtonReset();

        btnStats.style.backgroundColor = new StyleColor(new Color32(59, 59, 59, 255));
        groupStats.style.display = DisplayStyle.Flex;

    }
    private void LoadSet1(ClickEvent evt)
    {
        ResetColors();
        ButtonReset();

        btnSet1.style.backgroundColor = new StyleColor(new Color32(59, 59, 59, 255));
        groupSet.style.display = DisplayStyle.Flex;
        set1.style.display = DisplayStyle.Flex;




    }

    private void LoadSet2(ClickEvent evt)
    {
        ResetColors();
        ButtonReset();

        btnSet2.style.backgroundColor = new StyleColor(new Color32(59, 59, 59, 255));
        groupSet.style.display = DisplayStyle.Flex;
        set2.style.display = DisplayStyle.Flex;

    }

    private void ResetColors() {
        btnStats.style.backgroundColor = new StyleColor(new Color32(77, 77, 77, 255));
        btnSet1.style.backgroundColor = new StyleColor(new Color32(77, 77, 77, 255));
        btnSet2.style.backgroundColor = new StyleColor(new Color32(77, 77, 77, 255));
    }

    private void ButtonReset()
    {
        groupStats.style.display = DisplayStyle.None;
        groupStats.style.display = DisplayStyle.None;
        groupSet.style.display = DisplayStyle.None;
        set1.style.display = DisplayStyle.None;
        set2.style.display = DisplayStyle.None;
    }
}
