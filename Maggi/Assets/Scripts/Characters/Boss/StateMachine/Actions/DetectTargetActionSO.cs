using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using UnityEngine.AI;
using Maggi.Character.Boss;

[CreateAssetMenu(fileName = "DetectTargetAction", menuName = "State Machines/Actions/Boss/Detect Target Action")]
public class DetectTargetActionSO : StateActionSO<DetectTargetAction> { }

public class DetectTargetAction : StateAction
{
    private Boss _boss;
    private NavMeshAgent _agent;

    public override void Awake(StateMachine stateMachine)
    {
        _boss = stateMachine.GetComponent<Boss>();
        _agent = stateMachine.GetComponent<NavMeshAgent>();
    }

    public override void OnUpdate()
    {
        _agent.SetDestination(_boss.Target.position);

        // 목적지까지 갔다면 Timeline 실행.
        if (_boss.IsStopped())
        {
            _boss.SetMode(Mode.Trigger, "detect target action.cs");
        }  
    }

    public override void OnStateExit()
    {
        Debug.Log("detect 나옴");
    }
}
