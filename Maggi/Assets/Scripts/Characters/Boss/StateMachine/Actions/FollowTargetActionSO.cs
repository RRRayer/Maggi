using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using Maggi.Character.Boss;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "FollowTargetAction", menuName = "State Machines/Actions/Boss/Follow Target Action")]
public class FollowTargetActionSO : StateActionSO<FollowTargetAction> 
{
    public float speed = 3.0f;
}

public class FollowTargetAction : StateAction
{
	protected new FollowTargetActionSO _originSO => (FollowTargetActionSO)base.OriginSO;

    private Boss _boss;
    private NavMeshAgent _agent;

	public override void Awake(StateMachine stateMachine)
	{
        _boss = stateMachine.GetComponent<Boss>();
        _agent = stateMachine.GetComponent<NavMeshAgent>();
	}

    public override void OnStateEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }

    public override void OnUpdate()
	{
        Vector3 currentPos = _boss.hand.transform.position;
        Vector3 targetPos = _boss.Target.position;

        _boss.hand.transform.position = Vector3.Lerp(currentPos, targetPos, _originSO.speed * Time.deltaTime);

        float dist = Vector3.Distance(_boss.hand.transform.position, _boss.Target.position);
    }
	
	public override void OnStateExit()
	{
	}
}
