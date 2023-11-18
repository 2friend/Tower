using UnityEngine;

public class InitializeLevelState : IGameState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly Tutorials _tutorials;

    public InitializeLevelState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
        _tutorials = GameObject.Find("Tutorial").GetComponent<Tutorials>();
    }
    public void Enter()
    {
        _tutorials.StartTutorial(_tutorials.tutors[0]);
    }

    public void Exit()
    {
        
    }
}
