using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "IsBoundOfWallCondition", menuName = "State Machines/Conditions/Is Bound Of Wall Condition")]
public class IsBoundOfWallConditionSO : StateConditionSO<IsBoundOfWallCondition>
{
	public LayerMask WallLayerMask;
}

public class IsBoundOfWallCondition : Condition
{
	private IsBoundOfWallConditionSO _originSO => (IsBoundOfWallConditionSO)base.OriginSO;
    private InteractionManager _interactionManager;
    private Player _player;
	private Transform _transform;

	public override void Awake(StateMachine stateMachine)
	{
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
        _player = stateMachine.GetComponent<Player>();
		_transform = _player.transform;
	}
	
	protected override bool Statement()
	{
        Ray ray = new Ray(_transform.position, -_transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2.0f, _originSO.WallLayerMask))
            return true;


        // Out of Boundary
        _interactionManager.currentInteractionType = InteractionType.None;
        _interactionManager.currentInteractiveObject = null;
        return false;
    }
}
