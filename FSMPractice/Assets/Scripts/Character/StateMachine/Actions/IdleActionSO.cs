using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IdleAction", menuName = "State Machines/Actions/IdleAction")]
public class IdleActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new IdleAction();
}

public class IdleAction : StateAction
{

	protected new IdleActionSO OriginSO => (IdleActionSO)base.OriginSO;

    // private Transform _transform;
    private InteractionManager _interactionManager;
    public override void Awake(StateMachine stateMachine)
    {
        // _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnStateEnter()
    {
		_interactionManager.currentInteractionType = InteractionType.None;
		_interactionManager.currentInteractiveObject = null;
    }
    public override void OnUpdate()
    {
        //Debug.Log("Idle Action");
        // Debug.Log(_transform.position);
    }
}
