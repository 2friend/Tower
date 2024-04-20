using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
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
    private RectTransform rect;

    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;

    private string debugCardName = "[Gameplay] [CardGame] Playing ";

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
    }


    public void InitCard(Card _card)
    {
        currentCard = _card;
        currentCardType = _card.cardType;

        switch(currentCardType)
        {
            case Magic magicCard:
                GetComponent<MagicCard>().InitMagic(currentCard);
                debugCardName += "Magic Card: %";
                break;
            case EnemyBD enemyCard:
                GetComponent<UnitCard>().InitUnit(currentCard);
                debugCardName += "Unit Card: %";
                break;
            case Tower towerCard:
                GetComponent<TowerCard>().InitTower(currentCard);
                debugCardName += "Tower Card: %";
                break;
        }

        debugCardName += $"{currentCard.Name}%";

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

        originalPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        GameConstant.cardsGameManager.UpdateCurrentActiveCard(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform canvasRectTransform = transform.parent as RectTransform;

        if (canvasRectTransform != null)
        {
            Vector3 globalMousePos;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                transform.position = globalMousePos;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (Vector3.Distance(transform.position, originalPosition) > 50)
        {
            PlayCard();
        }
        else
        {
            transform.position = originalPosition;
        }

        GameConstant.cardsGameManager.UpdateCurrentActiveCard(null);
    }

    private void PlayCard()
    {
        Debug.Log(debugCardName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.localScale = Vector3.one*1.1f;
        GameConstant.cameraMover.BlockCameraMove(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = Vector3.one;
        GameConstant.cameraMover.BlockCameraMove(true);
    }
}
