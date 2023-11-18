using UnityEngine;

public class LoadingObjectsState : IGameState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly GridObjects _gridObjects;
    private readonly PathController _pathController;
    private readonly WaveController _waveController;
    private readonly GridController _gridController;
    private readonly CardManager _cardManager;

    public LoadingObjectsState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
        _gridObjects = GameObject.Find("GridObjects").GetComponent<GridObjects>();
        _pathController = GameObject.Find("PathController").GetComponent<PathController>();
        _waveController = GameObject.Find("WaveController").GetComponent<WaveController>();
        _gridController = GameObject.Find("Grid").GetComponent<GridController>();
        //_cardManager = GameObject.Find("").GetComponent<CardManager>();
    }

    public void Enter()
    {
        Debug.Log("[State] [Loading] Start Loading!");
        _gridObjects.ReadEnemysFile();
        _gridObjects.ReadTowersFile();

        _pathController.ReadPathFile();

        _waveController.ReadWavesFile();

        _gridController.GenerateGrid();

        _gameStateMachine.EnterIn<InitializeLevelState>();
    }

    public void Exit()
    {
        Debug.Log("[State] [Loading] End Loading!");
    }
}
