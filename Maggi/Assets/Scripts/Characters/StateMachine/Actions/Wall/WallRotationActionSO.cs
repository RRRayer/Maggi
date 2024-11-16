using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "WallRotationAction", menuName = "State Machines/Actions/Wall Rotation Action")]
public class WallRotationActionSO : StateActionSO<WallRotationAction>
{
    public float turnSmoothTime = 0.2f;
}

public class WallRotationAction : StateAction
{
	private WallRotationActionSO _originSO => (WallRotationActionSO)base.OriginSO;
    private Player _player;
    private Transform _transform;
    private InteractionManager _interactionManager;
    private float _turnSmoothSpeed;
    private const float ROTATION_TRESHOLD = 0.02f;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnUpdate()
	{
		//Vector3 direction = _player.movementVector;

  //      if (direction.sqrMagnitude >= ROTATION_TRESHOLD)
  //      {
  //          _transform.rotation = Quaternion.LookRotation(direction);
  //      }
    }
}
