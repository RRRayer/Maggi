using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private TransformAnchor _gameplayCameraTransform = default;

    //These fields are read and manipulated by the StateMachine actions
    [HideInInspector] public Vector3 movementInput;
    [HideInInspector] public Vector3 movementVector;
    [HideInInspector] public bool jumpInput;

    private Vector2 _inputVector; // _inputVector.x : x movement, _inputVector.y : z movement
    private float _previousSpeed;

    public const float AIR_RESISTANCE = 5f;
    public const float MAX_FALL_SPEED = -50f;
    public const float MAX_RISE_SPEED = 100f;
    public const float GRAVITY_MULTIPLIER = 2f;
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
        _inputReader.JumpEvent -= OnJumpInitiated;
        _inputReader.JumpCancelEvent -= OnJumpCancelInitiated;
    }

    private void Update()
    {
        RecalculateMovement();
    }

    private void RecalculateMovement()
    {
        float targetSpeed;
        Vector3 adjustedMovement;

        if (_gameplayCameraTransform.isSet)
        {
            Vector3 cameraForward = _gameplayCameraTransform.Value.forward;
            cameraForward.y = .0f;
            Vector3 cameraRight = _gameplayCameraTransform.Value.right;
            cameraRight.y = .0f;

            adjustedMovement = cameraRight.normalized * _inputVector.x + cameraForward.normalized * _inputVector.y;
        }
        else
        {
            Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
            adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);
        }

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

    /* --- Animation Clip Functions --- */

}
