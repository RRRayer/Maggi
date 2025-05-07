using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RunForwardAction", menuName = "State Machines/Actions/Run Forward Action")]
public class RunForwardActionSO : StateActionSO<RunForwardAction>
{
    public float speed = 10f;
}

public class RunForwardAction : StateAction
{
    private PlayerController _player;
    private Rigidbody _rb;
    private RunForwardActionSO _origin => (RunForwardActionSO)OriginSO;

    public override void Awake(StateMachine machine)
    {
        _player = machine.GetComponent<PlayerController>();
        _rb = _player.GetComponent<Rigidbody>();
    }

    public override void OnUpdate()
    {

    }
    public override void OnFixedUpdate()
    {
        if (_player == null || _rb == null || _player.IsDead) return;

        Vector3 forwardVel = _player.transform.forward * _origin.speed;
        Vector3 gravityVel = Vector3.Project(_rb.velocity, Physics.gravity.normalized);
        _rb.velocity = forwardVel + gravityVel;
    }
}