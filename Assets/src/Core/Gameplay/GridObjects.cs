using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEditor.Animations;
using TMPro;
using System.Globalization;
using UnityEngine.Events;
using UnityEngine.UI;


public class GridObjects : MonoBehaviour
{
    private const string FOLDER_PATH = "Data";
    private const string ENEMY_FILE_PATH = "Enemy";
    private const string TOWER_FILE_PATH = "Tower";
    private const string MAGIC_FILE_PATH = "Magic";

    private const string MAGIC_OBJECT_VAR = "magic";
    private const string MAGIC_NAME_ATT = "name";
    private const string MAGIC_CLASS_ATT = "class";
    private const string MAGIC_GLOBAL_ATT = "isGlobal";
    private const string MAGIC_EFFECT_ATT = "effect";
    private const string MAGIC_SOUND_ATT = "sound";
    private const string MAGIC_TYPE_ATT = "type";
    private const string MAGIC_TYPEVALUE_ATT = "typeValue";

    private const string TOWER_OBJECT_VAR = "tower";
    private const string TOWER_ID_ATTRIBUTE_VAR = "id";
    private const string TOWER_NAME_ATTRIBUTE_VAR = "name";

    private const string TOWER_ATTACK_RANGE = "attackRange";
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

    private Camera mainCamera;

    public List<Tower> towers = new List<Tower>();
    public static List<EnemyBD> enemys = new List<EnemyBD>();
    public List<Magic> magics = new List<Magic>();

    public List<GameObject> allObjectOnGrid = new List<GameObject>();

    private CameraMover cameraMover;

    [SerializeField] private GridController grid;
    [SerializeField] private WaveController waveController;
    [SerializeField] private EffectHolder effectHolder;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject tower;

    private PlayerTower player;

    public GameObject currentBuilding;

    public bool isBuilding = false;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraMover = GameObject.Find("CameraController").GetComponent<CameraMover>();
        grid = GameConstant.gridController;
    }

    private void Update()
    {

        if (player == null)
        {
            player = grid.player.GetComponent<PlayerTower>();
        }

        if(isBuilding)
            cameraMover.canMove = false;
        else
            cameraMover.canMove = true;

        Vector3 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

        if (Input.GetMouseButtonDown(0) && isBuilding)
        {
            if (hit.collider != null && !hit.collider.GetComponent<Node>().haveSomething)
            {
                isBuilding = false;
                hit.collider.GetComponent<Node>().haveSomething = true;

                Debug.Log("[Gameplay] [Building] Builded tower: %" + currentBuilding.GetComponent<Tower>().id + "%. On Node X: %" + hit.collider.GetComponent<Node>().x + "% Y: %" + hit.collider.GetComponent<Node>().y + "%");

                foreach (Node _node in grid.GetNodeNeighbors(hit.collider.GetComponent<Node>()))
                {
                    _node.haveSomething = true;
                }

                currentBuilding.GetComponent<SpriteRenderer>().color = Color.white;
                currentBuilding.GetComponent<Animator>().SetBool("Placed", true);
                currentBuilding = null;
            }
           
        }
        else if (Input.GetMouseButtonDown(0) && !isBuilding)
        {
            if (hit.collider != null && hit.collider.GetComponent<Tower>())
            {
                Debug.Log("Clicked on tower!");
            }
            else if (hit.collider != null && hit.collider.GetComponent<EnemyBD>())
            {
                Debug.Log("Clicked on enemy!");
            }
        }
    }

    public IEnumerator Spawn(EnemyBD _enemy, int count)
    {
        Debug.Log("[Gameplay] [Spawning] Start Spawning enemy: %" + _enemy.enemyName + "%. Count: %" + count + "%");
        for (int i = 0; i < count; i++)
        {
            GameObject enemyObj = Instantiate(enemy, grid.waypoints[0]);
            EnemyBD enemyComp = enemyObj.AddComponent<EnemyBD>();
            enemyComp.InitializeFrom(_enemy);
            waveController.aliveEnemys++;
            yield return new WaitForSeconds(1.1f);
        }
        waveController.activeWave.startedWave = false;
    }

    public void ReadEnemysFile()
    {
        TextAsset binary = Resources.Load<TextAsset>(FOLDER_PATH + "/" + ENEMY_FILE_PATH);

        if (binary == null)
        {
            Debug.LogError("[!] [Loading] [Enemys] No Such File: %" + FOLDER_PATH + "/" + ENEMY_FILE_PATH + "% To Read!");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Loading] [Enemys] Reading Enemys File: " + FOLDER_PATH + "/" + ENEMY_FILE_PATH + ".xml");

        while (reader.Read())
        {
            if (reader.IsStartElement(ENEMY_OBJECT_VAR)) // Build reading
            {
                int _enemyId = Convert.ToInt32(reader.GetAttribute(ENEMY_ID_ATTRIBUTE_VAR));
                string _enemyName = reader.GetAttribute(ENEMY_NAME_ATTRIBUTE_VAR);
                int _enemyHp = Convert.ToInt32(reader.GetAttribute(ENEMY_HP_ATTRIBUTE_VAR));
                float _enemySpeed = Convert.ToSingle(reader.GetAttribute(ENEMY_SPEED_ATTRIBUTE_VAR), CultureInfo.InvariantCulture);
                string _enemySprite = reader.GetAttribute(ENEMY_SPRITE_ATTRIBUTE_VAR);
                int _enemyMoney = Convert.ToInt32(reader.GetAttribute(ENEMY_MONEY_ATTRIBUTE_VAR));

                EnemyBD _enemy = new EnemyBD(_enemyId, _enemyName, _enemyHp, _enemySprite, _enemySpeed, _enemyMoney);
                enemys.Add(_enemy);
                Debug.Log("[Loading] [Enemys] Loaded New Enemy: %" + _enemyName + "%");
            }
        }
        reader.Close();
        Debug.Log("[Loading] [Enemys] Reading Finished, loaded: %" + enemys.Count + "%" + " enemys!");
    }

    public void ReadTowersFile()
    {
        TextAsset binary = Resources.Load<TextAsset>(FOLDER_PATH + "/" + TOWER_FILE_PATH);

        if(binary==null)
        {
            Debug.LogError("[!] [Loading] [Towers] No Such File: %" + FOLDER_PATH + "/" + TOWER_FILE_PATH + "% To Read!");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Loading] [Towers] Reading Towers File: " + FOLDER_PATH + "/" + TOWER_FILE_PATH + ".xml");
        towers.Clear();

        while (reader.Read())
        {

            if (reader.IsStartElement(TOWER_OBJECT_VAR)) // Build reading
            {
                int _towerId = Convert.ToInt32(reader.GetAttribute(TOWER_ID_ATTRIBUTE_VAR));
                string _towerName = reader.GetAttribute(TOWER_NAME_ATTRIBUTE_VAR);
                float _towerSpeed = float.Parse(reader.GetAttribute(ENEMY_SPEED_ATTRIBUTE_VAR), CultureInfo.InvariantCulture);
                string _towerSprite = reader.GetAttribute(ENEMY_SPRITE_ATTRIBUTE_VAR);
                float _attackRange = float.Parse(reader.GetAttribute(TOWER_ATTACK_RANGE), CultureInfo.InvariantCulture);
                Tower _tower = CreateTower(_towerId, _towerName, _towerSprite, _towerSpeed, _attackRange);

                XmlReader inner = reader.ReadSubtree();
                while (inner.Read())
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
                Debug.Log("[Loading] [Towers] Loaded New Tower: %" + _towerName + "%");
                towers.Add(_tower);
            }
        }
        reader.Close();
        Debug.Log("[Loading] [Towers] Reading Finished, loaded: %" + towers.Count + "%" + " towers!");
    }

    public void ReadMagicsFile()
    {
        TextAsset binary = Resources.Load<TextAsset>(FOLDER_PATH + "/" + MAGIC_FILE_PATH);

        if (binary == null)
        {
            Debug.LogError("[!] [Loading] [Magics] No Such File: %" + FOLDER_PATH + "/" + MAGIC_FILE_PATH + "% To Read!");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Loading] [Magics] Reading Towers File: " + FOLDER_PATH + "/" + MAGIC_FILE_PATH + ".xml");
        towers.Clear();
        int index = 0;
        while (reader.Read())
        {
            index++;

            if (reader.IsStartElement(TOWER_OBJECT_VAR)) // Build reading
            {
                string _magicId = reader.GetAttribute(TOWER_ID_ATTRIBUTE_VAR);
                if (_magicId == "")
                    Debug.LogError("[!] [Loading] [Magic] Magic With Index: %" + index.ToString() + "% Have No ID!");

                string _magicName = reader.GetAttribute(TOWER_NAME_ATTRIBUTE_VAR);
                if (_magicName == "")
                    Debug.LogError("[!] [Loading] [Magic] Magic With Id: %" + _magicId + "% Have No Name!");
                string _magicClass = reader.GetAttribute(ENEMY_SPEED_ATTRIBUTE_VAR);
                bool _magicIsGlobal = Convert.ToBoolean(reader.GetAttribute(ENEMY_SPRITE_ATTRIBUTE_VAR));
                string _magicEffect = reader.GetAttribute(ENEMY_SPRITE_ATTRIBUTE_VAR);
                string _magicSound = reader.GetAttribute(ENEMY_SPRITE_ATTRIBUTE_VAR);
                string _magicType = reader.GetAttribute(ENEMY_SPRITE_ATTRIBUTE_VAR);
                int _magicTypeValue = Convert.ToInt32(reader.GetAttribute(ENEMY_SPRITE_ATTRIBUTE_VAR));
                Magic _magic = new Magic(_magicId, _magicName, _magicClass, _magicIsGlobal, _magicEffect, _magicSound, _magicType, _magicTypeValue);
                Debug.Log("[Loading] [Magics] Loaded New Magic: %" + _magicId + "%");
                magics.Add(_magic);
            }
        }
        reader.Close();
        Debug.Log("[Loading] [Magics] Reading Finished, loaded: %" + towers.Count + "%" + " towers!");
    }

    public EnemyBD GetEnemyByID(string _name)
    {
        EnemyBD _enemy = null;

        foreach (EnemyBD item in enemys)
        {
            if (item.enemyName == _name)
            {
                _enemy = item;
            }
        }

        if (_enemy == null)
            Debug.LogError("[!] [Loading] [Enemies] No Such Enemy Found: %" + _name + "%!");

        return _enemy;
    }

    public Tower GetTowerByID(string _name)
    {
        Tower _tower = null;

        foreach (Tower item in towers)
        {
            if (item.towerName == _name)
                _tower = item;
        }

        if (_tower == null)
            Debug.LogError("[!] [Loading] [Towers] No Such Tower Found: %" + _name + "%!");

        return _tower;
    }

    public Magic GetMagicByID(string _name)
    {
        Magic _magic = null;

        foreach (Magic item in magics)
        {
            if (item.Name == _name)
                _magic = item;
        }

        if (_magic == null)
            Debug.LogError($"[!] [Loading] [Magic] No Such Magic Found: %{_name}%!");

        return _magic;
    }

    private Tower CreateTower(int _id, string _name, string _sprite, float _spd, float _attackRange)
    {
        return new Tower(_id, _name, _sprite, _spd, _attackRange);
    }

    private EnemyBD CreateEnemy(int _id, string _name, int _hp, string _spr, float _spd, int _money)
    {
        return new EnemyBD(_id, _name, _hp, _spr, _spd, _money);
    }
}

public class Tower : MonoBehaviour, ICardType
{
    public int id;
    public string towerName;
    public Bullet bulletType;
    public string sprite;
    public float atkspd;
    public float attackRange;
    public int moneyCost; // TODO: Parsing moneyCost att in xml
    private float atkspdTimer = 0;

    public Tower(int _id, string _name, string _sprite, float _spd, float _attackRange)
    {
        id = _id;
        towerName = _name;
        sprite = _sprite;
        atkspd = _spd;
        attackRange = _attackRange;
        moneyCost = 25;
    }

    void Update()
    {
        atkspdTimer += Time.deltaTime; 
        if (atkspdTimer >= 1 / atkspd) 
        {
            EnemyBD targetEnemy = FindNearestEnemy();

            if (targetEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);

                if (distance <= attackRange)
                {
                    Attack(targetEnemy);
                    atkspdTimer = 0; 
                }
            }
        }
    }


    private EnemyBD FindNearestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        EnemyBD nearestEnemy = null;

        foreach (EnemyBD enemy in FindObjectsOfType<EnemyBD>())
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    public void Attack(EnemyBD enemy)
    {
        if (enemy != null)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= attackRange)
            {
                if(gameObject.GetComponent<TowerAnimatorController>().unit.activeSelf) 
                {
                     Shoot(enemy);
                }
            }
            else
            {
                EnemyBD newTarget = FindNearestEnemy();
                if (newTarget != null)
                {
                    float newDistance = Vector3.Distance(transform.position, newTarget.transform.position);
                    if (newDistance <= attackRange)
                    {
                        Attack(newTarget); 
                    }
                }
            }
        }
    }

    public void Shoot(EnemyBD enemy)
    {
        gameObject.GetComponent<TowerAnimatorController>().unit.GetComponent<UnitAnimationController>().Attack();

        Bullet bullet = bulletType;
        GameObject bulletObject = new GameObject("Bullet");
        BulletComponent bulletComponent = bulletObject.AddComponent<BulletComponent>();

        bulletObject.AddComponent<SpriteRenderer>();
        bulletObject.GetComponent<SpriteRenderer>().sortingOrder = 4;

        bulletComponent.InitializeFrom(bullet);
        bulletObject.transform.position = transform.position;

        bulletComponent.SetTarget(enemy.gameObject);
    }

    public void InitializeFrom(Tower other)
    {
        id = other.id;
        towerName = other.towerName;
        bulletType = other.bulletType;
        sprite = other.sprite;
        atkspd = other.atkspd;
        attackRange = other.attackRange;
    }
}


public class BulletComponent : MonoBehaviour
{
    private const string BULLET_SPRITE_PATH = "Sprites\\Bullets\\";
    private int damage;
    private float speed;
    private GameObject targetPosition;
    public SpriteRenderer projectile;
    private string sprite;

    public void InitializeFrom(Bullet bullet)
    {
        damage = bullet.dmg;
        speed = bullet.speed;
        sprite = bullet.sprite;
    }

    public void SetTarget(GameObject target)
    {
        targetPosition = target;
    }

    private void Start ()
    {
        projectile = GetComponent<SpriteRenderer>();
        if (projectile != null)
        {
            projectile.sprite = Resources.Load<Sprite>(BULLET_SPRITE_PATH + sprite);
        }
        else
        {
            Debug.LogError("[!] [Gameplay] [Bullet] SpriteRenderer is missing on the Bullet object!");
        }
    }

    private void Update()
    {
        Vector3 direction = targetPosition.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction.normalized);

        transform.rotation = rotation;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition.transform.position) < 0.1f && !targetPosition.GetComponent<EnemyBD>().isDead )
        {
            HitEnemy();
            Destroy(gameObject);
        } 
        else if (Vector3.Distance(transform.position, targetPosition.transform.position) < 0.1f && targetPosition.GetComponent<EnemyBD>().isDead || 
        Vector3.Distance(transform.position, targetPosition.transform.position) >= 0.1f && targetPosition.GetComponent<EnemyBD>().isDead
        ) 
        {
            Destroy(gameObject);
        }
    }

    private void HitEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (var collider in colliders)
        {
            EnemyBD enemy = collider.GetComponent<EnemyBD>();
            if (enemy != null && !enemy.isDead)
            {
                enemy.TakingDamage(damage);
            }
        }
        Destroy(gameObject);

    }
}

public class EnemyBD : MonoBehaviour, ICardType
{
    private const string ANIMATORS_PATH = "Animations\\Enemys\\";

    public int id;
    public string enemyName;
    public int maxHp;
    public int currHp;
    public bool isDead = false;
    public string sprite;
    public float speed;
    public int money;
    public int playerDamage; // TODO: Parsing playerDamage att in xml
    public bool isEnemy;

    private int currentWaypointIndex = 0;

    private Animator animator;
    private PlayerTower player;
    public GridController grid;
    public WaveController waveController;

    public EnemyBD(int _id, string _name, int _hp, string _spr, float _spd, int _money)
    {
        id = _id;
        enemyName = _name;
        currHp = maxHp = _hp;
        sprite = _spr;
        speed = _spd;
        money = _money;
        playerDamage = 1; // TODO: Parsing playerDamage att in xml
    }

    public void InitializeFrom(EnemyBD other)
    {
        id = other.id;
        enemyName = other.enemyName;
        currHp = other.currHp;
        maxHp = other.maxHp;
        sprite = other.sprite;
        speed = other.speed;
        money = other.money;
    }

    private void Start()
    {
        grid = GameConstant.gridController;
        waveController = GameConstant.waveController;
        player = GameObject.Find("Player").GetComponent<PlayerTower>();
        animator = GetComponent<Animator>();
        InitializeEnemyType();
    }

    private void Update()
    {
        if (currentWaypointIndex < grid.waypoints.Count)
        {
            if (!isDead)
            {
                Transform targetWaypoint = grid.waypoints[currentWaypointIndex];
                if (currentWaypointIndex < grid.waypoints.Count)
                {

                Vector3 moveDirection = (targetWaypoint.position - transform.position).normalized;

                animator.SetFloat("X", moveDirection.x);
                animator.SetFloat("Y", moveDirection.y);

                transform.Translate(moveDirection * speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
                {
                    currentWaypointIndex++;
                }
            }
        }
        }
        else
        {
            waveController.aliveEnemys--;
            waveController.enemysToKill--;

            player.TakingDamage(playerDamage);

            Debug.Log("[Gameplay] [Enemys] Enemy: %" + enemyName + "% Finished Way And Be Destroyed!");

            Destroy(this.gameObject);
        }
    }

    public void TakingDamage(int _value)
    {
        if (!isDead)
        {
            if (currHp - _value > 0)
            {
                Debug.Log("[Gameplay] [Enemy] Enemy: %" + enemyName + "% Taking Damage: %" + (currHp - _value).ToString() + "%");
                currHp -= _value;
            }
            else if (currHp - _value <= 0)
            {
                Die();
            }
        }
    }

    public void TakingHeal(int _value)
    {
        if (!isDead)
        {
            if (currHp + _value <= maxHp)
            {
                Debug.Log("[Gameplay] [Enemy] Enemy: %" + enemyName + "% Taking Heal: %" + (currHp + _value).ToString() + "%");
                currHp += _value;
            }
            else if (currHp + _value > maxHp)
            {
                currHp = maxHp;
            }
        }
    }

    public void BuffHp(int _value)
    {
        if (!isDead)
        {
            currHp += _value;
            maxHp += _value;

            Debug.Log("[Gameplay] [Enemy] Enemy: %" + enemyName + "% Taking HP Buff: %" + _value.ToString() + "%");
        }
    }

    private void Die()
    {
        if (!isDead)
        {
            player.money += money;

            isDead = true;

            animator.SetTrigger("Die");

            Debug.Log("[Gameplay] [Enemy] Enemy: %" + enemyName + "% Die!");

            waveController.aliveEnemys -= 1;
            waveController.enemysToKill -= 1;
        }
    }

    private void InitializeEnemyType()
    {
        if (animator != null)
        {
            RuntimeAnimatorController newController = Resources.Load<RuntimeAnimatorController>(ANIMATORS_PATH + enemyName + "\\" + "E_" + enemyName);
            if (newController != null)
            {
                animator.runtimeAnimatorController = newController;
            }
            else
            {
                Debug.LogError("[!] [Gameplay] [Enemys] NO SUCH ENEMY: %" + enemyName + "% ANIMATION CONTROLLER FOUND!");
                return;
            }
        }
        else
        {
            Debug.LogError("[!] [Gameplay] [Enemys] ANIMATOR IN ENEMY: %" + enemyName + "% NOT FOUND!");
        }
    }
}

public class Bullet
{
    public int dmg;
    public float speed;
    public string sprite;

    public Bullet(int _dmg, float _spd, string _sprite)
    {
        dmg = _dmg;
        speed = _spd;
        sprite = _sprite;
    }
}

public class Magic : MonoBehaviour, ICardType
{
    public string id;
    public string Name;
    public string clas;
    public bool isGlobal;
    // TODO: Effect as GameObject
    public string effect;
    //TODO: Sound as AudioClip
    public string sound;
    public string typeMagic;
    public int typeValue;

    public Magic(string _id, string _name, string _class, bool _global, string _effect, string _sound, string _type, int _value)
    {
        id = _id;
        Name = _name;
        clas = _class;
        isGlobal = _global;
        effect = _effect;
        sound = _sound;
        typeMagic = _type;
        typeValue = _value;
    }

    public void UseMagic()
    {
        if (isGlobal)
            switch(typeMagic)
            {
                case "heal_all":
                    Magic_HealAll();
                    break;
                case "damage_all":
                    Magic_DamageAll();
                    break;
                case "damage_enemies":
                    Magic_DamageAllEnemies();
                    break;
                case "buff_hp":
                    Magic_BuffHpAll();
                    break;
            }    
    }

    private GameObject ChooseTarget()
    {
        GameObject _target = null;

        return _target;
    }

    private void Magic_HealTarget()
    {
        GameObject _target = ChooseTarget();

        EnemyBD _enemyComponent = _target.GetComponent<EnemyBD>();

        if (_target == null)
            return;

        if (_enemyComponent != null)
            _enemyComponent.TakingHeal(typeValue);
        else
            Debug.LogError("[!] [Gameplay] [Magic] Cant Heal Object Because It Dont Have Type");
    }

    private void Magic_DamageTarget()
    {
        GameObject _target = ChooseTarget();

        EnemyBD _enemyComponent = _target.GetComponent<EnemyBD>();

        if (_target == null)
            return;

        if (_enemyComponent != null)
            _enemyComponent.TakingDamage(typeValue);
        else
            Debug.LogError("[!] [Gameplay] [Magic] Cant Damage Object Because It Dont Have Type");
    }

    private void Magic_DamageAllEnemies()
    {
        foreach (GameObject item in GameConstant.gridObjects.allObjectOnGrid)
        {
            if (item.GetComponent<EnemyBD>() != null && item.GetComponent<EnemyBD>().isEnemy)
            {
                item.GetComponent<EnemyBD>().TakingDamage(typeValue);
            }
        }
    }

    private void Magic_DamageAll()
    {
        foreach (GameObject item in GameConstant.gridObjects.allObjectOnGrid)
        {
            if (item.GetComponent<EnemyBD>() != null)
            {
                item.GetComponent<EnemyBD>().TakingDamage(typeValue);
            }
        }
    }

    private void Magic_HealAll()
    {
        foreach (GameObject item in GameConstant.gridObjects.allObjectOnGrid)
        {
            if (item.GetComponent<EnemyBD>() != null)
            {
                item.GetComponent<EnemyBD>().TakingHeal(typeValue);
            }
        }
    }

    private void Magic_BuffHpAll()
    {
        foreach (GameObject item in GameConstant.gridObjects.allObjectOnGrid)
        {
            if (item.GetComponent<EnemyBD>() != null && !item.GetComponent<EnemyBD>().isEnemy)
            {
                item.GetComponent<EnemyBD>().BuffHp(typeValue);
            }
        }
    }

}