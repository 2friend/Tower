using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard : CardObject
{
    public EnemyBD currentUnit;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI speedText;

    public void InitUnit(Card _card)
    {
        currentUnit = (EnemyBD)_card.cardType;

        healthText.text = currentUnit.maxHp.ToString();
        speedText.text = currentUnit.speed.ToString();
    }
}
