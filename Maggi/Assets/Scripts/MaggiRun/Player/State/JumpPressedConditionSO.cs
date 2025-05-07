using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "JumpPressedCondition", menuName = "State Machines/Conditions/Jump Pressed")]
public class JumpPressedConditionSO : StateConditionSO
{
    protected override Condition CreateCondition() => new JumpPressedCondition();
}

public class JumpPressedCondition : Condition
{
    private PlayerController2 _player;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerController2>();
    }

    protected override bool Statement() => _player.JumpPressed;
}