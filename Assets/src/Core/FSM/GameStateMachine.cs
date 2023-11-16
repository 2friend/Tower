using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    private Dictionary<Type, IGameState> _states;
    private IGameState _currentGameState;

    public GameStateMachine()
    {
        _states = new Dictionary<Type, IGameState>()
        {
            [typeof(LoadingObjectsState)] = new LoadingObjectsState(this),
            [typeof(InitializeLevelState)] = new InitializeLevelState(this)
        };
    }

    public void EnterIn<TState>() where TState : IGameState
    {
        if(_states.TryGetValue(typeof(TState), out IGameState state))
        {
            _currentGameState?.Exit();
            _currentGameState = state;
            _currentGameState.Enter();
        }
    }
}
