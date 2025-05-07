using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GravityDescendAction", menuName = "State Machines/Actions/Gravity Descend Action")]
public class GravityDescendActionSO : StateActionSO<GravityDescendAction> { }

public class GravityDescendAction : StateAction
{
    private PlayerController2 _player;
    private Rigidbody _rb;

    private float _verticalVelocity;

    private const float GRAVITY_MULTIPLIER = 2f;
    private const float MAX_FALL_SPEED = -50f;
    private const float MAX_RISE_SPEED = 100f;

    public override void Awake(StateMachine.StateMachine machine)
    {
        _player = machine.GetComponent<PlayerController2>();
        _rb = machine.GetComponent<Rigidbody>();
    }

    public override void OnStateEnter()
    {
        _verticalVelocity = _rb.velocity.y;
    }

    public override void OnUpdate()
    {
        _verticalVelocity += Physics.gravity.y * GRAVITY_MULTIPLIER * Time.deltaTime;
        _verticalVelocity = Mathf.Clamp(_verticalVelocity, MAX_FALL_SPEED, MAX_RISE_SPEED);

        Vector3 horizontal = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.velocity = horizontal + Vector3.up * _verticalVelocity;
    }

    public override void OnFixedUpdate() { }
}