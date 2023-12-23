using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCard : CardObject
{
    public Tower currentTower;

    public TextMeshProUGUI atackText;
    public TextMeshProUGUI speedText;

    public void InitTower(Card _card)
    {
        currentTower = (Tower)_card.cardType;

        atackText.text = currentTower.bulletType.dmg.ToString();
        speedText.text = currentTower.atkspd.ToString();
    }
}
