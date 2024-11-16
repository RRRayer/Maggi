namespace Maggi.StateMachine
{
    public interface IStateComponent
    {
        void OnStateEnter();
        void OnStateExit();
    }
}


