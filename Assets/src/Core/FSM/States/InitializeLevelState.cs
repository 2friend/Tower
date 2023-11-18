public class InitializeLevelState : IGameState
{
    private readonly GameStateMachine _gameStateMachine;

    public InitializeLevelState(GameStateMachine gameStateMachine)
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
