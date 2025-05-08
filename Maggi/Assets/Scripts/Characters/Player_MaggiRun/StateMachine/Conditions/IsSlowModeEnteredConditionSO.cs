using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsSlowModeEnteredCondition", menuName = "State Machines/Conditions/MaggiRun/Is Slow Mode Entered")]
public class IsSlowModeEnteredConditionSO : StateConditionSO<IsSlowModeEnteredCondition> { }

public class IsSlowModeEnteredCondition : Condition
{
    private PlayerMaggiRun _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<PlayerMaggiRun>();
    }

    protected override bool Statement()
    {
        return _player.InSlowMode;
    }
}