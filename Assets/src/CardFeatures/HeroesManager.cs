using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    private const string ABILITY_FILE_PATH = "Data/Cards/Abilities";
    private const string ABILITY_OBJ = "Ability";
    private const string ABILITY_NAME_ATT = "name";
    private const string ABILITY_SPRITE_ATT = "sprite";
    private const string ABILITY_TYPE_ATT = "type";
    private const string ABILITY_GLOBAL_ATT = "isGlobal";
    private const string ABILITY_VALUE_ATT = "value";

    private const string HERO_FILE_PATH = "Data/Cards/Heroes";
    private const string HERO_OBJ = "Hero";
    private const string HERO_NAME_ATT = "name";
    private const string HERO_SPRITE_ATT = "sprite";
    private const string HERO_ABILITY_ATT = "ability";
    private const string HERO_MAXHP_ATT = "hp";

    public List<Ability> abilities = new List<Ability>();

    public List<Hero> heroes = new List<Hero>();

    public void LoadAbilities()
    {
        abilities.Clear();

        TextAsset binary = Resources.Load<TextAsset>(ABILITY_FILE_PATH);

        if (binary == null)
        {
            Debug.LogError("[!] [Loading] [Abilities] No Such File: %" + ABILITY_FILE_PATH + "% To Read!");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));

        Debug.Log("[Loading] [Abilities] Reading Abilities File: " + ABILITY_FILE_PATH + ".xml");

        int index = 0;

        while (reader.Read())
        {
            index++;
            if (reader.IsStartElement(ABILITY_OBJ))
            {
                string _abilityName = reader.GetAttribute(ABILITY_NAME_ATT);
                if (_abilityName == null)
                    Debug.LogError("[!] [Loading] [Abilities] Ability With Index: %" + index.ToString() + "% Have No Name!");

                string _abilitySprite = reader.GetAttribute(ABILITY_SPRITE_ATT);
                if (_abilitySprite == null)
                    Debug.LogError("[!] [Loading] [Abilities] Ability With ID: %" + _abilityName + "% Have No Sprite!");

                string _abilityType = reader.GetAttribute(ABILITY_TYPE_ATT);
                if (_abilityType == null)
                    Debug.LogError("[!] [Loading] [Abilities] Ability With ID: %" + _abilityName + "% Have No Ability Type!");

                bool _abilityGlobal = Convert.ToBoolean(reader.GetAttribute(ABILITY_GLOBAL_ATT));

                int _abilityValue = Convert.ToInt32(reader.GetAttribute(ABILITY_VALUE_ATT));

                Ability _ability = new Ability(_abilityName, _abilitySprite, _abilityType, _abilityGlobal, _abilityValue);

                abilities.Add(_ability);
            }
        }

        reader.Close();
        Debug.Log("[Loading] [Abilities] Reading Finished, Loaded: " + abilities.Count + " Abilities!");
    }

    public void LoadHeroes()
    {
        heroes.Clear();

        TextAsset binary = Resources.Load<TextAsset>(HERO_FILE_PATH);

        if (binary == null)
        {
            Debug.LogError("[!] [Loading] [Heroes] No Such File: %" + HERO_FILE_PATH + "% To Read!");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));

        Debug.Log("[Loading] [Heroes] Reading Abilities File: " + HERO_FILE_PATH + ".xml");

        int index = 0;

        while (reader.Read())
        {
            index++;
            if (reader.IsStartElement(HERO_OBJ))
            {
                string _heroName = reader.GetAttribute(HERO_NAME_ATT);
                if (_heroName == null)
                    Debug.LogError("[!] [Loading] [Heroes] Hero With Index: %" + index.ToString() + "% Have No Name!");

                string _heroSprite = reader.GetAttribute(HERO_SPRITE_ATT);
                if (_heroSprite == null)
                    Debug.LogError("[!] [Loading] [Heroes] Hero With ID: %" + _heroName + "% Have No Sprite!");

                string _heroAbility = reader.GetAttribute(HERO_ABILITY_ATT);
                if (_heroAbility == null)
                    Debug.LogError("[!] [Loading] [Heroes] Hero With ID: %" + _heroName + "% Have No Ability!");

                int _heroHP = Convert.ToInt32(reader.GetAttribute(HERO_MAXHP_ATT));

                Hero _hero = new Hero(_heroName, _heroSprite, _heroAbility, _heroHP);

                heroes.Add(_hero);
            }
        }

        reader.Close();
        Debug.Log("[Loading] [Heroes] Reading Finished, Loaded: " + abilities.Count + " Abilities!");
    }

    public Ability GetAbilityByID(string _name)
    {
        Ability _ability = null;

        foreach(Ability item in abilities)
        {
            if (item.name == _name)
            {
                _ability = item;
            }
        }

        if(_ability == null)
            Debug.LogError("[!] [Loading] [Abilities] No Such Ability Found: %" + _name + "%");

        return _ability;
    }

    public Hero GetHeroByID(string _name)
    {
        Hero _hero = null;

        foreach (Hero item in heroes)
        {
            if (item.name == _name)
            {
                _hero = item;
            }
        }

        if (_hero == null)
            Debug.LogError("[!] [Loading] [Heroes] No Such Hero Found: %" + _name + "%");

        return _hero;
    }
}

public class Hero
{
    public string name;
    public Sprite sprite;
    public Ability ability;
    public int maxHP;

    public Hero(string _name, string _sprite, string _ability, int _maxHP)
    {
        name = _name;
        sprite = GameConstant.resourcesLoader.GetSpriteByID(_sprite);
        ability = GameConstant.heroesManager.GetAbilityByID(_ability);
        maxHP = _maxHP;
    }
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

    private bool _isChoosing;

    public Ability(string _name, string _sprite, string _type, bool _isGlobal, int _value)
    {
        name = _name;
        sprite = GameConstant.resourcesLoader.GetSpriteByID(_sprite);
        switch(_type)
        {
            case "Heal":
                currentType = type.HEAL;
                break;
            case "Damage":
                currentType = type.DAMAGE;
                break;
            case "AtackBoost":
                currentType = type.ATACK_BOOST;
                break;
            case "HPBoost":
                currentType = type.HP_BOOST;
                break;
        }
        isGlobal = _isGlobal;
        value = _value;
    }

    // Main Ability using realisation
    // Use ONLY THIS fuction in other classes
    public void TryUseAbility()
    {
        if (isGlobal)
            UseAbility();
        else
            UseAbility(ChooseTarget());
    }

    private GameObject ChooseTarget()
    {
        GameObject _target = null;

        return _target; 
    }

    private void UseAbility()
    {

    }

    private void UseAbility(GameObject _target)
    {
        if (_target == null)
            return;
    }
}