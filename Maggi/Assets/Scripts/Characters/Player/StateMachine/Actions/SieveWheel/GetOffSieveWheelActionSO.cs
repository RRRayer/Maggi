using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GetOffSieveWheelAction", menuName = "State Machines/Actions/Get Off Sieve Wheel Action")]
public class GetOffSieveWheelActionSO : StateActionSO<GetOffSieveWheelAction> { }

public class GetOffSieveWheelAction : StateAction
{
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
        _transform.position = _interactiveObject.transform.position
							+ _interactiveObject.transform.up * -0.3f
							+ _interactiveObject.transform.forward * -1.5f;
    }

    public override void OnUpdate() { }
}
