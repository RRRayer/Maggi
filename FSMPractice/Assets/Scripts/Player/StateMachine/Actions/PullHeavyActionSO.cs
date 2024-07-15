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
    private Vector3 _offset;
    private Vector3 _previousPlayerPosition;

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

        _previousPlayerPosition = _player.transform.position;

        Vector3 currentInteractiveObjectPosition = _interactionManager.currentInteractiveObject.transform.position;
        Vector3 distanceVector = currentInteractiveObjectPosition - _player.transform.position;
        float interactiveObjectSize = _interactionManager.currentInteractiveObject.transform.localScale.x; // 박스의 한 변의 길이의 절반
        float distance = interactiveObjectSize / 2 + _player.transform.localScale.x - 0.2f;

        #region Calculate Offset
        if (distanceVector.x >= interactiveObjectSize / 2) // 왼에서 잡음
        {
            _offset = new Vector3(distance, currentInteractiveObjectPosition.y - _player.transform.position.y, 0);
        }
        else if (-distanceVector.x >= interactiveObjectSize / 2) // 오에서 잡음
        {
            _offset = new Vector3(-distance, currentInteractiveObjectPosition.y - _player.transform.position.y, 0);
        }
        else if (distanceVector.z >= interactiveObjectSize / 2) // 앞에서 잡음
        {
            _offset = new Vector3(0, currentInteractiveObjectPosition.y - _player.transform.position.y, distance);
        }
        else if (-distanceVector.z >= interactiveObjectSize / 2) // 뒤에서 잡음
        {
            _offset = new Vector3(0, currentInteractiveObjectPosition.y - _player.transform.position.y, -distance);
        }
        #endregion
    }

    public override void OnUpdate() { }

    public override void OnFixedUpdate()
    {
        // 오프셋을 이용하여 상호작용 오브젝트의 목표 위치 계산
        Vector3 objectTargetPosition = _player.transform.position + _offset;
        Vector3 direction = (objectTargetPosition - _interactiveObjectRigidbody.position).normalized;
        Vector3 newPosition = _interactiveObjectRigidbody.position + direction * _originSO.moveSpeed * Time.fixedDeltaTime;

        // 충돌 검사 및 이동

        // 벽과 충돌한 경우
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

    private void OnDrawGizmos()
    {
        if (_interactiveObjectRigidbody != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_interactiveObjectRigidbody.position, _interactiveObjectRigidbody.transform.localScale);
        }
    }
}
