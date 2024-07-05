using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "VerticalFixAction", menuName = "State Machines/Actions/Vertical Fix Action")]
public class VerticalFixActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new VerticalFixAction();
}

public class VerticalFixAction : StateAction
{
	private VerticalFixActionSO _originSO => (VerticalFixActionSO)base.OriginSO;
    private Player _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
    }

    public override void OnUpdate()
    {
        _player.movementVector.y = 0.0f;
    }
}
