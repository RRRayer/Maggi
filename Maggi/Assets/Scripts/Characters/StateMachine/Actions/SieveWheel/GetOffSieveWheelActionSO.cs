using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GetOffSieveWheelAction", menuName = "State Machines/Actions/Get Off Sieve Wheel Action")]
public class GetOffSieveWheelActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new GetOffSieveWheelAction();
}

public class GetOffSieveWheelAction : StateAction
{
	protected new GetOffSieveWheelActionSO _originSO => (GetOffSieveWheelActionSO)base.OriginSO;

    private Transform _transform;
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

    public override void OnStateEnter()
    {
        _transform.rotation = _interactionManager.currentInteractiveObject.transform.rotation;
        _transform.position = _interactionManager.currentInteractiveObject.transform.position
							+ _interactionManager.currentInteractiveObject.transform.up * -0.3f
							+ _interactionManager.currentInteractiveObject.transform.forward * -1.5f;
    }

    public override void OnUpdate() { }
}
