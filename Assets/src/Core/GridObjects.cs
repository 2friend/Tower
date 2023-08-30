using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using TMPro;
using System.Globalization;

public class GridObjects : MonoBehaviour
{
    private const string FOLDER_PATH = "Data";
    private const string ENEMY_FILE_PATH = "Enemy";
    private const string TOWER_FILE_PATH = "Tower";

    private const string TOWER_OBJECT_VAR = "tower";
    private const string TOWER_ID_ATTRIBUTE_VAR = "id";
    private const string TOWER_NAME_ATTRIBUTE_VAR = "name";

    private const string BULLET_OBJECT_VAR = "bullet";
    private const string BULLET_DAMAGE_ATTRIBUTE_VAR = "damage";
    private const string BULLET_SPEED_ATTRIBUTE_VAR = "speed";
    private const string BULLET_SPRITE_ATTRIBUTE_VAR = "sprite";

    private const string ENEMY_OBJECT_VAR = "enemy";
    private const string ENEMY_ID_ATTRIBUTE_VAR = "id";
    private const string ENEMY_NAME_ATTRIBUTE_VAR = "name";
    private const string ENEMY_HP_ATTRIBUTE_VAR = "hp";
    private const string ENEMY_SPEED_ATTRIBUTE_VAR = "speed";
    private const string ENEMY_SPRITE_ATTRIBUTE_VAR = "sprite"; 
    private const string ENEMY_MONEY_ATTRIBUTE_VAR = "money";

    private List<Tower> towers = new List<Tower>();
    private List<EnemyBD> enemys = new List<EnemyBD>();

    private void Start()
    {
        ReadEnemysFile();
        ReadTowersFile();
    }

    private void ReadEnemysFile()
    {
        TextAsset binary = Resources.Load<TextAsset>(FOLDER_PATH + "/" + ENEMY_FILE_PATH);
        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Core] [Enemys] Reading Enemys File: " + FOLDER_PATH + "/" + ENEMY_FILE_PATH + ".xml");
        enemys.Clear();

        while (reader.Read())
        {
            if (reader.IsStartElement(ENEMY_OBJECT_VAR)) // Build reading
            {
                int _enemyId = Convert.ToInt32(reader.GetAttribute(ENEMY_ID_ATTRIBUTE_VAR));
                string _enemyName = reader.GetAttribute(ENEMY_NAME_ATTRIBUTE_VAR);
                int _enemyHp = Convert.ToInt32(reader.GetAttribute(ENEMY_HP_ATTRIBUTE_VAR));
                float _enemySpeed = float.Parse(reader.GetAttribute(ENEMY_SPEED_ATTRIBUTE_VAR), CultureInfo.InvariantCulture);
                string _enemySprite = reader.GetAttribute(ENEMY_SPRITE_ATTRIBUTE_VAR);
                int _enemyMoney = Convert.ToInt32(reader.GetAttribute(ENEMY_MONEY_ATTRIBUTE_VAR));

                EnemyBD _enemy = new EnemyBD(_enemyId, _enemyName, _enemyHp, _enemySprite, _enemySpeed, _enemyMoney);

                Debug.Log("[Core] [Enemys] Loaded New Enemy: %" + _enemyName + "%");

                enemys.Add(_enemy);
            }
        }
        reader.Close();
        Debug.Log("[Core] [Enemys] Reading Finished, loaded: %" + enemys.Count + "%" + " enemys!");
    }

    private void ReadTowersFile()
    {
        TextAsset binary = Resources.Load<TextAsset>(FOLDER_PATH + "/" + TOWER_FILE_PATH);
        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Core] [Towers] Reading Towers File: " + FOLDER_PATH + "/" + TOWER_FILE_PATH + ".xml");
        towers.Clear();

        while (reader.Read())
        {

            if (reader.IsStartElement(TOWER_OBJECT_VAR)) // Build reading
            {
                int _towerId = Convert.ToInt32(reader.GetAttribute(TOWER_ID_ATTRIBUTE_VAR));
                string _towerName = reader.GetAttribute(TOWER_NAME_ATTRIBUTE_VAR);
                float _towerSpeed = float.Parse(reader.GetAttribute(ENEMY_SPEED_ATTRIBUTE_VAR), CultureInfo.InvariantCulture);
                string _towerSprite = reader.GetAttribute(ENEMY_SPRITE_ATTRIBUTE_VAR);

                Tower _tower = new Tower(_towerId, _towerName, _towerSprite, _towerSpeed);

                XmlReader inner = reader.ReadSubtree();
                while (reader.Read())
                {
                    if (inner.IsStartElement(BULLET_OBJECT_VAR))
                    {
                        int _bulletDamage = Convert.ToInt32(inner.GetAttribute(BULLET_DAMAGE_ATTRIBUTE_VAR));
                        float _bulletSpeed = float.Parse(reader.GetAttribute(BULLET_SPEED_ATTRIBUTE_VAR), CultureInfo.InvariantCulture);
                        string _bulletSprite = inner.GetAttribute(BULLET_SPRITE_ATTRIBUTE_VAR);

                        Bullet _bullet = new Bullet(_bulletDamage, _bulletSpeed, _bulletSprite);
                        _tower.bulletType = _bullet;
                    }
    
                }
                inner.Close();
                Debug.Log("[Core] [Towers] Loaded New Tower: %" + _towerName + "%");
                towers.Add(_tower);
            }
        }
        reader.Close();
        Debug.Log("[Core] [Towers] Reading Finished, loaded: %" + towers.Count + "%" + " towers!");
    }

    public void SpawnEnemy(EnemyBD _enemy)
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

class Tower 
{
    int id;
    string name;
    public Bullet bulletType;
    string sprite;
    float atkspd;

    public Tower(int _id, string _name, string _sprite, float _spd)
    {
        id = _id;
        name = _name;
        sprite = _sprite;
        atkspd = _spd;
    }

    void Attack()
    {
        
    }
}

public class EnemyBD
{
    int id;
    string name;
    int maxHp;
    int currHp;
    string sprite;
    float speed;
    int money;

    public EnemyBD(int _id, string _name, int _hp, string _spr, float _spd, int _money)
    {
        id = _id;
        name = _name;
        currHp = maxHp = _hp;
        sprite = _spr;
        speed = _spd;
        money = _money;
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
    float speed;
    string sprite;

    public Bullet(int _dmg, float _spd, string _sprite)
    {
        dmg = _dmg;
        speed = _spd;
        sprite = _sprite;
    }
}