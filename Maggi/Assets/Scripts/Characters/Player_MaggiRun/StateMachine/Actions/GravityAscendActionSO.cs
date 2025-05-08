using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GravityAscendAction", menuName = "State Machines/Actions/MaggiRun/Gravity Ascend")]
public class GravityAscendActionSO : StateActionSO<GravityAscendAction>
{
    public float initialJumpForce = 15f;
}

public class GravityAscendAction : StateAction
{
    private PlayerMaggiRun _player;
    private Rigidbody _rb;
    private GravityAscendActionSO _origin => (GravityAscendActionSO)OriginSO;

    private float _verticalVelocity;
    private float _gravityTimer;
    private float _gravityFactor = 1f;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
        _rb = _player.GetComponent<Rigidbody>();
    }

    public override void OnStateEnter()
    {
        _verticalVelocity = _origin.initialJumpForce;
        _player.jumpInput = false;
        _gravityTimer = 0f;
    }

    public override void OnUpdate()
    {
        Vector3 up = -Physics.gravity.normalized;
        
        _gravityTimer += Time.deltaTime;
        float effectiveGravity = Physics.gravity.y * 2f * Mathf.Clamp01(_gravityTimer);  // 점점 커짐

        _verticalVelocity += effectiveGravity * Time.deltaTime;

        Vector3 horizontalVel = Vector3.ProjectOnPlane(_rb.velocity, up);
        _rb.velocity = horizontalVel + up * _verticalVelocity;
    }
}