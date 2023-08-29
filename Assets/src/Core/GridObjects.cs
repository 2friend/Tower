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

    public Tower(Bullet _bullet, GameObject _bulletObj, float _spd)
    {
        bulletType = _bullet;
        bulletObj = _bulletObj;
        atkspd = _spd;
    }

    void Attack()
    {
        if (atkspdCurrent <= 0)
        {
            Instantiate(bulletObj);
            atkspdCurrent = atkspd;
        }
    }
}

class Enemy
{
    int maxHp;
    int currHp;

    public Enemy(int _hp)
    {
        currHp = maxHp = _hp;
    }

    void TakeDamage(Bullet bullet)
    {
        if (currHp - bullet.dmg <= 0)
            Die();
        else
            currHp -= bullet.dmg;
    }

    void Die()
    {

    }

    
}

class Bullet
{
    public int dmg;
    public float speed;

    public Bullet(int _dmg, float _spd)
    {
        dmg = _dmg;
        speed = _spd;
    }
}