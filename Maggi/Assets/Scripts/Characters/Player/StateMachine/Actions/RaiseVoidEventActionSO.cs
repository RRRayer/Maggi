using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RaiseVoidEventAction", menuName = "State Machines/Actions/Raise Void Event Action")]
public class RaiseVoidEventActionSO : StateActionSO
{
	public VoidEventChannelSO VoidChannel;
	protected override StateAction CreateAction() => new RaiseVoidEventAction();
}

public class RaiseVoidEventAction : StateAction
{
	private RaiseVoidEventActionSO _originSO => (RaiseVoidEventActionSO)base.OriginSO;
	private VoidEventChannelSO _voidEvent;

	public override void Awake(StateMachine stateMachine)
	{
		_voidEvent = _originSO.VoidChannel;
	}

    public override void OnStateEnter()
    {
        _voidEvent.RaiseEvent();
    }

    public override void OnUpdate() { }
}
