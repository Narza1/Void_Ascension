using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using ColorUtility = UnityEngine.ColorUtility;

public class StoryTeller : MonoBehaviour
{
    [SerializeField]
    private GameObject[] images;
    [SerializeField]
    private string[] dialogs1,dialogs2,dialogs3;
    [SerializeField]
    private TMP_Text actualText;

    void Start()
    {
        StartCoroutine(BeginStoryTelling());  
    }

    private IEnumerator BeginStoryTelling()
    {
        Color color;
        color = actualText.color;
        yield return new WaitForSeconds(1);
        foreach (var text in dialogs1)
        {
            actualText.text = text;
            for (int i = 0; i < 100; i++)
            {
                color.a = i / 100f;
                actualText.color = color;
                yield return new WaitForSeconds(0.02f);

            }
            yield return new WaitForSeconds(4f);

        }
        SpriteRenderer image;
        for (int i = 0; i < dialogs2.Length; i++)   
        {
            image = images[i].GetComponent<SpriteRenderer>();
            actualText.text = dialogs2[i];
            var imageColor = image.color;
           
            for (int j = 0; j < 100; j++)
            {
                color.a = j/100f;
                actualText.color = color;
                imageColor.a = j / 100f;
                image.color = imageColor;
                yield return new WaitForSeconds(0.02f);

            }
            yield return new WaitForSeconds(4f);
            images[i].SetActive(false);

        }

        actualText.color = Color.white;
        foreach (var text in dialogs3)
        {
            actualText.text = text;


            for (int i = 0; i < 100; i++)
            {
                color.a = i / 100f;
                actualText.color = color;
                yield return new WaitForSeconds(0.02f);

            }
            yield return new WaitForSeconds(4f);

        }

        SceneManager.LoadScene("NewGameScene");

    }

    public void Skip()
    {
        SceneManager.LoadScene("NewGameScene");

    }
}
