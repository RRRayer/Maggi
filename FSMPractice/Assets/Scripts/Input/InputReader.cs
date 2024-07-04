using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    // GamePlay
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction JumpCancelEvent = delegate { };
    public event UnityAction PullEvent = delegate { };
    public event UnityAction PushEvent = delegate { };
    public event UnityAction PushCancelEvent = delegate { };

    private GameInput _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            _gameInput.Gameplay.SetCallbacks(this);
            _gameInput.Gameplay.Enable();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    void GameInput.IGameplayActions.OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            JumpEvent.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            JumpCancelEvent.Invoke();
    }

    public void OnPull(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) 
            PullEvent.Invoke();
    }

    public void OnPush(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            PushEvent.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            PushCancelEvent.Invoke();
    }
}
