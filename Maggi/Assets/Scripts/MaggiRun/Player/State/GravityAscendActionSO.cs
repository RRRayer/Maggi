using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GravityAscendAction", menuName = "State Machines/Actions/Gravity Ascend Action")]
public class GravityAscendActionSO : StateActionSO<GravityAscendAction>
{
    [SerializeField] public float initialJumpForce = 15.0f;
}


public class GravityAscendAction : StateAction
{
    private GravityAscendActionSO _origin => (GravityAscendActionSO)OriginSO;
    private PlayerController _player;
    private Rigidbody _rb;
    private const float GRAVITY_MULTIPLIER = 2f;
    private const float GRAVITY_COMEBACK_MULTIPLIER = 0.06f;
    private const float GRAVITY_DIVIDER = 0.6f;


    private float _verticalMovement;
    private float _gravityContributionMultiplier;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerController>();
        _rb = _player.GetComponent<Rigidbody>();
    }

    public override void OnStateEnter()
    {
        _verticalMovement = _origin.initialJumpForce;
        _gravityContributionMultiplier = 1f;
    }

    public override void OnUpdate()
    {
        Vector3 up = -Physics.gravity.normalized;

        _gravityContributionMultiplier += GRAVITY_COMEBACK_MULTIPLIER;
        _gravityContributionMultiplier *= GRAVITY_DIVIDER;

        _verticalMovement += Physics.gravity.magnitude * GRAVITY_MULTIPLIER * _gravityContributionMultiplier * Time.deltaTime;
        _rb.velocity = up * _verticalMovement + Vector3.Project(_rb.velocity, Physics.gravity.normalized);
    }

    public override void OnFixedUpdate() { }
}