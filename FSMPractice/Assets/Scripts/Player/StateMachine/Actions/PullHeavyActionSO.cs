using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

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

        _previousPlayerPosition = _player.transform.position;

        // Freeze rotation to prevent the box from rolling
        _interactiveObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        Vector3 distanceVector = _interactionManager.currentInteractiveObject.transform.position - _player.transform.position;
        float interactiveObjectSize = _interactionManager.currentInteractiveObject.transform.localScale.x; // 박스의 한 변의 길이의 절반
        float distance = interactiveObjectSize / 2 + _player.transform.localScale.x + 0.2f;

        if (distanceVector.x >= interactiveObjectSize / 2) // 왼에서 잡음
        {
            _player.transform.position = new Vector3(
                _interactionManager.currentInteractiveObject.transform.position.x - distance,
                _player.transform.position.y,
                _interactionManager.currentInteractiveObject.transform.position.z
            );
            _offset = _interactionManager.currentInteractiveObject.transform.position - _player.transform.position;
        }
    }

    public override void OnUpdate()
    {
        Vector3 targetPosition = _player.transform.position + _offset;
        Vector3 direction = (targetPosition - _interactiveObjectRigidbody.position).normalized;
        Vector3 newPosition = _interactiveObjectRigidbody.position + direction * _originSO.moveSpeed * Time.fixedDeltaTime;

        // Check if the new position collides with anything
        if (!Physics.CheckBox(newPosition, _interactiveObjectRigidbody.transform.localScale / 2, Quaternion.identity, _originSO.collisionLayerMask))
        {
            _interactiveObjectRigidbody.MovePosition(newPosition);
        }
        else
        {
            // 벽과 수직으로 이동
            Vector3 slideDirection = Vector3.Cross(Vector3.up, direction).normalized;
            // 슬라이드 방향의 외적 값이 양수인지 음수인지 결정
            if (Vector3.Dot(_player.movementInput, slideDirection) < 0)
            {
                slideDirection = -slideDirection;
            }

            Vector3 slidePosition = _interactiveObjectRigidbody.position + slideDirection * _originSO.moveSpeed * Time.deltaTime;

            if (!Physics.CheckBox(slidePosition, _interactiveObjectRigidbody.transform.localScale / 2, Quaternion.identity, _originSO.collisionLayerMask))
            {
                _interactiveObjectRigidbody.MovePosition(slidePosition);
            }
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
