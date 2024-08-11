using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullWallAction", menuName = "State Machines/Actions/Pull Wall Action")]

public class PullWallActionSO : StateActionSO<PullWallAction>
{
    public float Speed = 4.0f;
    public LayerMask WallLayerMask;
}

public class PullWallAction : StateAction
{
	protected new PullWallActionSO _originSO => (PullWallActionSO)base.OriginSO;
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
        //_transform.rotation = _interactionManager.currentInteractiveObject.transform.rotation;

        // 현재 오브젝트의 위치에서 인터랙션 대상의 표면과의 충돌을 감지
        RaycastHit hit;
        if (Physics.Raycast(
            _transform.position,
            _interactionManager.currentInteractiveObject.transform.position - _transform.position,
            out hit,
            2.0f,
            _originSO.WallLayerMask))
        {
            // 충돌한 면의 법선 벡터를 가져옴
            Vector3 wallNormal = hit.normal;

            Debug.Log(wallNormal);

            // Transform의 down 방향을 벽의 법선 벡터로 맞춤
            Quaternion targetRotation = Quaternion.FromToRotation(_transform.up, wallNormal) * _transform.rotation;

            // 계산된 회전을 Transform에 적용
            _transform.rotation = targetRotation;
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
        if (Physics.Raycast(ray, out hit, 2.0f, _originSO.WallLayerMask))
        {
            Vector3 wallNormal = hit.normal;

            Vector3 rightOnPlane = Vector3.Cross(Vector3.back, wallNormal).normalized;
            Vector3 forwardOnPlane = Vector3.Cross(Vector3.right, wallNormal).normalized;
            if (rightOnPlane == Vector3.zero)
                rightOnPlane = Vector3.right;
            if (forwardOnPlane == Vector3.zero)
                forwardOnPlane = Vector3.forward;

            Vector3 newInputVector = new Vector3(_player.movementInput.x, 0, _player.movementInput.z);
            Vector3 newMovementVector = (newInputVector.x * rightOnPlane + newInputVector.z * forwardOnPlane).normalized;

            Vector3 gravityOnPlane = -_transform.up * 5.0f;
            _player.movementVector = (newMovementVector + gravityOnPlane) * _originSO.Speed;
        }
    }
}
