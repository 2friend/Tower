using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public int money;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadProgress()
    {

    }

    public void SaveProgress()
    {

    }
}
