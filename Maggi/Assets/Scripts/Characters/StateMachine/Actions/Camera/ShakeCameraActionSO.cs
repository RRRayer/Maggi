using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ShakeCameraAction", menuName = "State Machines/Actions/Shake Camera Action")]
public class ShakeCameraActionSO : StateActionSO
{
	public VoidEventChannelSO CameraShakeEvent;
	protected override StateAction CreateAction() => new ShakeCameraAction();
}

public class ShakeCameraAction : StateAction
{
	private ShakeCameraActionSO _originSO => (ShakeCameraActionSO)base.OriginSO;

	public override void OnUpdate() { }
	
	public override void OnStateEnter()
	{
		_originSO.CameraShakeEvent.RaiseEvent();
		// Player 와의 거리가 짧아지면 더 많이 흔들어야 함
		// Void Event 에서 Float Event로 바꾸든가
	}
}
