using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AerialMovementAction", menuName = "State Machines/Actions/Aerial Movement Action")]
public class AerialMovementActionSO : StateActionSO
{
    public float Speed => _speed;
    public float Acceleration => _acceleration;

    [Tooltip("Desired horizontal movement speed while in the air")]
    [SerializeField][Range(0.1f, 100f)] private float _speed = 10f;
    [Tooltip("The acceleration applied to reach the desired speed")]
    [SerializeField][Range(0.1f, 100f)] private float _acceleration = 20f;

    protected override StateAction CreateAction() => new AerialMovementAction();
}

public class AerialMovementAction : StateAction
{
	protected new AerialMovementActionSO OriginSO => (AerialMovementActionSO)base.OriginSO;

	private Player _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
    }

    public override void OnUpdate()
	{
        Vector3 velocity = _player.movementVector;
        Vector3 input = _player.movementInput; // Normalized Vector
        float speed = OriginSO.Speed;
        float acceleration = OriginSO.Acceleration;

        SetVelocityPerAxis(ref velocity.x, input.x, acceleration, speed); // Update X
        SetVelocityPerAxis(ref velocity.z, input.z, acceleration, speed); // Update Z

        _player.movementVector = velocity;
    }

    private void SetVelocityPerAxis(ref float currentAxisSpeed, float axisInput, float acceleration, float targetSpeed)
    {
        // 입력값이 없으면 공기저항 설정
        if (axisInput == 0.0f)
        {
            if (currentAxisSpeed != 0.0f)
            {
                ApplyAirResistance(ref currentAxisSpeed);
            }
        }
        else
        {
            (float absVel, float absInput) = (Mathf.Abs(currentAxisSpeed), Mathf.Abs(axisInput));
            (float signVel, float signInput) = (Mathf.Abs(currentAxisSpeed), Mathf.Abs(axisInput));
            targetSpeed *= absInput;

            if (signVel != signInput || absVel < targetSpeed)
            {
                currentAxisSpeed += axisInput * acceleration;
                currentAxisSpeed = Mathf.Clamp(currentAxisSpeed, -targetSpeed, targetSpeed);
            }
            else
            {
                ApplyAirResistance(ref currentAxisSpeed);
            }
        }
    }

    private void ApplyAirResistance(ref float value)
    {
        float sign = Mathf.Sign(value);

        value -= sign * Player.AIR_RESISTANCE * Time.deltaTime; // 가는 방향의 반대로 공기 저항 적용
        if (Mathf.Sign(value) != sign) 
        {
            value = 0;
        }
    }
}
