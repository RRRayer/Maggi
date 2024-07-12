using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "PullPointAction", menuName = "State Machines/Actions/Pull Point Action")]

public class PullPointActionSO : StateActionSO<PullPointAction>
{
    public LayerMask PointLayerMask;
}

public class PullPointAction : StateAction
{
	protected new PullPointActionSO _originSO => (PullPointActionSO)base.OriginSO;

	private Player _player;

    private Transform _transform;
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

	public override void OnUpdate()
	{
        Quaternion lookAt = Quaternion.identity;
        Vector3 lookatVec = (_interactionManager.currentInteractiveObject.transform.position - _transform.position).normalized;
        lookAt.SetLookRotation (lookatVec);
        _transform.rotation = lookAt;
		_transform.position = _interactionManager.currentInteractiveObject.transform.position - _transform.forward * 1.0f;


	}
}