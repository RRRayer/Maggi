using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "SideClickMoveAction", menuName = "State Machines/Actions/Side Click Move Action")]
public class SideClickMoveActionSO : StateActionSO<SideClickMoveAction>
{
    public float sideSpeed = 5f;
}

public class SideClickMoveAction : StateAction
{
    private PlayerController _player;
    private Rigidbody _rb;
    private SideClickMoveActionSO _origin => (SideClickMoveActionSO)OriginSO;

    public override void Awake(StateMachine machine)
    {
        _player = machine.GetComponent<PlayerController>();
        _rb = _player.GetComponent<Rigidbody>();
    }

    public override void OnFixedUpdate()
    {
        if (_player == null || _rb == null || _player.IsDead) return;

        Vector3 right = Vector3.Cross(-Physics.gravity.normalized, _player.transform.forward).normalized;

        if (Input.GetMouseButton(0))
            _rb.MovePosition(_rb.position - right * _origin.sideSpeed * Time.fixedDeltaTime);
        else if (Input.GetMouseButton(1))
            _rb.MovePosition(_rb.position + right * _origin.sideSpeed * Time.fixedDeltaTime);
    }

    public override void OnUpdate() { }
}