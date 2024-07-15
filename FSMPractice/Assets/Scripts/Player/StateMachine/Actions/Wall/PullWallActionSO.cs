using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "PullWallAction", menuName = "State Machines/Actions/Pull Wall Action")]

public class PullWallActionSO : StateActionSO<PullWallAction>
{
    public LayerMask WallLayerMask;
}

public class PullWallAction : StateAction
{
	protected new PullWallActionSO _originSO => (PullWallActionSO)base.OriginSO;
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
		// Ray ray = new Ray(_transform.position, Vector3.forward);
		// RaycastHit slopeHit;
		// if(Physics.Raycast(ray, out slopeHit, Mathf.Infinity, _originSO.WallLayerMask)) {
		// 	Quaternion rot = Quaternion.FromToRotation(Vector3.up, slopeHit.normal);
		// 	Debug.Log(rot);
		// 	_transform.rotation = rot;
		// }

        Quaternion x = _interactionManager.currentInteractiveObject.transform.rotation;
        _transform.rotation = x;
	}
}
