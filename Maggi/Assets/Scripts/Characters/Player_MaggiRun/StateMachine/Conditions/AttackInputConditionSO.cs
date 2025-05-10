using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/MaggiRun/Attack Input")]
public class AttackInputConditionSO : StateConditionSO<AttackInputCondition> { }

public class AttackInputCondition : Condition
{
    private InputReaderMaggiRun _input;
    private bool _attackTriggered = false;

    public override void Awake(StateMachine stateMachine)
    {
        _input = stateMachine.GetComponent<PlayerMaggiRun>().InputReader as InputReaderMaggiRun;
        if (_input != null)
        {
            _input.AttackEvent += OnAttack;
        }
    }

    public override void OnStateExit()
    {
        _attackTriggered = false;
    }

    protected override bool Statement() => _attackTriggered;

    private void OnAttack() => _attackTriggered = true;
}