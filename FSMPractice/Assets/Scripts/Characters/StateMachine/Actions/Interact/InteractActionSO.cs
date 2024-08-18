using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "InteractAction", menuName = "State Machines/Actions/Interact Action")]
public class InteractActionSO : StateActionSO<InteractAction> { }

public class InteractAction : StateAction
{
    private InteractionManager _interactionManager;

    public override void Awake(StateMachine stateMachine)
    {
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnUpdate() { }
	
	public override void OnStateEnter()
	{
        Debug.Log("Interact");

        if (_interactionManager.currentInteractiveObject.TryGetComponent(out InteractionEventListener listener))
        {
			listener.OnInteract();
        }
    }
}
