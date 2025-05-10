using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GravityDescendAction", menuName = "State Machines/Actions/MaggiRun/Gravity Descend")]
public class GravityDescendActionSO : StateActionSO<GravityDescendAction> { }

public class GravityDescendAction : StateAction
{
    private PlayerMaggiRun _player;
    private Rigidbody _rb;

    private float _verticalVelocity;
    private const float GRAVITY_MULTIPLIER = 2f;
    private const float MAX_FALL_SPEED = -50f;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
        _rb = _player.GetComponent<Rigidbody>();
    }

    public override void OnStateEnter()
    {
        _verticalVelocity = Vector3.Dot(_rb.linearVelocity, Physics.gravity.normalized);
    }

    public override void OnUpdate()
    {
        Vector3 down = Physics.gravity.normalized;
        float gravityForce = Physics.gravity.magnitude * GRAVITY_MULTIPLIER;

        _verticalVelocity += gravityForce * Time.deltaTime;
        _verticalVelocity = Mathf.Clamp(_verticalVelocity, MAX_FALL_SPEED, float.MaxValue);

        Vector3 horizontalVel = Vector3.ProjectOnPlane(_rb.linearVelocity, down);
        _rb.linearVelocity = horizontalVel + down * _verticalVelocity;
    }
}