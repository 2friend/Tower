using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicCard : CardObject
{
    public Magic currentMagic;

    public void InitMagic(Card _card)
    {
        currentMagic = (Magic)_card.cardType;
    }
}
