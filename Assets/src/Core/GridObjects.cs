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
    private List<Tower> towers = new List<Tower>();
    public List<EnemyBD> enemys = new List<EnemyBD>();
    private CameraMover cameraMover;

    [SerializeField] private GridController grid;
    [SerializeField] private WaveController waveController;
    [SerializeField] private EffectHolder effectHolder;

    [SerializeField] private List<GameObject> shopsObjects = new List<GameObject>();
    [SerializeField] private Transform shopParent;
    [SerializeField] private GameObject shopObject;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject tower;

    public GameObject currentBuilding;

    public bool isBuilding = false;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraMover = GameObject.Find("CameraController").GetComponent<CameraMover>();
        ReadEnemysFile();
        ReadTowersFile();
    }

    private void Update()
    {

        if(isBuilding)
            cameraMover.canMove = false;
        else
            cameraMover.canMove = true;

        if (Input.GetMouseButtonDown(0) && isBuilding)
        {
            Vector3 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

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
            Vector3 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Tower>())
            {
                Debug.Log("Clicked on tower!");
            }
        }
    }

    public IEnumerator Spawn(EnemyBD _enemy, int count)
    {
        Debug.Log("[Core] [Spawning] Start Spawning enemy: %" + _enemy.enemyName + "%. Count: %" + count + "%");
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

    private void ReadEnemysFile()
    {
        TextAsset binary = Resources.Load<TextAsset>(FOLDER_PATH + "/" + ENEMY_FILE_PATH);
        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Core] [Enemys] Reading Enemys File: " + FOLDER_PATH + "/" + ENEMY_FILE_PATH + ".xml");

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
                float _attackRange = float.Parse(reader.GetAttribute(TOWER_ATTACK_RANGE), CultureInfo.InvariantCulture);
                Tower _tower = new Tower(_towerId, _towerName, _towerSprite, _towerSpeed, _attackRange);

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
                Debug.Log("[Core] [Towers] Loaded New Tower: %" + _towerName + "%");
                towers.Add(_tower);
                AddToShop(_tower);
                Debug.Log("[Core] [UI] Added new tower button: %" + _towerName + "%");
            }
        }
        reader.Close();
        Debug.Log("[Core] [Towers] Reading Finished, loaded: %" + towers.Count + "%" + " towers!");
    }

    private void AddToShop(Tower _type)
    {
        UnityAction<Tower> clickAction = (_tower) =>
        {
            OnShopButtonClick(_tower);
        };

        GameObject shopButtonNew = Instantiate(shopObject, shopParent, false);
        Tower shopButtonType = shopButtonNew.AddComponent<Tower>();

        shopButtonType.InitializeFrom(_type);
        shopButtonNew.name = shopButtonType.towerName;

        shopButtonNew.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites\\UI\\Shop\\Shop");
        shopButtonNew.GetComponent<Button>().onClick.AddListener(() => clickAction.Invoke(_type)); ;
    }

    public void OnShopButtonClick(Tower _tower)
    {
        currentBuilding = Instantiate(tower);
        currentBuilding.GetComponent<SpriteRenderer>().sortingOrder = 2;
        isBuilding = true;
        Tower towerType = currentBuilding.AddComponent<Tower>();
        towerType.InitializeFrom(_tower);
    }
}

public class Tower : MonoBehaviour
{
    public int id;
    public string towerName;
    public Bullet bulletType;
    public string sprite;
    public float atkspd;
    public float attackRange;
    private float atkspdTimer = 0;

    public Tower(int _id, string _name, string _sprite, float _spd, float _attackRange)
    {
        id = _id;
        towerName = _name;
        sprite = _sprite;
        atkspd = _spd;
        attackRange = _attackRange;
    }

    void Update()
    {
        atkspdTimer += Time.deltaTime; 
        if (atkspdTimer >= 1 / atkspd) 
        {
            EnemyBD targetEnemy = FindNearestEnemy();

            if (targetEnemy != null)
            {
                Attack(targetEnemy);

                atkspdTimer = 0; 
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
                Shoot(enemy);
            }
        }
    }

    public void Shoot(EnemyBD enemy)
    {
        Bullet bullet = bulletType;
        GameObject bulletObject = new GameObject("Bullet");
        BulletComponent bulletComponent = bulletObject.AddComponent<BulletComponent>();
        bulletObject.AddComponent<SpriteRenderer>();
        bulletComponent.InitializeFrom(bullet);
        bulletObject.transform.position = transform.position;
        bulletComponent.SetTarget(enemy.transform.position);
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
    private Vector3 targetPosition;
    public SpriteRenderer projectile;
    private string sprite;

    public void InitializeFrom(Bullet bullet)
    {
        damage = bullet.dmg;
        speed = bullet.speed;
        sprite = bullet.sprite;
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }
    void Start ()
    {
        projectile = GetComponent<SpriteRenderer>();
        if (projectile != null)
        {
            projectile.sprite = Resources.Load<Sprite>(BULLET_SPRITE_PATH + sprite);
        }
        else
        {
            Debug.LogError("SpriteRenderer is missing on the Bullet object!");
        }
    }
    void Update()
    {
        Vector3 direction = targetPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction.normalized);

        transform.rotation = rotation;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            HitEnemy();
            Destroy(gameObject);
        }
    }

    void HitEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (var collider in colliders)
        {
            EnemyBD enemy = collider.GetComponent<EnemyBD>();
            if (enemy != null)
            {
                enemy.TakingDamage(damage);
            }
        }
    }
}

public class EnemyBD : MonoBehaviour
{
    private const string ANIMATORS_PATH = "Animations\\Enemys\\";

    public int id;
    public string enemyName;
    public int maxHp;
    public int currHp;
    private bool isDead = false;
    public string sprite;
    public float speed;
    public int money;
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
        grid = GameObject.Find("Grid").GetComponent<GridController>();
        waveController = GameObject.Find("MainCamera").GetComponent<WaveController>();
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
            else
            {
                waveController.aliveEnemys--;
                    waveController.enemysToKill--;
                Destroy(this.gameObject);
            }
        }
        }
        else
        {
            waveController.aliveEnemys--;
            waveController.enemysToKill--;
            player.TakingDamage(1); // TO DO: ������� ������� ����� ���� �������� ��� �� ����� �����
            Debug.Log("[Gameplay] [Enemys] Enemy: %" + enemyName + "% Finished Way And Be Destroyed!");
            Destroy(this.gameObject);
        }
    }

    public void TakingDamage(int _dmg)
    {
        if (currHp - _dmg > 0)
        {
            Debug.Log("[Gameplay] [Enemy] Enemy: %" + enemyName + "% Taking Damage: %" + (currHp - _dmg).ToString() + "%");
            currHp -= _dmg;
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("[Gameplay] [Enemy] Enemy: %" + enemyName + "% Die!");
        isDead = true;
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    void InitializeEnemyType()
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
                Debug.LogError("[!] [Core] [Enemys] NO SUCH ENEMY: %" + enemyName + "% ANIMATION CONTROLLER FOUND!");
            }
        }
        else
        {
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