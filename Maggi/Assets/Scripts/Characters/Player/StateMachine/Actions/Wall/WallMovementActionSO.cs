using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "WallMovementAction", menuName = "State Machines/Actions/Wall Movement Action")]

public class WallMovementActionSO : StateActionSO<WallMovementAction>
{
    public float speed = 4.0f;
    public LayerMask wallLayerMask;
    public float turnSmoothTime = 0.2f;
}

public class WallMovementAction : StateAction
{
	private WallMovementActionSO _originSO => (WallMovementActionSO)base.OriginSO;
	private Player _player;
    private Transform _transform;
	private InteractionManager _interactionManager;
    private InteractiveObject _interactiveObject;
    private const float ROTATION_TRESHOLD = 0.02f;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
	{
		_player = owner.GetComponent<Player>();
        _transform = owner.GetComponent<Transform>();
        _interactionManager = owner.GetComponent<InteractionManager>();
        _interactiveObject = interactiveObject;
    }

    public override void OnStateEnter()
    {
        // 현재 오브젝트의 위치에서 인터랙션 대상의 표면과의 충돌을 감지
        RaycastHit hit;
        if (Physics.Raycast(
            _transform.position,
            _interactiveObject.transform.position - _transform.position,
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
            _transform.rotation = _interactiveObject.transform.rotation;
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
            if (newMovementVector.sqrMagnitude >= ROTATION_TRESHOLD)
            {
                Quaternion targetRotation = Quaternion.LookRotation(newMovementVector, wallNormal);
                // _transform.up을 기준으로 90도 회전
                Quaternion additionalRotation = Quaternion.AngleAxis(-90.0f, _transform.up);

                // 현재 회전에 추가 회전을 곱함
                targetRotation = additionalRotation * targetRotation;

                _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * _originSO.turnSmoothTime);
            }
        }
    }
}
