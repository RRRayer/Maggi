using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GroundGravityAction", menuName = "State Machines/Actions/Ground Gravity Action")]
public class GroundGravityActionSO : StateActionSO<GroundGravityAction>
{
	public float verticalPull = -5f;
}

public class GroundGravityAction : StateAction
{
	private GroundGravityActionSO _originSO => (GroundGravityActionSO)base.OriginSO;
    private Player _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
    }

    public override void OnUpdate()
	{
        _player.movementVector.y = _originSO.verticalPull;
	}
}
