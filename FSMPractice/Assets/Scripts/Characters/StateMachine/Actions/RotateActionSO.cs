using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RotateAction", menuName = "State Machines/Actions/Rotate Action")]
public class RotateActionSO : StateActionSO<RotateAction>
{
	public float turnSmoothTime = 0.2f;
}

public class RotateAction : StateAction
{
	private RotateActionSO _originSO => (RotateActionSO)base.OriginSO;
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
            _transform.eulerAngles = Vector3.up *
                Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetRotation, ref _turnSmoothSpeed, _originSO.turnSmoothTime);
        }

        //// Movement Vector를 사용하여 회전을 결정
        //Vector3 movementVector = _player.movementVector;
        //Vector3 inputVector = _player.movementInput;

        //// Movement Vector의 크기가 일정 임계값 이상일 때 회전을 계산
        //if (inputVector.sqrMagnitude >= ROTATION_TRESHOLD)
        //{
        //    // 현재 이동 벡터의 방향을 회전각도로 변환
        //    float targetRotation = Mathf.Atan2(inputVector.x, inputVector.z) * Mathf.Rad2Deg;

        //    // y 축 회전도 고려하여 회전 적용
        //    Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation - 90.0f, 0);

        //    // 현재 회전과 목표 회전 사이를 부드럽게 전환
        //    _transform.rotation = Quaternion.Slerp(_transform.rotation, targetQuaternion, _originSO.turnSmoothTime * Time.deltaTime);
        //}
    }
}
