using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class User
{
    public string name;
    public User(string name)
    {
        this.name = name;
    }

}

[Serializable]
public class CharacterData
{
    private int characterType, lv, floor, money;
    private string weaponGUID, accessoryGUID, consumableGUID;
    private DropItem drop;
    public CharacterData()
    {

    }

    public CharacterData(int characterType, int lv, int floor, int money, string weaponGUID, string accessoryGUID, string consumableGUID, DropItem drop)
    {
        this.characterType = characterType;
        this.lv = lv;
        this.floor = floor;
        this.money = money;
        this.weaponGUID = weaponGUID;
        this.accessoryGUID = accessoryGUID;
        this.consumableGUID = consumableGUID;
        this.drop = drop;
    }

    public int CharacterType { get => characterType; set => characterType = value; }
    public int Lv { get => lv; set => lv = value; }
    public int Floor { get => floor; set => floor = value; }
    public int Money { get => money; set => money = value; }
    public string WeaponGUID { get => weaponGUID; set => weaponGUID = value; }
    public string AccessoryGUID { get => accessoryGUID; set => accessoryGUID = value; }
    public string ConsumableGUID { get => consumableGUID; set => consumableGUID = value; }
    public DropItem Drop { get => drop; set => drop = value; }
}

[Serializable]
public class DropItem
{
    private string itemGUID;
    private int quantity;

    public DropItem(string itemGUID, int quantity)
    {
        this.itemGUID = itemGUID;
        this.quantity = quantity;
    }

    public string ItemGUI { get => itemGUID; set => itemGUID = value; }
    public int Quantity { get => quantity; set => quantity = value; }
}
