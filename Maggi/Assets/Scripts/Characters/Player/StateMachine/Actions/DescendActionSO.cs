using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "DescendAction", menuName = "State Machines/Actions/Descend Action")]
public class DescendActionSO : StateActionSO<DescendAction> { }

public class DescendAction : StateAction
{
	private Player _player;

	private float _verticalMovement;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
	}

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
    }

    public override void OnStateEnter()
    {
        _verticalMovement = _player.movementVector.y;
        _player.jumpInput = false;
    }

    public override void OnUpdate()
	{
        _verticalMovement += Physics.gravity.y * Player.GRAVITY_MULTIPLIER * Time.deltaTime;
        _verticalMovement = Mathf.Clamp(_verticalMovement, Player.MAX_FALL_SPEED, Player.MAX_RISE_SPEED);

        _player.movementVector.y = _verticalMovement;
	}
}
