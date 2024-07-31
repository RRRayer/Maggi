using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullHeavyAction", menuName = "State Machines/Actions/Pull Heavy Action")]
public class PullHeavyActionSO : StateActionSO<PullHeavyAction>
{
    public LayerMask collisionLayerMask;
    public float moveSpeed = 2.0f;
}

public class PullHeavyAction : StateAction
{
    protected new PullHeavyActionSO _originSO => (PullHeavyActionSO)base.OriginSO;
    private Player _player;
    private InteractionManager _interactionManager;

    private Rigidbody _interactiveObjectRigidbody;
    private Vector3 _offset = Vector3.zero;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnStateEnter()
    {
        _interactiveObjectRigidbody = _interactionManager.currentInteractiveObject.GetComponent<Rigidbody>();
        // Freeze rotation to prevent the box from rolling
        _interactiveObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        Vector3 currentInteractiveObjectPosition = _interactionManager.currentInteractiveObject.transform.position;
        Vector3 playerPosition = _player.transform.position;

        // 박스 콜라이더의 크기와 위치 정보를 가져옴
        BoxCollider boxCollider = _interactionManager.currentInteractiveObject.GetComponent<BoxCollider>();
        Vector3 boxSize = boxCollider.size;
        Vector3 boxCenter = boxCollider.center;

        // 플레이어->박스 사이의 벡터
        Vector3 distanceVector = currentInteractiveObjectPosition - playerPosition;

        // 박스의 로컬 반지름 (half extents)
        Vector3 halfBoxSize = boxSize * 0.5f;

        // 플레이어의 크기 (반지름)
        float playerHalfSize = _player.transform.localScale.x * 0.5f;

        // 각 축에 따른 거리 계산
        float distanceX = halfBoxSize.x + playerHalfSize + 0.2f;
        float distanceZ = halfBoxSize.z + playerHalfSize + 0.2f;

        // 로컬 축 벡터
        Vector3 localRight = boxCollider.transform.right;
        Vector3 localForward = boxCollider.transform.forward;

        #region Calculate Offset
        // 각 축에 투영된 거리를 계산
        float projectedDistanceX = Vector3.Dot(distanceVector, localRight);
        float projectedDistanceZ = Vector3.Dot(distanceVector, localForward);

        if (Mathf.Abs(projectedDistanceX) > Mathf.Abs(projectedDistanceZ))
        {
            if (projectedDistanceX <= -halfBoxSize.x) // 오른쪽에서 잡음 <-
            {
                _offset = -localRight * distanceX;
            }
            else if (projectedDistanceX >= halfBoxSize.x) // 왼쪽에서 잡음 ->
            {
                _offset = localRight * distanceX;
            }
            else
            {
                Debug.LogWarning("Player and Heavy Collider is overlapped");
                return;
            }
        }
        else
        {
            if (projectedDistanceZ >= halfBoxSize.z) // 앞쪽에서 잡음 ^
            {
                _offset = localForward * distanceZ;
            }
            else if (projectedDistanceZ <= -halfBoxSize.z) // 뒤쪽에서 잡음 v
            {
                _offset = -localForward * distanceZ;
            }
            else
            {
                Debug.LogWarning("Player and Heavy Collider is overlapped");
                return;
            }
        }
        #endregion

        // 높이 보정
        _offset.y = currentInteractiveObjectPosition.y - playerPosition.y;
    }


    public override void OnUpdate() { }

    public override void OnFixedUpdate()
    {
        // 오프셋을 이용하여 상호작용 오브젝트의 목표 위치 계산
        Vector3 objectTargetPosition = _player.transform.position + _offset;
        Vector3 direction = (objectTargetPosition - _interactiveObjectRigidbody.position).normalized;
        Vector3 newPosition = _interactiveObjectRigidbody.position + direction * _originSO.moveSpeed * Time.fixedDeltaTime;

        // 충돌 검사 및 이동

        // 땅에서의 이동
        if (!Physics.CheckBox(newPosition, _interactiveObjectRigidbody.transform.localScale / 2, Quaternion.identity, _originSO.collisionLayerMask))
        {
            _interactiveObjectRigidbody.MovePosition(newPosition);
        }
        // 벽과 접촉한 경우, 벽면에 수직으로 이동
        else
        {
            // 외적으로 진행방향과 수직인 벡터를 구함
            Vector3 slideDirection = Vector3.Cross(Vector3.up, direction).normalized;
            // 내적으로 슬라이드 방향이 음수인지 양수인지 구함
            if (Vector3.Dot(_player.movementInput, slideDirection) < 0)
            {
                slideDirection = -slideDirection;
            }
            else if (Vector3.Dot(_player.movementInput, slideDirection) == 0)
            {
                slideDirection = Vector3.zero;
            }

            newPosition = _interactiveObjectRigidbody.position + slideDirection * _originSO.moveSpeed * Time.deltaTime;

            if (!Physics.CheckBox(newPosition, _interactiveObjectRigidbody.transform.localScale / 2, Quaternion.identity, _originSO.collisionLayerMask))
            {
                _interactiveObjectRigidbody.MovePosition(newPosition);
            }
        }

        // Player의 위치를 알맞게 조절
        if (_interactiveObjectRigidbody.position == newPosition)
        {
            _player.transform.position = newPosition - _offset;
        }
    }

    public override void OnStateExit()
    {
        // Unfreeze rotation if needed when exiting the state
        _interactiveObjectRigidbody.constraints = RigidbodyConstraints.None;
    }
}
