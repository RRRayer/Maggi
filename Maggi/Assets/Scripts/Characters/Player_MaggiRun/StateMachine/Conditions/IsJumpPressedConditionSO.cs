using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsJumpPressedCondition", menuName = "State Machines/Conditions/MaggiRun/Jump Pressed")]
public class IsJumpPressedConditionSO : StateConditionSO
{
    protected override Condition CreateCondition() => new IsJumpPressedCondition();
}

public class IsJumpPressedCondition : Condition
{
    private PlayerMaggiRun _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
    }

    protected override bool Statement()
    {
        return _player.jumpInput; // InputReader에서 받아서 true일 때만 점프
    }
}