using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    public int money;
    public int livesCurrent;
    public int livesMax = 30;


    [SerializeField] private TextMeshProUGUI moneyCountText;
    [SerializeField] private WaveController wave;

    private void Start()
    {
        livesCurrent = livesMax;
        money = 100;
        wave = GameObject.Find("MainCamera").GetComponent<WaveController>();
        moneyCountText = GameObject.Find("MoneyCount").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        moneyCountText.text = money.ToString();
    }

    public void TakingDamage(int _dmg)
    {
        if(livesCurrent - _dmg > 0)
        {
            Debug.Log("[Gameplay] [Player] Player Taking Damage: %" + _dmg.ToString() + "%");
            livesCurrent -= _dmg;
        }
        else
        {
            Debug.Log("[Gameplay] [Player] Player Die!");
            Die();
        }
    }

    private void Die()
    {
        wave.enabled = false;
    }
}
