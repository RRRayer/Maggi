using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "WallMovementAction", menuName = "State Machines/Actions/Wall Movement Action")]

public class WallMovementActionSO : StateActionSO<WallMovementAction>
{
    public float speed = 4.0f;
    public LayerMask wallLayerMask;
    public float turnSmoothTime = 0.2f;
}

public class WallMovementAction : StateAction
{
	protected new WallMovementActionSO _originSO => (WallMovementActionSO)base.OriginSO;
	private Player _player;
    private Transform _transform;
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

    public override void OnStateEnter()
    {
        // 현재 오브젝트의 위치에서 인터랙션 대상의 표면과의 충돌을 감지
        RaycastHit hit;
        if (Physics.Raycast(
            _transform.position,
            _interactionManager.currentInteractiveObject.transform.position - _transform.position,
            out hit,
            2.0f,
            _originSO.wallLayerMask))
        {
            Vector3 wallNormal = hit.normal;

            // Transform의 down 방향을 벽의 법선 벡터로 맞춤
            _transform.rotation = Quaternion.FromToRotation(_transform.up, wallNormal) * _transform.rotation;
        }
        else
        {
            // Raycast에 실패한 경우 기본 회전 설정
            _transform.rotation = _interactionManager.currentInteractiveObject.transform.rotation;
        }
    }

    public override void OnUpdate()
	{
        Ray ray = new Ray(_transform.position, -_transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2.0f, _originSO.wallLayerMask))
        {
            Vector3 wallNormal = hit.normal;

            Transform _interactiveObjectTransform = _interactionManager.currentInteractiveObject.transform;
            Vector3 rightOnPlane = Vector3.Cross(-_interactiveObjectTransform.forward, wallNormal).normalized;
            Vector3 forwardOnPlane = Vector3.Cross(_interactiveObjectTransform.right, wallNormal).normalized;
            if (rightOnPlane == Vector3.zero) rightOnPlane = Vector3.right;
            if (forwardOnPlane == Vector3.zero) forwardOnPlane = Vector3.forward;

            Vector3 newInputVector = new Vector3(_player.movementInput.x, 0, _player.movementInput.z);
            Vector3 newMovementVector = (newInputVector.x * rightOnPlane + newInputVector.z * forwardOnPlane).normalized;

            Vector3 gravityOnPlane = -_transform.up * 5.0f;
            _player.movementVector = (newMovementVector + gravityOnPlane) * _originSO.speed;

            // _transform을 이동 벡터 방향으로 회전
            if (newMovementVector != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(newMovementVector, wallNormal);

                // 벽면에 맞게 Z축 회전을 적용 (예: Vector3.up 방향이 벽면의 법선과 일치하게 회전)
                Quaternion additionalRotation = Quaternion.FromToRotation(Vector3.up, wallNormal) * Quaternion.Euler(0, 0, 90);
                targetRotation = targetRotation * additionalRotation;
                _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * _originSO.turnSmoothTime);
            }
        }
    }
}
