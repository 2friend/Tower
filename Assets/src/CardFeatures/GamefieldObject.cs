using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamefieldObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameConstant.cardsGameManager.GetCurrentActiveCard() != null)
        {
            var activeCard = GameConstant.cardsGameManager.GetCurrentActiveCard();
            switch (activeCard.currentCardType) 
            {
                case EnemyBD:
                    Debug.Log("[Gameplay] Placing Unit Card");
                    break;
                case Tower:
                    Debug.Log("[Gameplay] Placing Tower Card");
                    break;
                case Magic:
                    Debug.Log("[Gameplay] Placing Magic Card");
                    break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ddfg
    }
}
