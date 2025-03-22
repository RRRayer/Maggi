using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using Maggi.Character.Boss;

[CreateAssetMenu(fileName = "IsCatchCondition", menuName = "State Machines/Conditions/Boss/Is Catch Condition", order = 0)]
public class IsCatchConditionSO : StateConditionSO<IsCatchCondition> { }

public class IsCatchCondition : Condition
{
	private Boss _boss;

	public override void Awake(StateMachine stateMachine)
	{
        _boss = stateMachine.GetComponent<Boss>();
    }
	
	protected override bool Statement()
	{
		return _boss.CurrentMode == Mode.Catch;
	}
}
