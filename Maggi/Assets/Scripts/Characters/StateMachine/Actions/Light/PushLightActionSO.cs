﻿using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PushLightAction", menuName = "State Machines/Actions/Push Light Action")]
public class PushLightActionSO : StateActionSO<PushLightAction> 
{
	public float pushForce = 2.0f;
	public float pushHeight = 2.0f;
}

public class PushLightAction : StateAction
{
	private PushLightActionSO _originSO => (PushLightActionSO)base.OriginSO;
	private InteractionManager _interactionManager;
	private Rigidbody _interactiveObjectRigidbody;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

	public override void OnStateEnter()
	{
		if (_interactionManager.currentInteractiveObject != null)
		{
			_interactiveObjectRigidbody = _interactionManager.currentInteractiveObject.GetComponent<Rigidbody>();

			// Init Position to Player position and Add
			_interactiveObjectRigidbody.transform.position = _interactionManager.transform.position + _interactiveObjectRigidbody.transform.forward * 0.2f;
			_interactiveObjectRigidbody.velocity = _interactiveObjectRigidbody.transform.forward * _originSO.pushForce + _interactiveObjectRigidbody.transform.up * _originSO.pushHeight;
		}
	}

    public override void OnStateExit()
    {
        _interactionManager.currentInteractionType = InteractionType.None;
        _interactionManager.currentInteractiveObject = null;
    }

    public override void OnUpdate() { }
}
