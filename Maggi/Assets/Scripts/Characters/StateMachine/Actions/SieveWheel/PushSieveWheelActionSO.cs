using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PushSieveWheelAction", menuName = "State Machines/Actions/Push Sieve Wheel Action")]
public class PushSieveWheelActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new PushSieveWheelAction();
}

public class PushSieveWheelAction : StateAction
{
	protected new PushSieveWheelActionSO _originSO => (PushSieveWheelActionSO)base.OriginSO;
	private InteractionManager _interactionManager;



	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

    public override void OnStateExit()
    {
        _interactionManager.currentInteractionType = InteractionType.None;
        _interactionManager.currentInteractiveObject = null;
    }

    public override void OnUpdate() { }
}
