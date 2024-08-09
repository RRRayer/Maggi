using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "VerticalMoveAction", menuName = "State Machines/Actions/Vertical Move Action")]
public class VerticalMoveActionSO : StateActionSO<VerticalMoveAction>
{
	public float speed = 4.0f;
    public LayerMask floorLayerMask;

}

public class VerticalMoveAction : StateAction
{
	private VerticalMoveActionSO _originSO => (VerticalMoveActionSO)base.OriginSO;
    private Player _player;
    private Transform _transform;


    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
    }

    public override void OnUpdate()
    {
        Vector3 originalVector = _player.movementVector;
        _player.movementVector = new Vector3(originalVector);
    }
}
