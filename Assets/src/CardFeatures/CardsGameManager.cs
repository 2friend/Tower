using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CardsGameManager : MonoBehaviour
{
    private const int MAX_CADS_IN_HAND = 6;

    public Deck playerDeck;
    public Deck enemyDeck;

    public List<Card> playerHand = new List<Card>();
    public List<Card> enemyHand = new List<Card>();

    [SerializeField]
    private GameObject playerHandObj;
    [SerializeField]
    private GameObject enemyHandObj;

    [SerializeField]
    private GameObject magicCardPref;
    [SerializeField]
    private GameObject towerCardPref;
    [SerializeField]
    private GameObject enemyCardPref;

    public int turnCount;

    private CardObject currentActiveCard;

    public void StartGame()
    {
        GetCardsToHand(playerDeck, playerHand, 4);
        //GetCardsToHand(enemyDeck, enemyHand, 4);
    }

    public void NextTurn()
    {
        turnCount++;

        GetCardsToHand(playerDeck, playerHand, 1);
        //GetCardsToHand(enemyDeck, enemyHand, 1);
    }

    private void GetCardsToHand(Deck _deck, List<Card> _hand, int _count)
    {
        if (_deck.cards.Count < _count)
        {
            Debug.LogError("[Loading] [CardGameManager] Not enough cards in the deck.");
            return;
        }

        int n = _deck.cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card value = _deck.cards[k];
            _deck.cards[k] = _deck.cards[n];
            _deck.cards[n] = value;
        }

        if (_count > _deck.cards.Count)
        {
            _count = _deck.cards.Count;
        }

        List<Card> selectedCards = _deck.cards.GetRange(0, _count);

        if (_hand.Count + 1 >= MAX_CADS_IN_HAND)
        {

        }

        foreach (Card card in selectedCards)
        {
            _hand.Add(card);
            if (card.cardType is Magic)
            {
                GameObject handCard = Instantiate(magicCardPref, Vector3.zero, Quaternion.identity);
                handCard.GetComponent<CardObject>().InitCard(card);
                handCard.transform.SetParent(playerHandObj.transform);
                handCard.transform.localScale = Vector3.one;
            }
            else if (card.cardType is EnemyBD)
            {
                GameObject handCard = Instantiate(enemyCardPref, Vector3.zero, Quaternion.identity);
                handCard.GetComponent<CardObject>().InitCard(card);
                handCard.transform.SetParent(playerHandObj.transform);
                handCard.transform.localScale = Vector3.one;
            }
            else if (card.cardType is Tower)
            {
                GameObject handCard = Instantiate(towerCardPref, Vector3.zero, Quaternion.identity);
                handCard.GetComponent<CardObject>().InitCard(card);
                handCard.transform.SetParent(playerHandObj.transform);
                handCard.transform.localScale = Vector3.one;
            }
            _deck.cards.Remove(card);
        }
    }

    public CardObject GetCurrentActiveCard()
    {
        return currentActiveCard;
    }

    public bool UpdateCurrentActiveCard(CardObject value)
    {
        if (value != null)
        {
            currentActiveCard = value;
            return true;
        }

        return false;
    }

    public bool IsMaxCardsInHand()
    {


        return false;
    }
}
