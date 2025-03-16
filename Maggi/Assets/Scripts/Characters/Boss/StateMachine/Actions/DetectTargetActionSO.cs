using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using UnityEngine.AI;

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
    }
}
