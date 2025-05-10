using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "StrafeAction", menuName = "State Machines/Actions/MaggiRun/Strafe Move")]
public class StrafeActionSO : StateActionSO<StrafeAction>
{
    public float sideSpeed = 6f;
}

public class StrafeAction : StateAction
{
    private PlayerMaggiRun _player;
    private Rigidbody _rb;
    private StrafeActionSO _origin => (StrafeActionSO)OriginSO;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
        _rb = stateMachine.GetComponent<Rigidbody>();
    }

    public override void OnUpdate() { }

    public override void OnFixedUpdate()
    {
        if (_player == null || _rb == null) return;

        // 좌우 이동 처리
        Vector3 right = _player.transform.right;
        Vector3 gravityVel = Vector3.Project(_rb.linearVelocity, Physics.gravity.normalized);
        _rb.linearVelocity = right * _player.movementInput.x * _origin.sideSpeed + gravityVel;

        // 중력 회전 처리
        if (_player.NeedsGravityRotation)
        {
            Quaternion targetRot = _player.GetTargetRotation();
            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRot, 360f * Time.fixedDeltaTime));

            if (Quaternion.Angle(_rb.rotation, targetRot) < 0.1f)
                _player.FinishGravityRotation();
        }
    }
}