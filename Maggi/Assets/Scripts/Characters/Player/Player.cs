using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private TransformAnchor _gameplayCameraTransform = default;

    //These fields are read and manipulated by the StateMachine actions
    /*[HideInInspector]*/ public Vector3 movementInput;
    /*[HideInInspector]*/ public Vector3 movementVector;
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

        // �÷��̾� �������� ī�޶� ��(front), ��(back), ������(right), ����(left) �� ��� �ִ°�?
        Vector3 directionToCamera = Vector3.zero;
        if (_gameplayCameraTransform.isSet)
        {
            directionToCamera = (_gameplayCameraTransform.Value.position - transform.position).normalized;
        }
        else
        {
            Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
        }

        // �Ǻ��� ���� �÷��̾��� forward, right ���Ϳ� dot product ��
        // dotF : ī�޶� ����(+) ������ �ĸ�(-) ������ �Ǵ�
        // dotR : ī�޶� ������(+) ������ ����(-) ������ �Ǵ�
        float dotF = Vector3.Dot(directionToCamera, Vector3.forward);
        float dotR = Vector3.Dot(directionToCamera, Vector3.right);

        // ���� ū ������ �������� "��/��" �Ǵ� "������/����"�� ����
        // �׸��� �׿� ���� cameraForward / cameraRight�� ������ �� �ϳ��� ����
        Vector3 cameraForward = Vector3.zero;
        Vector3 cameraRight = Vector3.zero;

        // �յڿ� �¿� ����
        if (Mathf.Abs(dotF) >= Mathf.Abs(dotR))
        {
            // �յ� ������ �켼
            if (dotF >= 0f)
            {
                // ī�޶� �÷��̾� "��" �ʿ� �ִ� ���
                // forward �Է� => back ���, right �Է� => left ���
                cameraForward = Vector3.back;   // (0,0,-1)
                cameraRight = Vector3.left;   // (-1,0,0)
            }
            else
            {
                // ī�޶� �÷��̾� "��" �ʿ� �ִ� ���
                // forward �Է� => forward ���, right �Է� => right ���
                cameraForward = Vector3.forward; // (0,0,1)
                cameraRight = Vector3.right;   // (1,0,0)
            }
        }
        else
        {
            // �¿� ������ �켼
            if (dotR >= 0f)
            {
                // ī�޶� �÷��̾� "������"�� �ִ� ���
                // forward �Է� => left ���, right �Է� => forward ���
                cameraForward = Vector3.left;   // (-1,0,0)
                cameraRight = Vector3.forward;   // (0,0,1)
            }
            else
            {
                // ī�޶� �÷��̾� "����"�� �ִ� ���
                // forward �Է� => right ���, right �Է� => back ���
                cameraForward = Vector3.right;   // (1,0,0)
                cameraRight = Vector3.back; // (0,0,-1)
            }
        }

        // inputVector�� ������ ���� cameraForward / cameraRight �࿡ ����
        // x �Է� => cameraRight   ���� / y �Է� => cameraForward ����
        adjustedMovement = cameraRight * _inputVector.x + cameraForward * _inputVector.y;

        // interpolate speed
        targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
        targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4.0f);

        movementInput = adjustedMovement * targetSpeed;
        _previousSpeed = targetSpeed;
    }


    //private void RecalculateMovement()
    //{
    //    float targetSpeed;
    //    Vector3 adjustedMovement;

    //    if (_gameplayCameraTransform.isSet)
    //    {
    //        Vector3 cameraForward = _gameplayCameraTransform.Value.forward;
    //        cameraForward.y = .0f;
    //        Vector3 cameraRight = _gameplayCameraTransform.Value.right;
    //        cameraRight.y = .0f;

    //        adjustedMovement = cameraRight.normalized * _inputVector.x + cameraForward.normalized * _inputVector.y;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
    //        adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);
    //    }

    //    targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
    //    targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4.0f);

    //    //if (_inputVector.sqrMagnitude == 0.0f)
    //    //    adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

    //    movementInput = adjustedMovement * targetSpeed;

    //    _previousSpeed = targetSpeed;
    //}

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
