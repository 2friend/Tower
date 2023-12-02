using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private const int MAX_CARDS_IN_DECK = 30;
    private const int MAX_REPLY_CARDS_IN_DECK = 2;

    private const string XML_PATH = "Data/Decks/Cards";
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

    private int currCardsInDeck = 0;

    public Hero currHero;
    public List<Deck> decks = new List<Deck>();
    public List<Card> currDeck = new List<Card>();

    public void LoadAllDecks()
    {
        TextAsset binary = Resources.Load<TextAsset>(XML_PATH);
        if (binary == null)
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
                Debug.Log("[Loading] [Decks] Reading Cards File: %" + XML_PATH + ".xml%");

                XmlReader inner = reader.ReadSubtree();

                while (inner.ReadToFollowing(CARD_NODE))
                {
                    index++;

                    string _cardID = reader.GetAttribute(ID_ATT);
                    if (_cardID == "")
                        Debug.LogError("[!] [Loading] [Decks] Card With Index: %" + index.ToString() + "% Have No ID!");

                    string _cardName = reader.GetAttribute(NAME_ATT);
                    if (_cardName == "")
                        Debug.LogError("[!] [Loading] [Decks] Card With ID: %" + _cardID + "% Have No Name!");

                    reader.Read();

                    Deck deck = new Deck(_cardID, _cardName);

                    decks.Add(deck);
                }
                inner.Close();
            }
        }
        Debug.Log("[Loading] [Decks] Loaded Decks Count %" + decks.Count + "%");

        Debug.Log("[Loading] [Decks] End of Reading Decks File: %" + XML_PATH + ".xml%");
    }

    public void AddCardToDeck(Card _card)
    {
        if(currCardsInDeck++ <= MAX_CARDS_IN_DECK)
        {
            if (CheckReplies(_card))
            {
                currDeck.Add(_card);
                currCardsInDeck++;
            }
        }
    }

    private bool CheckReplies(Card _card)
    {
        int index = 0;

        for(int i = 0; i >= currDeck.Count; i++)
        {
            if (currDeck[i].Name == _card.Name)
                index++;
        }

        if (index <= MAX_REPLY_CARDS_IN_DECK)
            return true;
        else
            return false;
    }
}

public class Deck
{
    public string id;
    public string name;
    List<Card> cards = new List<Card>();

    public Deck(string _id, string _name)
    {
        id = _id;
        name = _name;
    }
}
