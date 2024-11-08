using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullAction", menuName = "State Machines/Actions/Pull Action")]
public class PullActionSO : StateActionSO<PullAction> { }

public class PullAction : StateAction
{
	private InteractionManager _interactionManager;
	private InteractiveObject _interactiveObjectScript;

	public override void Awake(StateMachine stateMachine)
	{
		_interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnStateEnter()
    {
        _interactiveObjectScript = _interactionManager.currentInteractiveObject.GetComponent<InteractiveObject>();

        if (_interactiveObjectScript != null)
		{
            _interactiveObjectScript.OnStateEnter();
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
            _interactiveObjectScript.OnUpdate();
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
            _interactiveObjectScript.OnStateExit();
        }
        else
        {
            Debug.Log("There is no InteractiveObject Scripts");
        }
    }
}
