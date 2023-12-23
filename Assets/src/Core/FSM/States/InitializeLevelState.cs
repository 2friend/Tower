using UnityEngine;

public class InitializeLevelState : IGameState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly CardsGameManager _cardGameManager;
    private readonly Tutorials _tutorials;

    public InitializeLevelState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
        _cardGameManager = GameObject.Find("CardManager").GetComponent<CardsGameManager>();
        _tutorials = GameObject.Find("Tutorial").GetComponent<Tutorials>();
    }

    public void Enter()
    {
        _cardGameManager.playerDeck = GameConstant.deckManager.GetDeckByName("Test");
        _cardGameManager.enemyDeck = GameConstant.deckManager.GetDeckByName("Test");
        _cardGameManager.StartGame();

        _tutorials.StartTutorial(_tutorials.tutors[0]);
    }

    public void Exit()
    {
        
    }
}
