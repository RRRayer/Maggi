using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReaderMaggiRun", menuName = "Game/MaggiRun/Input Reader")]
public class InputReaderMaggiRun : ScriptableObject, GameInput.IGameplayActions
{
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction JumpCancelEvent = delegate { };
    public event UnityAction AttackEvent = delegate { };
    public event UnityAction AttackCancelEvent = delegate { };

    private GameInput _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.Gameplay.SetCallbacks(this);
        }

        _gameInput.Gameplay.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) JumpEvent.Invoke();
        else if (context.canceled) JumpCancelEvent.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
            if (context.phase == InputActionPhase.Performed)
                AttackEvent.Invoke();
            else if (context.phase == InputActionPhase.Canceled)
                AttackCancelEvent.Invoke();
    }

    public void EnableGameplayInput()
    {
        _gameInput.Gameplay.Enable();
    }

    public void DisableAllInput()
    {
        _gameInput.Gameplay.Disable();
    }

    public void OnPush(InputAction.CallbackContext context) { }
    public void OnPull(InputAction.CallbackContext context) { }
    public void OnPause(InputAction.CallbackContext context) { }
    public void OnRun(InputAction.CallbackContext context) { }
    public void OnAim(InputAction.CallbackContext context) { }

}
