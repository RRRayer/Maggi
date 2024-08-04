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
            Vector3 playerPosition = _player.transform.position;

            // 플레이어와 박스 사이의 벡터
            Vector3 distanceVector = currentInteractiveObjectPosition - playerPosition;

            // 박스 콜라이더의 크기와 위치 정보를 가져옴
            BoxCollider boxCollider = _interactionManager.currentInteractiveObject.GetComponent<BoxCollider>();
            Vector3 boxSize = boxCollider.size;

            // y 성분이 특정 임계값 이상인 경우 위나 아래에서 상호작용하는 것으로 판단하여 아무 동작도 하지 않음
            float yThreshold = boxSize.y / 2 - 0.1f; // 임계값 설정

            if (Mathf.Abs(distanceVector.y) < yThreshold)
            {
                return true;
            }
        }

        _interactionManager.currentInteractionType = InteractionType.None;
        _interactionManager.currentInteractiveObject = null;

        return false;
    }
}
