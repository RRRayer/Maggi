using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullGlobeAction", menuName = "State Machines/Actions/Pull Globe Action")]
public class PullGlobeActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new PullGlobeAction();
}

public class PullGlobeAction : StateAction
{
	protected new PullGlobeActionSO _originSO => (PullGlobeActionSO)base.OriginSO;

    private Transform _transform;
	private InteractionManager _interactionManager;

	public override void Awake(StateMachine stateMachine)
	{
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
	}

  public override void OnStateEnter()
  {
    _transform.LookAt(_interactionManager.transform.position);
  }

  public override void OnUpdate() {}

}
