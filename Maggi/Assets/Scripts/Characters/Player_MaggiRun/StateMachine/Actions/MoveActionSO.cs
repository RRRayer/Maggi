using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "MoveAction", menuName = "State Machines/Actions/MaggiRun/Move")]
public class MoveActionSO : StateActionSO<MoveAction>
{
    public float forwardSpeed = 8f;
    public float sideSpeed = 6f;
}

public class MoveAction : StateAction
{
    private PlayerMaggiRun _player;
    private Rigidbody _rb;
    private MoveActionSO _origin => (MoveActionSO)OriginSO;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
        _rb = stateMachine.GetComponent<Rigidbody>();
    }

    public override void OnFixedUpdate()
    {
        if (_player == null || _rb == null) return;

        Vector3 forward = _player.transform.forward;
        Vector3 right = _player.transform.right;

        Vector3 move = forward * _origin.forwardSpeed + right * _player.movementInput.x * _origin.sideSpeed;
        Vector3 gravityVel = Vector3.Project(_rb.velocity, Physics.gravity.normalized);

        _rb.velocity = move + gravityVel;
    }

    public override void OnUpdate() { }
}