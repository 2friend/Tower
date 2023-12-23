using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    public Card currentCard;
    public ICardType currentCardType;

    public Image backgroundSprite;
    public Image atrSprite;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI costText;

    [SerializeField]
    private Image star1, star2, star3;
    [SerializeField]
    private Sprite starSprite;

    public void InitCard(Card _card)
    {
        currentCard = _card;
        currentCardType = _card.cardType;

        switch(currentCardType)
        {
            case Magic magicCard:
                GetComponent<MagicCard>().InitMagic(currentCard);
                break;
            case EnemyBD enemyCard:
                GetComponent<UnitCard>().InitUnit(currentCard);
                break;
            case Tower towerCard:
                GetComponent<TowerCard>().InitTower(currentCard);
                break;
        }

        backgroundSprite.sprite = currentCard.Background;
        atrSprite.sprite = currentCard.sprite;

        nameText.text = currentCard.Name;
        descText.text = currentCard.Description;
        costText.text = currentCard.MoneyCost.ToString();

        switch (currentCard.rare)
        {
            case 1:
                star2.sprite = starSprite;
                break;
            case 2:
                star1.sprite = starSprite;
                star2.sprite = starSprite;
                break;
            case 3:
                star1.sprite = starSprite;
                star2.sprite = starSprite;
                star3.sprite = starSprite;
                break;
            case 0:
                break;
        }
    }
}
