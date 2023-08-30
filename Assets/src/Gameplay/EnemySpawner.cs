using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemy;

    private void Update()
    {
        if (Input.GetKeyDown("f"))
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i <=10; i++)
        {
            Instantiate(enemy[Random.RandomRange(0, enemy.Count)]);
            yield return new WaitForSeconds(3.1f);
        }
    }
}
