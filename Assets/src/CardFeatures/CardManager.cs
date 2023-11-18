using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;



public class CardManager : MonoBehaviour
{
    private const string XML_PATH = "Data/Cards";

    private const string ROOT_NODE = "Root";

    private const string CARD_NODE = "Card";
    private const string ID_ATT = "id";
    private const string NAME_ATT = "name";
    private const string BACKGROUND_ATT = "background";
    private const string SPRITE_ATT = "sprite";
    private const string TYPE_ATT = "type";
    private const string TYPE_VALUE_ATT = "typeValue";
    private const string RARE_ATT = "rare";
    private const string MONEYCOST_ATT = "moneycost";

    [SerializeField]
    private bool extendedLogs;

    [SerializeField]
    private GridObjects gridObjects;

    private void Awake()
    {
        gridObjects = GameConstant._gridObjects;
    }

    public void LoadCards()
    {
        TextAsset binary = Resources.Load<TextAsset>(XML_PATH);
        if (binary==null)
        {
            Debug.LogError("[!] [Loading] [Cards] No Such File To Read: %" + XML_PATH + "%");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        int index = 0;

        while (reader.Read())
        {
            if (reader.IsStartElement(ROOT_NODE))
            {
                Debug.Log("[Loading] [Cards] Reading Cards File: %" + XML_PATH + ".xml%");

                XmlReader inner = reader.ReadSubtree();

                while (inner.ReadToFollowing(CARD_NODE))
                {
                    index++;

                    string _cardID = reader.GetAttribute(ID_ATT);
                    if (_cardID == "")
                        Debug.LogError("[!] [Loading] [Cards] Card With Index: %" + index.ToString() + "% Have No ID!");

                    string _cardName = reader.GetAttribute(NAME_ATT);
                    if (_cardName == "")
                        Debug.LogError("[!] [Loading] [Cards] Card With ID: %" + _cardID + "% Have No Name!");

                    string _cardBackground = reader.GetAttribute(BACKGROUND_ATT);
                    if (_cardBackground == "")
                        Debug.LogError("[!] [Loading] [Cards] Card With ID: %" + _cardID + "% Have No Background!");

                    string _cardSprite = reader.GetAttribute(SPRITE_ATT);
                    if (_cardSprite == "")
                        Debug.LogError("[!] [Loading] [Cards] Card With ID: %" + _cardID + "% Have No Sprite!");

                    string _cardType = reader.GetAttribute(TYPE_ATT);
                    if (_cardType == "")
                        Debug.LogError("[!] [Loading] [Cards] Card With ID: %" + _cardID + "% Have No Type!");

                    int _cardRare = Convert.ToInt32(reader.GetAttribute(RARE_ATT));

                    int _cardMoneycost = Convert.ToInt32(reader.GetAttribute(MONEYCOST_ATT));

                    string _cardTypeValue = reader.GetAttribute(TYPE_VALUE_ATT);

                    Tower _towerCard = null;
                    EnemyBD _enemyCard = null;

                    if (_cardType == "Tower")
                    {
                        foreach (var item in gridObjects.towers)
                        {
                            if (item.name == _cardTypeValue)
                            {
                                _towerCard = item;
                            }
                        };
                    }
                    else if (_cardType == "Enemy")
                    {
                        foreach (var item in gridObjects.enemys)
                        {
                            if (item.name == _cardTypeValue)
                            {
                                _enemyCard = item;
                            }
                        };
                    }

                    reader.Read();

                    Card card = new Card(_cardID, _cardName, _cardBackground, _cardSprite, _cardMoneycost, _cardType, _cardRare, _enemyCard, _towerCard);

                    if (extendedLogs)
                        Debug.Log("[Loading] [Cards] Loaded New Card: %" + _cardName + "%");

                    CardDB.AllCards.Add(card);
                }
                inner.Close();
            }
        }
        Debug.Log("[Loading] [Cards] Loaded Cards Count %" + CardDB.AllCards.Count + "%");

        Debug.Log("[Loading] [Cards] End of Reading Cards File: %" + XML_PATH + ".xml%");
    }
}

public struct Card
{
    public string Id;
    public string Name;
    public Sprite Background;
    public Sprite sprite;
    public EnemyBD enemy;
    public Tower tower;
    public int MoneyCost;
    public string cardType;
    public int rare;
    public bool isPlaced;

    public Card(string id, string name, string backgroundPath, string spritePath, int moneycost, string type, int stars, EnemyBD cardEnemy, Tower cardTower)
    {
        Id = id;
        Name = name;
        Background = Resources.Load<Sprite>(backgroundPath);
        sprite = Resources.Load<Sprite>(spritePath);
        MoneyCost = moneycost;
        cardType = type;
        enemy = cardEnemy;
        tower = cardTower;
        rare = stars;
        isPlaced = false;
    }
}

public static class CardDB
{
    public static List<Card> AllCards = new List<Card>();
}