using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    private const string ABILITY_FILE_PATH = "";

    private const string HERO_FILE_PATH = "";

    public void LoadAbilities()
    {

    }

    public void LoadHeroes()
    {

    }
}

public class Hero
{
    public string name;
    public Sprite sprite;
    public Ability ability;
    public int maxHP;
}

public class Ability
{
    public enum type
    {
        HEAL = 0,
        DAMAGE = 1,
        ATACK_BOOST = 2,
        HP_BOOST = 3
    }

    public string name;
    public Sprite sprite;
    public type currentType;
    public bool isGlobal;
    public int value;
}