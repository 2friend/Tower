using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDeckState : IGameState
{
    private readonly GameStateMachine _gameStateMachine;

    public CreateDeckState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
