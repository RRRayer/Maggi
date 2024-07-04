using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HorizontalMoveAction", menuName = "State Machines/Actions/Horizontal Move Action")]
public class HorizontalMoveActionSO : StateActionSO<HorizontalMoveAction>
{
    public float speed;
}

public class HorizontalMoveAction : StateAction
{
	private HorizontalMoveActionSO _originSO => (HorizontalMoveActionSO)base.OriginSO;
    private Player _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
    }

    public override void OnUpdate()
    {
        _player.movementVector.x = _player.movementInput.x * _originSO.speed;
        _player.movementVector.z = _player.movementInput.z * _originSO.speed;
    }
}
