using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

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
	private InteractiveObject _interactiveObject;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _transform = owner.GetComponent<Transform>();
        _interactiveObject = interactiveObject;
    }

    public override void OnUpdate()
    {
        _transform.RotateAround(_interactiveObject.transform.position,
                                _interactiveObject.transform.right,
                                _player.movementInput.x * _originSO.rotationSpeed);
    }
}

