using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstant : MonoBehaviour
{
    public static GridObjects _gridObjects;
    public static PathController _pathController;
    public static WaveController _waveController;
    public static GridController _gridController;

    private void Awake()
    {
        _gridObjects = GameObject.Find("GridObjects").GetComponent<GridObjects>();
        _pathController = GameObject.Find("PathController").GetComponent<PathController>();
        _waveController = GameObject.Find("WaveController").GetComponent<WaveController>();
        _gridController = GameObject.Find("Grid").GetComponent<GridController>();
    }
}
