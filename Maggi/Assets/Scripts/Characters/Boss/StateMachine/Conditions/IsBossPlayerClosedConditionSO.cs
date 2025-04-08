using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using Maggi.Character.Boss;

[CreateAssetMenu(fileName = "IsBossPlayerClosedCondition", menuName = "State Machines/Conditions/Boss/Is Boss Player Closed Condition")]
public class IsBossPlayerClosedConditionSO : StateConditionSO
{
    public float distance = 10.0f;

	protected override Condition CreateCondition() => new IsBossPlayerClosedCondition();
}

public class IsBossPlayerClosedCondition : Condition
{
	protected new IsBossPlayerClosedConditionSO _originSO => (IsBossPlayerClosedConditionSO)base.OriginSO;
    private Boss _boss = default;

	public override void Awake(StateMachine stateMachine)
	{
        _boss = stateMachine.GetComponent<Boss>();
	}
	
	protected override bool Statement()
	{
        float dist = Vector3.Distance(_boss.transform.position, _boss.Target.position);
        Debug.Log($"player와 boss 간의 거리 = {dist}");
        return dist < _originSO.distance;
    }
}
