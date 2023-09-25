using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    public int money;
    public int livesCurrent;
    public int livesMax = 30;

    private void Start()
    {
        livesCurrent = livesMax;
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

    }
}
