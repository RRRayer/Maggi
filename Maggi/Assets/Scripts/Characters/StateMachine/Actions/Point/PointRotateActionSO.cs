using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PointRotateAction", menuName = "State Machines/Actions/Point Rotate Action")]
public class PointRotateActionSO : StateActionSO<PointRotateAction>
{
    public LayerMask PointLayerMask;
    public float rotationSpeed = 2.0f;
}

public class PointRotateAction : StateAction
{
	protected new PointRotateActionSO _originSO => (PointRotateActionSO)base.OriginSO;

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
        _transform.RotateAround(_interactionManager.currentInteractiveObject.transform.position,
                                _interactionManager.currentInteractiveObject.transform.right,
                                _player.movementInput.x * _originSO.rotationSpeed);
    }
}

