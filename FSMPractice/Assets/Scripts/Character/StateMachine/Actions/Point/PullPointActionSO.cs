using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullPointAction", menuName = "State Machines/Actions/Pull Point Action")]

public class PullPointActionSO : StateActionSO<PullPointAction>
{
    public LayerMask PointLayerMask;
}

public class PullPointAction : StateAction
{
	protected new PullPointActionSO _originSO => (PullPointActionSO)base.OriginSO;


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
        _transform.position = _interactionManager.currentInteractiveObject.transform.position + _transform.up * 0.4f;
    }

    public override void OnUpdate() { }
}