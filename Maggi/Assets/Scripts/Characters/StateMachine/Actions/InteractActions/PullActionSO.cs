using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;
using static PullActionSO;

[CreateAssetMenu(fileName = "PullAction", menuName = "State Machines/Actions/Pull Action")]
public class PullActionSO : StateActionSO
{
    public Locomotion state;

    protected override StateAction CreateAction() => new PullAction();

    public enum Locomotion
    {
        Idle, Walk, JumpAscending, JumpDescending
    };
}

public class PullAction : StateAction
{
    protected new PullActionSO _originSO => (PullActionSO)base.OriginSO;
    private InteractionManager _interactionManager;
	private InteractiveObject _interactiveObjectScript;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnStateEnter()
    {
        _interactiveObjectScript = _interactionManager.currentInteractiveObject.GetComponent<InteractiveObject>();

        Debug.Log($"current state = {_originSO.state}");

        if (_interactiveObjectScript != null)
        {
            switch (_originSO.state)
            {
            case Locomotion.Idle:
                _interactiveObjectScript.OnIdleStateEnter();
                break;
            case Locomotion.Walk:
                _interactiveObjectScript.OnWalkStateEnter();
                break;
            case Locomotion.JumpAscending:
                _interactiveObjectScript.OnJumpAscendingStateEnter();
                break;
            case Locomotion.JumpDescending:
                _interactiveObjectScript.OnJumpDescendingStateEnter();
                break;
            default:
                Debug.Log("State not handled");
                break;
            }
        }
        else
        {
            Debug.Log("There is no InteractiveObject Scripts");
        }
    }

    public override void OnUpdate()
    {
        if (_interactiveObjectScript != null)
        {
            switch (_originSO.state)
            {
            case Locomotion.Idle:
                _interactiveObjectScript.OnIdleUpdate();
                break;
            case Locomotion.Walk:
                _interactiveObjectScript.OnWalkUpdate();
                break;
            case Locomotion.JumpAscending:
                _interactiveObjectScript.OnJumpAscendingUpdate();
                break;
            case Locomotion.JumpDescending:
                _interactiveObjectScript.OnJumpDescendingUpdate();
                break;
            default:
                Debug.Log("State not handled");
                break;
            }
        }
        else
        {
            Debug.Log("There is no InteractiveObject Scripts");
        }
    }

    public override void OnStateExit()
    {
        if (_interactiveObjectScript != null)
        {
            switch (_originSO.state)
            {
            case Locomotion.Idle:
                _interactiveObjectScript.OnIdleStateExit();
                break;
            case Locomotion.Walk:
                _interactiveObjectScript.OnWalkStateExit();
                break;
            case Locomotion.JumpAscending:
                _interactiveObjectScript.OnJumpAscendingStateExit();
                break;
            case Locomotion.JumpDescending:
                _interactiveObjectScript.OnJumpDescendingStateExit();
                break;
            default:
                Debug.Log("State not handled");
                break;
            }
        }
        else
        {
            Debug.Log("There is no InteractiveObject Scripts");
        }
    }
}
