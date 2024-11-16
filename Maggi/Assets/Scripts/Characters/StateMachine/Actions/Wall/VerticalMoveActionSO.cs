using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "VerticalMoveAction", menuName = "State Machines/Actions/Vertical Move Action")]
public class VerticalMoveActionSO : StateActionSO<VerticalMoveAction> { }

public class VerticalMoveAction : StateAction
{
    private Player _player;
    private Transform _transform;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
    }

    public override void OnUpdate()
    {
        Vector3 newMovementVector = _player.movementVector;

        RaycastHit hit;
        if (Physics.Raycast(_transform.position, -_transform.up, out hit, 1.5f))
        {
            Vector3 wallNormal = hit.normal;

            newMovementVector = Vector3.ProjectOnPlane(newMovementVector, wallNormal);
            Debug.Log("øÚ¡˜¿” ∫§≈Õ : " + newMovementVector + ", ∫Æ ≥Î∏÷ ∫§≈Õ : " + wallNormal);
        }

        _player.movementVector = newMovementVector;
    }
}
