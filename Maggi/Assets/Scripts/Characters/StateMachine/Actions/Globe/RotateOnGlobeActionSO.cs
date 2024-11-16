using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RotateOnGlobeAction", menuName = "State Machines/Actions/Rotate On Globe Action")]
public class RotateOnGlobeActionSO : StateActionSO<RotateOnGlobeAction>
{
    [SerializeField]
    public float rotationSpeed;
}

public class RotateOnGlobeAction : StateAction
{
	protected new RotateOnGlobeActionSO _originSO => (RotateOnGlobeActionSO)base.OriginSO;

    private Player _player;
    private Transform _transform;
	private InteractionManager _interactionManager;


    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnUpdate()
    {
		_transform.LookAt(_interactionManager.currentInteractiveObject.transform.position);
		_player.movementVector = -_transform.right * -_player.movementInput.x + _transform.up * _player.movementInput.z;
    }
}
