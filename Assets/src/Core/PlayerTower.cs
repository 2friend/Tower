using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    public int money;
    public int livesCurrent;
    public int livesMax = 30;

    [SerializeField] private WaveController wave;

    private void Start()
    {
        livesCurrent = livesMax;
        wave = GameObject.Find("MainCamera").GetComponent<WaveController>();
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
