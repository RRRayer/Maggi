using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;
using static UnityEditor.Experimental.GraphView.GraphView;

[CreateAssetMenu(fileName = "isPullingHeavyCondition", menuName = "State Machines/Conditions/is Pulling Heavy Condition")]
public class isPullingHeavyConditionSO : StateConditionSO<isPullingHeavyCondition> { }

public class isPullingHeavyCondition : Condition
{
    private Player _player;
    private InteractionManager _interactionManager;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    protected override bool Statement()
    {
        if (_interactionManager.currentInteractionType == InteractionType.Heavy)
        {
            Vector3 currentInteractiveObjectPosition = _interactionManager.currentInteractiveObject.transform.position;
            Vector3 distanceVector = currentInteractiveObjectPosition - _player.transform.position;
            float interactiveObjectSize = _interactionManager.currentInteractiveObject.transform.localScale.x; // 박스의 한 변의 길이의 절반
            float distance = interactiveObjectSize / 2 + _player.transform.localScale.x - 0.2f;

            if (distanceVector.y < interactiveObjectSize / 2 && distanceVector.y > -interactiveObjectSize / 2) // 위나 아래
            {
                Debug.Log(distanceVector.y);
                Debug.Log(interactiveObjectSize / 2);
                return true;
            }
        }
        return false;
    }
}
