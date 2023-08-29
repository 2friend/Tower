using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using TMPro;

public class GridObjects : MonoBehaviour
{
    private List<Tower> towers = new List<Tower>();
    private List<Enemy> enemys = new List<Enemy>();

    private void ReadTowersFile()
    {

    }

    private void ReadEnemysFile()
    {

    }

    public void SpawnEnemy()
    {

    }

    public void BuyTower()
    {
        BuildTower();
    }

    private void BuildTower()
    {

    }
}

class Tower : MonoBehaviour
{
    private Bullet bulletType;
    private GameObject bulletObj;
    float atkspd;
    float atkspdCurrent;

    void Attack()
    {
        if (atkspdCurrent <= 0)
        {
            Instantiate(bulletObj);
            atkspdCurrent = atkspd;
        }
    }
}

class Bullet
{
    int dmg;
    float speed;
}

class Enemy
{

}
