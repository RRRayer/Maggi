using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using static LocomotionActionSO;

[CreateAssetMenu(fileName = "PullAction", menuName = "State Machines/Actions/Pull Action")]
public class LocomotionActionSO : StateActionSO
{
    public Locomotion state;

    protected override StateAction CreateAction() => new PullAction();

    public enum Locomotion
    {
        Idle, Walk, JumpAscending, JumpDescending, Pull, Push
    };
}

public class PullAction : StateAction
{
    private LocomotionActionSO _originSO => (LocomotionActionSO)base.OriginSO;
    private InteractionManager _interactionManager;
	private InteractiveObject _interactiveObjectScript;
    private GameObject _owner;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
        _owner = stateMachine.gameObject;
    }

    public override void OnStateEnter()
    {
        if (!_interactionManager.currentInteractiveObject.TryGetComponent(out _interactiveObjectScript))
            return;
        
        //_interactiveObjectScript = _interactionManager.currentInteractiveObject.GetComponent<InteractiveObject>();
        _interactiveObjectScript.Init(_owner);

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
            case Locomotion.Pull:
                _interactiveObjectScript.OnPullStateEnter();
                break;
            case Locomotion.Push:
                _interactiveObjectScript.OnPushStateEnter();
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
            case Locomotion.Pull:
                _interactiveObjectScript.OnPullUpdate();
                break;
            case Locomotion.Push:
                _interactiveObjectScript.OnPushUpdate();
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
            case Locomotion.Pull:
                _interactiveObjectScript.OnPullStateExit();
                break;
            case Locomotion.Push:
                _interactiveObjectScript.OnPushStateExit();
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
