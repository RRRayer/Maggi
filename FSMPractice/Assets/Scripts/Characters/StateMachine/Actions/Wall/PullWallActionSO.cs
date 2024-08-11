using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullWallAction", menuName = "State Machines/Actions/Pull Wall Action")]

public class PullWallActionSO : StateActionSO<PullWallAction>
{
    public float Speed = 4.0f;
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
        _transform.rotation = _interactionManager.currentInteractiveObject.transform.rotation;

        Ray ray = new Ray(_transform.position, -_transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2.0f, _originSO.WallLayerMask))
        {
            Vector3 wallNormal = hit.normal;

            Vector3 rightOnPlane = Vector3.Cross(Vector3.back, wallNormal).normalized;
            Vector3 forwardOnPlane = Vector3.Cross(Vector3.right, wallNormal).normalized;

            Vector3 newInputVector = new Vector3(_player.movementInput.x, 0, _player.movementInput.z);
            Vector3 newMovementVector = (newInputVector.x * rightOnPlane + newInputVector.z * forwardOnPlane).normalized;

            _player.movementVector = newMovementVector * _originSO.Speed;
        }
    }
}
