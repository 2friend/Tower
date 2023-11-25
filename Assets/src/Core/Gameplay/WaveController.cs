using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using TMPro;

public class WaveController : MonoBehaviour
{
    private const string FOLDER_PATH = "Data";
    private const string WAVES_FILE_PATH = "Waves";

    private const string WAVE_OBJECT_VAR = "wave";
    private const string WAVE_ID_ATTRIBUTE_VAR = "id";

    private const string ENEMY_OBJECT_VAR = "enemy";
    private const string ENEMY_TYPE_ATTRIBUTE_VAR = "enemy_type";
    private const string ENEMY_COUNT_ATTRIBUTE_VAR = "count";

    [SerializeField] private GridObjects gridObjects;

    [SerializeField] private TextMeshProUGUI waveCounter;
    [SerializeField] private PlayerTower player;

    public List<Wave> waves = new List<Wave>();
    public bool waveStarted;
    [SerializeField] private int activeWaveId = -1;
    public int aliveEnemys = 0;
    public int enemysToKill = 0;
    public Wave activeWave;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerTower>();
    }

    private void Update()
    {
        if (waveStarted)
        {
            activeWave = waves[activeWaveId];

            if (!activeWave.startedWave && activeWave.currentEnemysIndex < activeWave.enemysPerWave.Count && player.livesCurrent > 0)
            {
                activeWave.startedWave = true;
                var enemyDict = activeWave.enemysPerWave[activeWave.currentEnemysIndex];
                foreach (var kvp in enemyDict)
                {
                    EnemyBD enemy = kvp.Key;
                    int count = kvp.Value;
                    StartCoroutine(SpawnEnemyWithDelay(enemy, count));
                }
            }
            else if (activeWave.currentEnemysIndex >= activeWave.enemysPerWave.Count && player.livesCurrent > 0)
            {
                if (aliveEnemys == 0 && activeWaveId + 1 <= waves.Count - 1) 
                {
                    waveStarted = false;
                    StartCoroutine(StartWaves());
                }
                else if (aliveEnemys == 0 && activeWaveId + 1 > waves.Count - 1)
                {
                    Debug.Log("GAME ENDED");
                }
            }
        }
    }

    public void ReadWavesFile()
    {
        TextAsset binary = Resources.Load<TextAsset>(FOLDER_PATH + "/" + WAVES_FILE_PATH);
        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Loading] [Waves] Reading Waves File: " + FOLDER_PATH + "/" + WAVES_FILE_PATH + ".xml");
        waves.Clear();

        while (reader.Read())
        {
            if (reader.IsStartElement(WAVE_OBJECT_VAR))
            {
                int _waveId = Convert.ToInt32(reader.GetAttribute(WAVE_ID_ATTRIBUTE_VAR));
                Wave _wave = new Wave(_waveId);

                _wave.enemysPerWave = new Dictionary<int, Dictionary<EnemyBD, int>>();

                XmlReader inner = reader.ReadSubtree();
                int index = 0;
                while (inner.Read())
                {
                    if (inner.IsStartElement(ENEMY_OBJECT_VAR))
                    {
                        string _enemyType = inner.GetAttribute(ENEMY_TYPE_ATTRIBUTE_VAR);
                        int _enemyCount = Convert.ToInt32(inner.GetAttribute(ENEMY_COUNT_ATTRIBUTE_VAR));
                        
                        foreach (EnemyBD _enemy in gridObjects.enemys)
                        {
                            if (_enemy.enemyName == _enemyType)
                            {
                                _wave.enemysPerWave.Add(index, new Dictionary<EnemyBD, int>());
                                _wave.enemysPerWave[index].Add(_enemy, _enemyCount);
                                _wave.waveEnemysToKill += _enemyCount;
                                
                                Debug.Log("[Loading] [Waves] Added new enemy: %" + _enemy.enemyName + "% to Wave: %" + _wave.id + "%");
                                index++;
                            }
                        }
                    }
                }
                inner.Close();

                Debug.Log("[Loading] [Waves] Loaded New Wave: %" + _wave.id + "%. With enemys count: %" + _wave.enemysPerWave.Count + "%");
                waves.Add(_wave);
            }
        }
        reader.Close();
        Debug.Log("[Loading] [Waves] Reading Finished, loaded: %" + waves.Count + "%" + " waves!");
    }

    public IEnumerator StartWaves()
    {
        for (int i = 3; i >= 0; i--)
        {
            waveCounter.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        waveCounter.text = "";
        if (activeWaveId+1<= waves.Count - 1)
        {
            activeWaveId++;
            waveStarted = true;
            enemysToKill = waves[activeWaveId].waveEnemysToKill;
            Debug.Log("[Core] [Waves] Started Wave: %" + activeWaveId + "%");
        }
        else
        {
            Debug.LogWarning("[?] [Core] [Waves] The waves is ended, no more waves left!");
        }
        
    }

    private IEnumerator SpawnEnemyWithDelay(EnemyBD enemy, int count)
    {
        activeWave.currentEnemysIndex++;
        for (int i = 0; i < 1; i++)
        {
            StartCoroutine(gridObjects.Spawn(enemy, count));

            yield return new WaitForSeconds(.3f);
        }
        
    }

    public void ForceStopWaves()
    {
        StopAllCoroutines();
    }
}

public class Wave
{
    public int id;
    public Dictionary<int, Dictionary<EnemyBD, int>> enemysPerWave = new Dictionary<int, Dictionary<EnemyBD, int>>();
    public int currentEnemysIndex = 0;
    public int waveEnemysToKill;
    public bool startedWave;

    public Wave(int _id)
    {
        id = _id;
    }
}
