using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RotateAction", menuName = "State Machines/Actions/Rotate Action")]
public class RotateActionSO : StateActionSO<RotateAction>
{
	public float turnSmoothTime = 0.2f;
}

public class RotateAction : StateAction
{
	protected new RotateActionSO _originSO => (RotateActionSO)base.OriginSO;
	private Player _player;
	private Transform _transform;
	private float _turnSmoothSpeed;
	private const float ROTATION_TRESHOLD = 0.02f;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
		_transform = stateMachine.GetComponent<Transform>();
	}
	
	public override void OnUpdate()
	{
		Vector3 horizontalMovement = _player.movementVector;
		horizontalMovement.y = 0;

		if (horizontalMovement.sqrMagnitude >= ROTATION_TRESHOLD)
		{
            float targetRotation = Mathf.Atan2(-horizontalMovement.z, horizontalMovement.x) * Mathf.Rad2Deg;
            _transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetRotation, ref _turnSmoothSpeed, _originSO.turnSmoothTime);
        }
	}
}
