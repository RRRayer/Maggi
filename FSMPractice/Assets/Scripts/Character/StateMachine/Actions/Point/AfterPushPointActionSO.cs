using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "AfterPushPointAction", menuName = "State Machines/Actions/After Push PointAction")]
public class AfterPushPointActionSO : StateActionSO<AfterPushPointAction> { }

public class AfterPushPointAction : StateAction
{
	public override void OnUpdate() { }
}
