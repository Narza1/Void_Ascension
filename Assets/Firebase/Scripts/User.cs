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

[System.Serializable]
public class CharacterData
{
    public int characterType, lv, floor, money;
    public string weaponGUID, accessoryGUID, consumableGUID;
    public DropItem drop;
    
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

   
    //public DropItem Drop { get => drop; set => drop = value; }
}

[System.Serializable]
public class DropItem
{
    public string itemGUID;
    public int quantity;
    public DropItem() { }
    public DropItem(string itemGUID, int quantity)
    {
        this.itemGUID = itemGUID;
        this.quantity = quantity;
    }

}
