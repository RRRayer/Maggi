using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RotateSieveWheelAction", menuName = "State Machines/Actions/Rotate Sieve Wheel Action")]
public class RotateSieveWheelActionSO : StateActionSO<RotateSieveWheelAction>
{
    [SerializeField]
    public float RotationAngle;
}

public class RotateSieveWheelAction : StateAction
{
	protected new RotateSieveWheelActionSO _originSO => (RotateSieveWheelActionSO)base.OriginSO;

    private Player _player;
	private InteractionManager _interactionManager;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnUpdate()
    {
        _interactionManager.currentInteractiveObject.transform.GetChild(0).Rotate(Vector3.forward * _player.movementInput.x * _originSO.RotationAngle * Time.deltaTime);
    }
}
