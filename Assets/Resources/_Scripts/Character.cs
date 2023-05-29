using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Character
{

    private float speed = 3;
    private int level = 1;
    public bool isDead;
    private float hp,stamina,exp,atk,magAtk,magDef,def,maxLevel;

    public float Hp { get => hp; set => hp = value; }
    public float Stamina { get => stamina; set => stamina = value; }
    public float Exp { get => exp; set => exp = value; }
    public float Atk { get => atk; set => atk = value; }
    public float MagAtk { get => magAtk; set => magAtk = value; }
    public float MagDef { get => magDef; set => magDef = value; }
    public float Def { get => def; set => def = value; }
    public float MaxLevel { get => maxLevel; set => maxLevel = value; }
    public float Speed { get => speed; set => speed = value; }
    public int Level { get => level; set => level = value; }

    public void LevelUP()
    {
        exp=0;
        level++;
    }
   
    public void SetLV(int level, float exp)
    {

        this.level=level;
        this.exp =exp;
    }

   
}
[Serializable]

public class MinionCharacter : Character
{
    public MinionCharacter()
    {
        Hp = 13;
        Speed = 4;
    }
}

[Serializable]

public class MageCharacter : Character
{
    public MageCharacter()
    {
        Hp = 9;
        Stamina = 9;
        MagAtk = 12;
        MagDef = 12;
    }
}

[Serializable]
public class WarriorCharacter : Character
{
    public WarriorCharacter()
    {
        Hp = 12;
        Def = 12;
        Speed = 2;
    }
}
[Serializable]

public class ArcherCharacter : Character
{
    public ArcherCharacter()
    {
        Hp = 9;
        Atk= 12;
        Speed = 5;
    }

}

