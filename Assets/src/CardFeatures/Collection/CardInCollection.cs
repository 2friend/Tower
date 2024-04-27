using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInCollection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool isCreated;
    public int count;
    private Card currentCard;

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCreated)
        {
            ShowCardDescriptionUI();
        }
        else
        {
            CreateCardUI();
        }
    }

    private void CreateCardUI()
    {

    }

    private void ShowCardDescriptionUI()
    {

    }
}
