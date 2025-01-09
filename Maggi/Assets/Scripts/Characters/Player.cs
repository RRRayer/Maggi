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

        // 플레이어 기준으로 카메라가 앞(front), 뒤(back), 오른쪽(right), 왼쪽(left) 중 어디에 있는가?
        Vector3 directionToCamera = Vector3.zero;
        if (_gameplayCameraTransform.isSet)
        {
            directionToCamera = (_gameplayCameraTransform.Value.position - transform.position).normalized;
        }
        else
        {
            Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
        }

        // 판별을 위해 플레이어의 forward, right 벡터와 dot product 비교
        // dotF : 카메라가 정면(+) 쪽인지 후면(-) 쪽인지 판단
        // dotR : 카메라가 오른쪽(+) 쪽인지 왼쪽(-) 쪽인지 판단
        float dotF = Vector3.Dot(directionToCamera, Vector3.forward);
        float dotR = Vector3.Dot(directionToCamera, Vector3.right);

        // 가장 큰 절댓값을 기준으로 "앞/뒤" 또는 "오른쪽/왼쪽"을 결정
        // 그리고 그에 맞춰 cameraForward / cameraRight를 전역축 중 하나로 설정
        Vector3 cameraForward = Vector3.zero;
        Vector3 cameraRight = Vector3.zero;

        // 앞뒤와 좌우 결정
        if (Mathf.Abs(dotF) >= Mathf.Abs(dotR))
        {
            // 앞뒤 방향이 우세
            if (dotF >= 0f)
            {
                // 카메라가 플레이어 "앞" 쪽에 있는 경우
                // forward 입력 => back 출력, right 입력 => left 출력
                cameraForward = Vector3.back;   // (0,0,-1)
                cameraRight = Vector3.left;   // (-1,0,0)
            }
            else
            {
                // 카메라가 플레이어 "뒤" 쪽에 있는 경우
                // forward 입력 => forward 출력, right 입력 => right 출력
                cameraForward = Vector3.forward; // (0,0,1)
                cameraRight = Vector3.right;   // (1,0,0)
            }
        }
        else
        {
            // 좌우 방향이 우세
            if (dotR >= 0f)
            {
                // 카메라가 플레이어 "오른쪽"에 있는 경우
                // forward 입력 => left 출력, right 입력 => forward 출력
                cameraForward = Vector3.left;   // (-1,0,0)
                cameraRight = Vector3.forward;   // (0,0,1)
            }
            else
            {
                // 카메라가 플레이어 "왼쪽"에 있는 경우
                // forward 입력 => right 출력, right 입력 => back 출력
                cameraForward = Vector3.right;   // (1,0,0)
                cameraRight = Vector3.back; // (0,0,-1)
            }
        }

        // inputVector를 위에서 정한 cameraForward / cameraRight 축에 매핑
        // x 입력 => cameraRight   방향 / y 입력 => cameraForward 방향
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
