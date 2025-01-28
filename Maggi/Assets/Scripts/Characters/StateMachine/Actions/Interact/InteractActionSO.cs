using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InteractAction", menuName = "State Machines/Actions/Interact Action")]
public class InteractActionSO : StateActionSO<InteractAction> { }

public class InteractAction : StateAction
{
    private InteractionManager _interactionManager;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _interactionManager = owner.GetComponent<InteractionManager>();
    }

    public override void OnUpdate() { }

    public override void OnStateEnter()
    {
        if (_interactionManager.currentInteractiveObject.TryGetComponent<InteractionEventListener>(out var e))
        {
            List<InteractionEventListener> listeners = new List<InteractionEventListener>(_interactionManager.currentInteractiveObject.GetComponents<InteractionEventListener>());
            Debug.Log(listeners.Count);
            foreach (var listener in listeners)
            {
                listener.OnInteract();
            }
        }
    }
}