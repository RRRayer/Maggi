using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RideSieveWheelAction", menuName = "State Machines/Actions/Ride Sieve Wheel Action")]
public class RideSieveWheelActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new RideSieveWheelAction();
}

public class RideSieveWheelAction : StateAction
{
	protected new RideSieveWheelActionSO _originSO => (RideSieveWheelActionSO)base.OriginSO;

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
							+ _interactionManager.currentInteractiveObject.transform.up * -0.3f;
    }

    public override void OnUpdate() { }
}
