using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using Moment = Maggi.StateMachine.StateAction.SpecificMoment;

[CreateAssetMenu(fileName = "ChangeGameStateAction", menuName = "State Machines/Actions/Change Game State Action")]
public class ChangeGameStateActionSO : StateActionSO
{
	[SerializeField] private GameState _newGameState = default;
    [SerializeField] private GameStateSO _gameState = default;
    [SerializeField] private Moment _whenToRun = default;

    protected override StateAction CreateAction() => new ChangeGameStateAction(_newGameState, _gameState, _whenToRun);
}

public class ChangeGameStateAction : StateAction
{
	private GameState _newGameState = default;
	private GameStateSO _gameStateSO = default;
	private Moment _whenToRun = default;

	public ChangeGameStateAction(GameState newGameState, GameStateSO gameStateSO, Moment whenToRun)
    {
        _newGameState = newGameState;
        _gameStateSO = gameStateSO;
        _whenToRun = whenToRun;
    }

	private void ChangeState()
	{
		_gameStateSO.UpdateGameState(_newGameState);
	}
	
	public override void OnStateEnter()
	{
		if (_whenToRun == Moment.OnStateEnter)
		{
			ChangeState();
		}
	}
	
	public override void OnStateExit()
	{
        if (_whenToRun == Moment.OnStateExit)
        {
            ChangeState();
        }
    }

    public override void OnUpdate() { }
}
