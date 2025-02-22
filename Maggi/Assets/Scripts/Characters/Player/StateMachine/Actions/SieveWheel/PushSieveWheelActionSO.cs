using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PushSieveWheelAction", menuName = "State Machines/Actions/Push Sieve Wheel Action")]
public class PushSieveWheelActionSO : StateActionSO<PushSieveWheelAction> { }

public class PushSieveWheelAction : StateAction
{
	private InteractionManager _interactionManager;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _interactionManager = owner.GetComponent<InteractionManager>();
    }

    public override void OnStateExit()
    {
        _interactionManager.currentInteractionType = InteractionType.None;
        _interactionManager.currentInteractiveObject = null;
    }

    public override void OnUpdate() { }
}
