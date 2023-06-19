using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class floorIndicator : MonoBehaviour
{
    Label lbl;
    // Start is called before the first frame update
    private void Start()
    {
        lbl = GetComponent<UIDocument>().rootVisualElement.Q("floor") as Label;
        lbl.text = $"FLOOR {GameObject.Find("GameManager").GetComponent<GameManager>().playerData.currentFloor}";
        StartCoroutine(MoveAndReturn());
    }

    private IEnumerator MoveAndReturn()
    {
        Time.timeScale= 0;
        for (float i = 0; i < 100; i++)
        {
            lbl.style.opacity = i/100f;
            yield return new WaitForSecondsRealtime(0.005f);
        }
        yield return new WaitForSecondsRealtime(0.75f);

        for (float i = 100; i > 0; i--)
        {
            lbl.style.opacity = i / 100f;
            yield return new WaitForSecondsRealtime(0.005f);
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
