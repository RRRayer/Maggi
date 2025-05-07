using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    public Vector3 MovementInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool RunPressed { get; private set; }

    private Vector2 _rawInput;

    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMove;
        _inputReader.JumpEvent += OnJump;
        _inputReader.JumpCancelEvent += OnJumpCancel;
        _inputReader.RunEvent += OnRun;
        _inputReader.RunCancelEvent += OnRunCancel;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMove;
        _inputReader.JumpEvent -= OnJump;
        _inputReader.JumpCancelEvent -= OnJumpCancel;
        _inputReader.RunEvent -= OnRun;
        _inputReader.RunCancelEvent -= OnRunCancel;
    }

    private void OnMove(Vector2 input)
    {
        _rawInput = input;
    }

    private void Update()
    {
        // World 방향 변환은 상태에서 할 수도 있음
        MovementInput = new Vector3(_rawInput.x, 0, _rawInput.y);
    }

    private void OnJump() => JumpPressed = true;
    private void OnJumpCancel() => JumpPressed = false;
    private void OnRun() => RunPressed = true;
    private void OnRunCancel() => RunPressed = false;
}
