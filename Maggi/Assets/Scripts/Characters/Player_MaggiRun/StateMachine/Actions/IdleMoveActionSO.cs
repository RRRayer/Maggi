using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IdleMoveAction", menuName = "State Machines/Actions/MaggiRun/Idle Move")]
public class IdleMoveActionSO : StateActionSO<IdleMoveAction> 
{
    public float forwardSpeed = 8f;
}

public class IdleMoveAction : StateAction
{
    private PlayerMaggiRun _player;
    private Rigidbody _rb;
    private IdleMoveActionSO _origin => (IdleMoveActionSO)OriginSO;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
        _rb = stateMachine.GetComponent<Rigidbody>();
    }

    public override void OnUpdate() { }

    public override void OnFixedUpdate()
    {
        if (_player == null || _rb == null) return;

        Vector3 forward = _player.transform.forward;
        Vector3 gravityVel = Vector3.Project(_rb.velocity, Physics.gravity.normalized);
        _rb.velocity = forward * _origin.forwardSpeed + gravityVel;
    }
}