using UnityEngine;
using Test.StateMachine;
using Test.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsBoundOfWallCondition", menuName = "State Machines/Conditions/Is Bound Of Wall Condition")]
public class IsBoundOfWallConditionSO : StateConditionSO<IsBoundOfWallCondition> {}

public class IsBoundOfWallCondition : Condition
{
	protected new IsBoundOfWallConditionSO OriginSO => (IsBoundOfWallConditionSO)base.OriginSO;
	private InteractionManager _interactionManager;
	private Player _player;


	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
		_player = stateMachine.GetComponent<Player>();
	}
	
	protected override bool Statement()
	{
		GameObject wall = _interactionManager.currentInteractiveObject;
		float ymin = wall.GetComponent<Collider>().bounds.min.y;
		float ymax = wall.GetComponent<Collider>().bounds.max.y;
		float playerY = _player.transform.position.y;

		if (playerY < ymin || playerY > ymax) {
			Debug.Log("boundary over");
			return true;
		}
		return false;
	}
}
