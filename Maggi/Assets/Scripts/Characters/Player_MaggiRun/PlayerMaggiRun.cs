using System;
using UnityEngine;

public class PlayerMaggiRun : MonoBehaviour
{
    [SerializeField] private InputReaderMaggiRun _inputReader = default;
    public InputReaderMaggiRun InputReader => _inputReader;
    [SerializeField] private TransformAnchor _gameplayCameraTransform = default;

    //These fields are read and manipulated by the StateMachine actions
    [HideInInspector] public Vector3 movementInput;
    [HideInInspector] public Vector3 movementVector;
    [HideInInspector] public bool jumpInput;
    [HideInInspector] public bool runInput;


    private Vector2 _inputVector; // _inputVector.x : x movement, _inputVector.y : z movement
    private float _previousSpeed;

    public const float AIR_RESISTANCE = 5f;
    public const float RUN_SPEED_MULTIPLIER = 1.5f;
    public const float MAX_FALL_SPEED = -50f;
    public const float MAX_RISE_SPEED = 100f;
    public const float GRAVITY_MULTIPLIER = 2f;
    public const float GRAVITY_COMEBACK_MULTIPLIER = 0.06f;
    public const float GRAVITY_DIVIDER = 0.6f;
    
    private bool inSlowMode = false;
    private bool isDead = false;
    public bool IsDead => isDead;
    private GameObject currentSlowWall = null;
    [SerializeField] private VoidEventChannelSO slowWallClearedEvent;
    public bool InSlowMode => inSlowMode;
    private Quaternion _targetRotation;
    private bool _needsRotationUpdate;

    // 기본 중력 설정
    [SerializeField] private float gravityStrength = 20f;
    private Vector3 GravityDirection = Vector3.down;
    private Vector3 MoveDirection = Vector3.forward;

    // 중력 전환 잠금 타이머
    private float gravityLockTimer = 0f;
    private const float GRAVITY_LOCK_DURATION = 0.2f;

    // Rigidbody 참조
    private Rigidbody rb;

    private void FixedUpdate()
    {
        // 중력 회전 처리
        if (_needsRotationUpdate)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 360f * Time.fixedDeltaTime);
            if (Quaternion.Angle(transform.rotation, _targetRotation) < 0.1f)
                _needsRotationUpdate = false;
        }

        // 기존 FixedUpdate 이동/물리 처리 로직 여기 넣기
    }

    private void UpdateGravity(Vector3 surfaceNormal)
    {
        if (gravityLockTimer > 0f) return;

        Vector3 newDir = -surfaceNormal.normalized;
        if (Vector3.Angle(GravityDirection, newDir) < 5f) return;

        GravityDirection = newDir;
        Physics.gravity = GravityDirection * gravityStrength;

        // 이동 방향 재보정
        Vector3 projected = Vector3.ProjectOnPlane(MoveDirection, GravityDirection).normalized;

        // 반대 방향이라면 뒤집기 (튐 방지)
        if (Vector3.Dot(projected, MoveDirection) < 0)
            projected = -projected;

        MoveDirection = projected == Vector3.zero
            ? Vector3.ProjectOnPlane(transform.forward, GravityDirection).normalized
            : projected;

        // 회전 보간 처리
        Quaternion currentRot = rb.rotation;
        Quaternion targetRot = Quaternion.LookRotation(MoveDirection, -GravityDirection);
        Quaternion smoothRot = Quaternion.RotateTowards(currentRot, targetRot, 360f); // 빠른 보간
        rb.MoveRotation(smoothRot);

        gravityLockTimer = GRAVITY_LOCK_DURATION;
    }

    public void FlipGravity()
    {
        GravityDirection = -GravityDirection;
        Physics.gravity = GravityDirection * gravityStrength;

        rb.velocity = Vector3.zero;
        rb.AddForce(-GravityDirection * 10f, ForceMode.VelocityChange);

        Vector3 projected = Vector3.ProjectOnPlane(MoveDirection, GravityDirection).normalized;

        if (Vector3.Dot(projected, MoveDirection) < 0)
            projected = -projected;

        MoveDirection = projected == Vector3.zero
            ? Vector3.ProjectOnPlane(transform.forward, GravityDirection).normalized
            : projected;

        Quaternion currentRot = rb.rotation;
        Quaternion targetRot = Quaternion.LookRotation(MoveDirection, -GravityDirection);
        Quaternion smoothRot = Quaternion.RotateTowards(currentRot, targetRot, 360f);
        rb.MoveRotation(smoothRot);

        gravityLockTimer = GRAVITY_LOCK_DURATION;

        Debug.Log("중력 반전됨 → " + GravityDirection);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GravityDirection = Vector3.down;
        Physics.gravity = GravityDirection * gravityStrength;

        // 초기 전진 방향 보정
        MoveDirection = Vector3.ProjectOnPlane(transform.forward, GravityDirection).normalized;
    }

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
        if (gravityLockTimer > 0f)
            gravityLockTimer -= Time.deltaTime;
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

    public void EnterSlowMode(GameObject slowWallRoot = null)
    {
        if (inSlowMode) return;

        Time.timeScale = 0.5f;
        inSlowMode = true;

        currentSlowWall = slowWallRoot;

        // 슬로우월 제거는 나중에 Attack에서 직접 하거나,
        // 자동 해제 조건에 따라 처리됨 (이곳에서 제거 X)

        Debug.Log("슬로우모드 진입");
    }
    
    public void ExitSlowMode()
    {
        Time.timeScale = 1f;
        inSlowMode = false;

        if (currentSlowWall != null)
            Destroy(currentSlowWall);

        currentSlowWall = null;
        slowWallClearedEvent?.RaiseEvent();
    }

    public void Die()
    {
        isDead = true;
    }
}
