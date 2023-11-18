using UnityEngine;

public class LoadingObjectsState : IGameState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly Logger _logger;
    private readonly GameConstant _gameConstant;
    private readonly GridObjects _gridObjects;
    private readonly PathController _pathController;
    private readonly WaveController _waveController;
    private readonly GridController _gridController;
    private readonly Tutorials _tutorials;
    //private readonly CardManager _cardManager;

    public LoadingObjectsState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
        _logger = GameObject.Find("Logger").GetComponent<Logger>();
        _gameConstant = GameObject.Find("Controllers").GetComponent<GameConstant>();
        _pathController = GameObject.Find("PathController").GetComponent<PathController>();
        _gridController = GameObject.Find("Grid").GetComponent<GridController>();
        _gridObjects = GameObject.Find("GridObjects").GetComponent<GridObjects>();
        _waveController = GameObject.Find("WaveController").GetComponent<WaveController>();
        _tutorials = GameObject.Find("Tutorial").GetComponent<Tutorials>();
        //_cardManager = GameObject.Find("").GetComponent<CardManager>();
    }

    public void Enter()
    {
        _logger.StartWritingLogs();

        Debug.Log("[State] [Loading] Start Loading!");

        _gameConstant.AssignValues();

        _pathController.ReadPathFile();

        _gridController.GenerateGrid();

        _gridObjects.ReadEnemysFile();

        _gridObjects.ReadTowersFile();

        _waveController.ReadWavesFile();

        _tutorials.ReadTutorsFile();

        _gameStateMachine.EnterIn<InitializeLevelState>();
    }

    public void Exit()
    {
        Debug.Log("[State] [Loading] End Loading!");
    }
}
