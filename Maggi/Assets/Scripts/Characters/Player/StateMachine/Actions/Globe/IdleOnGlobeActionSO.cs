using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IdleOnGlobeAction", menuName = "State Machines/Actions/Rotate On Globe Action")]
public class IdleOnGlobeActionSO : StateActionSO<IdleOnGlobeAction>
{
    [SerializeField]
    public float rotationSpeed;
}

public class IdleOnGlobeAction : StateAction
{
	protected new IdleOnGlobeActionSO _originSO => (IdleOnGlobeActionSO)base.OriginSO;

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
    }
}
