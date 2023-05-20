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
    protected bool isAlive;
    private float hp;
    private float stamina;
    private float exp;
    private float atk;
    private float magAtk;
    private float magDef;
    private float def;
    private float maxLevel;

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

public class MinionCharacter : Character
{
    public MinionCharacter()
    {
        hp = 13;
        speed = 4;
    }
}


public class MageCharacter : Character
{
    public MageCharacter()
    {
        hp = 9;
        stamina = 9;
        magAtk = 12;
        magDef = 12;
    }
}

public class WarriorCharacter : Character
{
    public WarriorCharacter()
    {
        hp = 12;
        def = 12;
        speed = 2;
    }
}

public class ArcherCharacter : Character
{
    public ArcherCharacter()
    {
        hp = 9;
        atk= 12;
        speed = 5;
    }

}

