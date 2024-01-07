using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using UnityEngine;

// TODO: Refactor. Finish Reallisation
public class DeckManager : MonoBehaviour
{
    private const int MAX_CARDS_IN_DECK = 30;
    private const int MAX_REPLY_CARDS_IN_DECK = 2;

    private const string DECK_VAR = "Deck";
    private const string NAME_ATT = "name";
    private const string CLASS_ATT = "class";
    private const string CARD_VAR = "card";
    private const string CARD_ID_ATT = "id";
    private const string CARD_COUNT_ATT = "count";

    private string directoryPath;
    private string[] allDecks;
    public List<Deck> decks = new List<Deck>();

    public Hero currHero;
    public Deck currDeck;

    public void ReadAllDecks()
    {
        string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        directoryPath = Path.Combine(myDocumentsPath, "2friends");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        allDecks = Directory.GetFiles(directoryPath);

        if (allDecks.Length < 1)
        {
            CreateDefaultDecks();
            Debug.LogError("[!] [Loading] [Decks] No Decks Found!");
            return;
        }

        allDecks = Directory.GetFiles(directoryPath);

        foreach (string file in allDecks)
        {
            LoadDeck(file);
        }
        
        Debug.Log($"[Loading] [Decks] Loaded Decks Count %{allDecks.Length}%");
    }

    private void LoadDeck(string _path)
    {
        try
        {
            string xmlText = File.ReadAllText(_path);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);

            XmlNodeList _decks = xmlDoc.GetElementsByTagName(DECK_VAR);

            foreach (XmlNode _deck in _decks)
            {
                int index = 0;

                string _deckName = _deck.Attributes[NAME_ATT].Value;
                string _deckClass = _deck.Attributes[CLASS_ATT].Value;

                List<Card> _cards = new List<Card>();

                Debug.Log($"[Loading] [Decks] Deck File: {_path}");

                XmlNodeList cards = _deck.SelectNodes(CARD_VAR);

                foreach (XmlNode _card in cards)
                {
                    index++;

                    string _cardId = _card.Attributes[CARD_ID_ATT].Value;
                    if (string.IsNullOrEmpty(_cardId))
                        Debug.LogError($"[!] [Loading] [Decks] Card With Index: {index} Have No ID!");

                    if (!CardDB.TryFoundCardById(_cardId))
                    {
                        Debug.LogWarning($"[?] [Loading] [Decks] No Such Card Found: %{_cardId}%");
                        continue;
                    }

                    int _cardCount = Convert.ToInt32(_card.Attributes[CARD_COUNT_ATT].Value);
                    if (_cardCount <= 0)
                        Debug.LogError($"[!] [Loading] [Decks] Card With ID: {_cardId} Dont Have Count!");

                    if (_cardCount <= 2)
                    {
                        for (int i = 0; i < _cardCount; i++)
                        {
                            _cards.Add(CardDB.GetCardByID(_cardId));
                        }
                    }
                    else
                    {
                        Debug.LogError($"[!] [Loading] [Decks] Count Cant Be > 2!");
                        continue;
                    }
                }

                Deck deck = new Deck(_deckName, _deckClass, _cards);

                Debug.Log($"[Loading] [Deck] Loaded New Deck With Cards: {_cards.Count}");

                decks.Add(deck);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[!] [Loading] [Decks] Error: {ex.Message}");
        }
    }

    private void CreateDefaultDecks()
    {
        XmlDocument xmlDoc = new XmlDocument();

        XmlElement rootElement = xmlDoc.CreateElement("Deck");
        rootElement.SetAttribute("name", "Test");
        rootElement.SetAttribute("class", "Test");

        xmlDoc.AppendChild(rootElement);

        XmlElement childElement1 = xmlDoc.CreateElement("card");
        childElement1.SetAttribute("id", "0");
        childElement1.SetAttribute("count", "2");
        rootElement.AppendChild(childElement1);

        string savePath = directoryPath + "\\test.xml";
        xmlDoc.Save(savePath);
    }

    public Deck GetDeckByName(string _name)
    {
        Deck _deck = null;


        foreach (Deck item in decks)
        {
            if (item.name == _name)
                _deck = item;
        }

        if (_deck == null)
            Debug.LogError($"[!] [Loading] [Magic] No Such Deck Found: %{_name}%!");

        return _deck;
    }

    public void AddCardToDeck(Card _card)
    {
        if(currDeck.cards.Count+1 <= MAX_CARDS_IN_DECK)
        {
            if (CheckReplies(_card))
            {
                currDeck.cards.Add(_card);
            }
        }
    }

    private bool CheckReplies(Card _card)
    {
        int index = 0;

        for(int i = 0; i >= currDeck.cards.Count; i++)
        {
            if (currDeck.cards[i].Name == _card.Name)
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
    public List<Card> cards = new List<Card>();

    public Deck(string _id, string _name, List<Card> _cards)
    {
        id = _id;
        name = _name;
        cards = _cards;
    }

}
