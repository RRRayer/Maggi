using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RideSieveWheelAction", menuName = "State Machines/Actions/Ride Sieve Wheel Action")]
public class RideSieveWheelActionSO : StateActionSO<RideSieveWheelAction> { }

public class RideSieveWheelAction : StateAction
{
	private RideSieveWheelActionSO _originSO => (RideSieveWheelActionSO)base.OriginSO;

    private Transform _transform;
	private InteractiveObject _interactiveObject;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _transform = owner.GetComponent<Transform>();
        _interactiveObject = interactiveObject;
    }

    public override void OnStateEnter()
    {
        _transform.rotation = _interactiveObject.transform.rotation;
        _transform.position = _interactiveObject.transform.position + _interactiveObject.transform.up * -0.3f;
    }

    public override void OnUpdate() { }
}
