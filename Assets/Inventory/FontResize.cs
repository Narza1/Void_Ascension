using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FontResize : MonoBehaviour
{
    public UIDocument uiDoc; // assign it in inspector
    private VisualElement root;

    private void Awake()
    {
        root = uiDoc.rootVisualElement;


        //List<Button> myButtons = root.Query<Button>("btn").ToList();

        //List<Label> myLabels = root.Query<Label>("lbl").ToList();
       

        //foreach (Button button in myButtons)
        //{
        //    Resize(button);
        //}

        //foreach (Label lbl in myLabels)
        //{
        //    ResizeLabel(lbl);
        //}
       
        // Llamada a la coroutine
        StartCoroutine(WaitOneFrame());
    }

    private void Resize(Button btn)
    {
      
        string btnText = btn.text;

        float btnWidth = btn.resolvedStyle.width;

        float f = btnWidth / btnText.Length;
        btn.style.fontSize = f;
        btn.MarkDirtyRepaint();
    }

    private void ResizeLabel(Label lbl)
    {

        string lblText = lbl.text;

        float btnWidth = lbl.resolvedStyle.width;

        float f =  btnWidth / lblText.Length;
        lbl.style.fontSize = f;
        lbl.MarkDirtyRepaint();
    }
    IEnumerator WaitOneFrame()
    {
        
        yield return new WaitForEndOfFrame();

        // Buscar todos los botones que contienen "my-button" en el nombre
        List<Button> buttons = root.Query<Button>().ToList();
        List<Label> labels = root.Query<Label>().ToList();
        foreach (var button in buttons)
        {
            Resize(button);
        }

        foreach (var label in labels)
        {
            ResizeLabel(label);
        }


        // Aquí puedes poner el código que quieres ejecutar después de esperar dos frames
    }



}
