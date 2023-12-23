using UnityEngine;

public class LoadingObjectsState : IGameState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly Logger _logger;
    private readonly ResourcesLoader _resourcesLoader;
    private readonly GameConstant _gameConstant;
    private readonly GridObjects _gridObjects;
    private readonly PathController _pathController;
    private readonly WaveController _waveController;
    private readonly GridController _gridController;
    private readonly CardManager _cardManager;
    private readonly HeroesManager _heroesManager;
    private readonly DeckManager _deckManager;
    private readonly Tutorials _tutorials;

    public LoadingObjectsState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
        _logger = GameObject.Find("Logger").GetComponent<Logger>();
        _resourcesLoader = GameObject.Find("Resources").GetComponent<ResourcesLoader>();
        _gameConstant = GameObject.Find("Controllers").GetComponent<GameConstant>();
        _pathController = GameObject.Find("PathController").GetComponent<PathController>();
        _gridController = GameObject.Find("Grid").GetComponent<GridController>();
        _gridObjects = GameObject.Find("GridObjects").GetComponent<GridObjects>();
        _waveController = GameObject.Find("WaveController").GetComponent<WaveController>();
        _cardManager = GameObject.Find("Cards").GetComponent<CardManager>();
        _heroesManager = GameObject.Find("Cards").GetComponent<HeroesManager>();
        _deckManager = GameObject.Find("Cards").GetComponent<DeckManager>();
        _tutorials = GameObject.Find("Tutorial").GetComponent<Tutorials>();
    }

    public void Enter()
    {
        _logger.StartWritingLogs();

        Debug.Log("[State] [Loading] Start Loading!");

        _resourcesLoader.LoadSprites();

        _gameConstant.AssignValues();

        _pathController.ReadPathFile();

        _gridController.GenerateGrid();

        _gridObjects.ReadEnemysFile();

        _gridObjects.ReadTowersFile();

        _heroesManager.LoadAbilities();
        _heroesManager.LoadHeroes();

        _cardManager.LoadCards();

        _deckManager.ReadAllDecks();

        _tutorials.ReadTutorsFile();

        _waveController.ReadWavesFile();

        _gameStateMachine.EnterIn<InitializeLevelState>();
    }

    public void Exit()
    {
        Debug.Log("[State] [Loading] End Loading!");
    }
}
