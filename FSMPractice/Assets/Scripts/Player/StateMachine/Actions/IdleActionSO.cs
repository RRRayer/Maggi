using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IdleAction", menuName = "State Machines/Actions/IdleAction")]
public class IdleActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new IdleAction();
}

public class IdleAction : StateAction
{
	protected new IdleActionSO OriginSO => (IdleActionSO)base.OriginSO;

    public override void OnUpdate()
    {
        //Debug.Log("Idle Action");
    }
}
