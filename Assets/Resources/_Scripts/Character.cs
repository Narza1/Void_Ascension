using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{

    protected float hp = 10, stamina= 10, exp = 0, atk = 10, magAtk= 10,magDef= 10, def = 10, maxLevel=30, speed = 3;
    protected int level = 1;

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

