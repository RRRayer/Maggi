using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RotateSieveWheelAction", menuName = "State Machines/Actions/Rotate Sieve Wheel Action")]
public class RotateSieveWheelActionSO : StateActionSO<RotateSieveWheelAction>
{
    public float RotationAngle;
}

public class RotateSieveWheelAction : StateAction
{
	private RotateSieveWheelActionSO _originSO => (RotateSieveWheelActionSO)base.OriginSO;

    private Player _player;
	private InteractiveObject _interactiveObject;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _interactiveObject = interactiveObject;
    }

    public override void OnUpdate()
    {
        _interactiveObject.transform.GetChild(0)
            .Rotate(Vector3.forward * _player.movementInput.x * _originSO.RotationAngle * Time.deltaTime);
    }
}
