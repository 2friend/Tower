using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstant : MonoBehaviour
{
    public static ResourcesLoader resourcesLoader;
    public static GridObjects gridObjects;
    public static PathController pathController;
    public static WaveController waveController;
    public static GridController gridController;
    public static Tutorials tutorials;
    public static HeroesManager heroesManager;
    public static CardManager cardManager;
    public static DeckManager deckManager;
    public static CameraMover cameraMover;
    public static CardsGameManager cardsGameManager;

    public static bool extendedLogs = true;

    public void AssignValues()
    {
        resourcesLoader = GameObject.Find("Resources").GetComponent<ResourcesLoader>();
        gridController = GameObject.Find("Grid").GetComponent<GridController>();
        gridObjects = GameObject.Find("GridObjects").GetComponent<GridObjects>();
        pathController = GameObject.Find("PathController").GetComponent<PathController>();
        waveController = GameObject.Find("WaveController").GetComponent<WaveController>();
        cardManager = GameObject.Find("Cards").GetComponent<CardManager>();
        heroesManager = GameObject.Find("Cards").GetComponent<HeroesManager>();
        deckManager = GameObject.Find("Cards").GetComponent<DeckManager>();
        cameraMover = GameObject.Find("CameraController").GetComponent<CameraMover>();
        cardsGameManager = GameObject.Find("CardManager").GetComponent<CardsGameManager>();
        tutorials = GameObject.Find("Tutorial").GetComponent<Tutorials>();
    }
}
