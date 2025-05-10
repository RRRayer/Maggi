using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using System.Threading.Tasks;
using UnityEditor.Timeline.Actions;

[CreateAssetMenu(fileName = "CountInteraction", menuName = "State Machines/Actions/Count Interaction")]
public class CountInteractionActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new CountInteractionAction();
	[Tooltip("Number of taps to activate the generator")]
	public int TapsToActivateGenerator = 3;

	[Tooltip("Time in milliseconds to decrease the taps count")]
	public int TimeToDecreaseTaps = 1500;
	public VoidEventChannelSO OnGeneratorActivated;
	[HideInInspector] public static int _currentTaps = 0;
}

public class CountInteractionAction : StateAction
{
	protected new CountInteractionActionSO _originSO => (CountInteractionActionSO)base.OriginSO;

	public override void OnUpdate() { }
	public override void OnStateEnter()
	{
		CountInteractionActionSO._currentTaps++;
		if (CountInteractionActionSO._currentTaps >= _originSO.TapsToActivateGenerator)
			_originSO.OnGeneratorActivated?.RaiseEvent();

		Task.Delay(_originSO.TimeToDecreaseTaps).ContinueWith(t =>
		{
			CountInteractionActionSO._currentTaps--;
		});
		
		// TODO: Delete Log
		#if DEBUG
		Debug.Log($"Taps: {CountInteractionActionSO._currentTaps}");
		#endif
	}
}
