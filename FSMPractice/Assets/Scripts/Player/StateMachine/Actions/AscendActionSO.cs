using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AscendAction", menuName = "State Machines/Actions/Ascend Action")]
public class AscendActionSO : StateActionSO<AscendAction>
{
	[SerializeField] public float initialJumpForce = 6.0f;
}

public class AscendAction : StateAction
{
	protected new AscendActionSO OriginSO => (AscendActionSO)base.OriginSO;
	private Player _player;

	private float _verticalMovement;
    private float _gravityContributionMultiplier;

    public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
	}

	public override void OnStateEnter()
	{
		_verticalMovement = OriginSO.initialJumpForce;
	}
	
	public override void OnUpdate()
	{
        _gravityContributionMultiplier += Player.GRAVITY_COMEBACK_MULTIPLIER;
        _gravityContributionMultiplier *= Player.GRAVITY_DIVIDER; //Reduce the gravity effect

        _verticalMovement += Physics.gravity.y * Player.GRAVITY_MULTIPLIER * _gravityContributionMultiplier * Time.deltaTime;
        _player.movementVector.y = _verticalMovement;
	}
}
