using UnityEngine;
using Pudding.StateMachine.ScriptableObjects;
using Pudding.StateMachine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Started Moving")]
public class IsMovingConditionSO : StateConditionSO<IsMovingCondition>
{
    public float treshold = 0.02f;
}

public class IsMovingCondition : Condition
{
    private IsMovingConditionSO _originSO => (IsMovingConditionSO)base.OriginSO;

    private Player _player;


    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
    }

    protected override bool Statement()
    {
        Vector3 movementVector = _player.movementInput;
        movementVector.y = 0;
        return movementVector.sqrMagnitude > _originSO.treshold;        
    }
}


