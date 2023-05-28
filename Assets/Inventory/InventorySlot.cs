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
                Icon.image = null;
                ItemGuid = "";
            }
        }
    }
}

