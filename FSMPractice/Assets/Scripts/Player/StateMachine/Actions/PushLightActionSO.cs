using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PushLightAction", menuName = "State Machines/Actions/Push Light Action")]
public class PushLightActionSO : StateActionSO<PushLightAction> 
{
	public float pushForce = 5.0f;
	public float pushHeight = 0.5f;
	public LayerMask floorLayerMask;
}

public class PushLightAction : StateAction
{
	protected new PushLightActionSO _originSO => (PushLightActionSO)base.OriginSO;
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
            _interactiveObjectRigidbody.isKinematic = false;
            _interactiveObjectRigidbody.velocity = _interactiveObjectRigidbody.transform.forward * _originSO.pushForce;
        }
		else
		{
			Debug.Log("Push할 오브젝트가 없음");
		}
	}
	
	public override void OnStateExit()
	{
		_interactionManager.currentInteractionType = InteractionType.None;
		_interactionManager.currentInteractiveObject = null;
	}

    public override void OnUpdate()
    {
		
    }
}
