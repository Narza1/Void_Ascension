using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class InventorySlot : VisualElement
{
    public Image Icon;
    public string ItemGuid = "";
    public int quantity;
    public float durability = 100;


    public InventorySlot()
    {
        //Create a new Image element and add it to the root
        Icon = new Image();
        Add(Icon);
        //Add USS style properties to the elements
        Icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");
        RegisterCallback<PointerDownEvent>(OnPointerDown);
    }
    public void HoldItem(ItemDetails item)
    {
        Icon.image = item.Icon.texture;
        ItemGuid = item.GUID;
        quantity = item.quantity;
    }
    public void DropItem()
    {
        ItemGuid = "";
        Icon.image = null;
        quantity = 0;
        durability = 0;


    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        //Not the left mouse button
        if (evt.button != 0 || ItemGuid.Equals(""))
        {
            return;
        }

        //Clear the image
        Icon.image = null;

        //Start the drag
        InventoryUIController.StartDrag(evt.position, this);
    }

    public void UseItem()
    {
        if (quantity != 0)
        {
            quantity--;
            if (quantity == 0)
            {
                DropItem();
            }
        }
    }
    private void OnMouseEnter()
    {
        
        // Realiza las acciones deseadas cuando el ratón entra en el objeto
        Debug.Log("Mouse entered");
    }

    private void OnMouseExit()
    {
        // Realiza las acciones deseadas cuando el ratón sale del objeto
        Debug.Log("Mouse exited");
    }
    private void Start()
    {
        // Obtener una referencia al VisualElement al que deseas agregar el evento de "hover"
        

        // Suscribirse al evento MouseEnterEvent
        this.RegisterCallback<MouseEnterEvent>(OnMouseEnter);

        // Suscribirse al evento MouseLeaveEvent
        this.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
    }

    private void OnMouseEnter(MouseEnterEvent evt)
    {
        // El cursor del ratón ha entrado en el elemento
        Debug.Log("Mouse entered!");

        // Aquí puedes realizar las acciones que desees cuando el cursor entre en el elemento
    }

    private void OnMouseLeave(MouseLeaveEvent evt)
    {
        // El cursor del ratón ha salido del elemento
        Debug.Log("Mouse left!");

        // Aquí puedes realizar las acciones que desees cuando el cursor salga del elemento
    }
}

