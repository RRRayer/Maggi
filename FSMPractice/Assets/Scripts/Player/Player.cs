using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;

    private Vector2 _inputVector; // _inputVector.x : x movement, _inputVector.y : z movement
    private float _previousSpeed;

    //These fields are read and manipulated by the StateMachine actions
    public Vector3 movementInput;
    public Vector3 movementVector;
    public bool jumpInput;

    private float speed = 2.0f;

    public const float AIR_RESISTANCE = 5f;
    public const float MAX_FALL_SPEED = -50f;
    public const float MAX_RISE_SPEED = 100f;
    public const float GRAVITY_MULTIPLIER = 5f;
    public const float GRAVITY_COMEBACK_MULTIPLIER = 0.06f;
    public const float GRAVITY_DIVIDER = 0.6f;

    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMovement;
        _inputReader.JumpEvent += OnJumpInitiated;
        _inputReader.JumpCancelEvent += OnJumpCancelInitiated;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMovement;
    }

    private void Update()
    {
        RecalculateMovement();
    }

    private void RecalculateMovement()
    {
        float targetSpeed;
        Vector3 adjustedMovement;

        adjustedMovement = new Vector3(_inputVector.x, 0.0f, _inputVector.y);

        targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
        targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4.0f);

        if (_inputVector.sqrMagnitude == 0.0f)
            adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

        movementInput = adjustedMovement * targetSpeed;

        _previousSpeed = targetSpeed;
    }

    /* --- Event Listener --- */

    private void OnMovement(Vector2 movement)
    {
        _inputVector = movement;
    }

    private void OnJumpInitiated()
    {
        jumpInput = true;
    }

    private void OnJumpCancelInitiated()
    {
        jumpInput = false;
    }
}
