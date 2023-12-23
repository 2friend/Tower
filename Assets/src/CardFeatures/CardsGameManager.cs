using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsGameManager : MonoBehaviour
{
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

    public void StartGame()
    {
        GetCardsToHand(playerDeck, playerHand, 4);
        GetCardsToHand(enemyDeck, enemyHand, 4);
    }

    public void NextTurn()
    {
        turnCount++;

        GetCardsToHand(playerDeck, playerHand, 1);
        GetCardsToHand(enemyDeck, enemyHand, 1);
    }

    private void GetCardsToHand(Deck _deck, List<Card> _hand, int _count)
    {
        int n = _deck.cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card value = _deck.cards[k];
            _deck.cards[k] = _deck.cards[n];
            _deck.cards[n] = value;
        }

        List<Card> selectedCards = _deck.cards.GetRange(0, Mathf.Min(_count, _deck.cards.Count));

        foreach (Card card in selectedCards)
        {
            _hand.Add(card);
            if (card.cardType is Magic)
            {
                GameObject handCard = Instantiate(magicCardPref, Vector3.zero, Quaternion.identity);
                handCard.GetComponent<CardObject>().InitCard(card);
                handCard.transform.SetParent(playerHandObj.transform);
            }
            else if (card.cardType is EnemyBD)
            {
                GameObject handCard = Instantiate(enemyCardPref, Vector3.zero, Quaternion.identity);
                handCard.GetComponent<CardObject>().InitCard(card);
                handCard.transform.SetParent(playerHandObj.transform);
            }
            else if (card.cardType is Tower)
            {
                GameObject handCard = Instantiate(towerCardPref, Vector3.zero, Quaternion.identity);
                handCard.GetComponent<CardObject>().InitCard(card);
                handCard.transform.SetParent(playerHandObj.transform);
            }
            _deck.cards.Remove(card);
        }
    }
}
