using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public int money;
    public int wave;

    [SerializeField] private TextMeshProUGUI waveText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        waveText.text = wave.ToString();
    }
}
