using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CollectionManager : MonoBehaviour
{
    public List<Card> AllCards = new List<Card>();
    public List<Card> CollectedCards = new List<Card>();
    public List<GameObject> ShowedCards = new List<GameObject>();

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject cardPref;

    public bool LoadAllCards()
    {
        return true;
    }

    public bool LoadAllPlayerDecks()
    {
        return true;
    }

    public bool LoadAllCollectedCard()
    {
        return true;
    }

    public void AddCardToCollection(Card card)
    {

        CollectedCards.Add(card);
    }

    public void ShowCardsForSelectedHero(Hero value)
    {
        Debug.Log($"[Collection] Start Filtering Cards By Hero Type: %{value.Name}%...");

        ClearShowedCards();

        foreach (Card card in AllCards)
        {
            if (card.heroClass == value)
            {
                GameObject cardObj = Instantiate(cardPref);
                cardObj.GetComponent<CardObject>().InitCard(card);
                ShowedCards.Add(cardObj);
            }
        }

        UpdateShowedCards();

        Debug.Log($"[Collection] ...Filtering Ended!");
    }

    private void UpdateShowedCards()
    {
        foreach (GameObject card in ShowedCards)
        {
            card.transform.SetParent(parent);
        }

        Debug.Log("[Collection] Showed Cards Updated!");
    }

    private void ClearShowedCards()
    {
        foreach (GameObject card in ShowedCards)
        {
            Destroy(card);
        }

        Debug.Log("[Collection] Cleared All Showed Cards!");
    }
}
