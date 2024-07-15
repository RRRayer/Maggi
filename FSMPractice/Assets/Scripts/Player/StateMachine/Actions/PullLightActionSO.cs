using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullLightAction", menuName = "State Machines/Actions/PullLightAction")]
public class PullLightActionSO : StateActionSO<PullLightAction> 
{
    public LayerMask mouseClickLayerMask;
}

public class PullLightAction : StateAction
{
	protected new PullLightActionSO _originSO => (PullLightActionSO)base.OriginSO;
	private Player _player;
	private InteractionManager _interactionManager;
    private Collider _interactiveObjectCollider;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnStateEnter()
    {
        _interactiveObjectCollider = _interactionManager.currentInteractiveObject.GetComponent<Collider>();
        _interactiveObjectCollider.enabled = false;

        // When Pulling Light Object, it needs to remove from list of Potential Interactions
        _interactionManager.OnTriggerChangeDetected(false, _interactionManager.currentInteractiveObject);
    }

    public override void OnUpdate()
	{
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _originSO.mouseClickLayerMask))
        {
            Vector3 mousePosition = hit.point;
            mousePosition.y = _player.transform.position.y;

            Vector3 direction = (mousePosition - _player.transform.position).normalized;
            _interactionManager.currentInteractiveObject.transform.position = _player.transform.position + direction * 1.0f;

            _interactionManager.currentInteractiveObject.transform.LookAt(mousePosition);
        }
    }
    public override void OnStateExit()
    {
        _interactiveObjectCollider.enabled = true;
    }
}
