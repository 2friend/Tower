using UnityEngine;

public class GameInstance : MonoBehaviour
{
    private GameStateMachine _gameStateMachine;

    private void Awake()
    {
        _gameStateMachine = new GameStateMachine();

        _gameStateMachine.EnterIn<LoadingObjectsState>();
    }
}
