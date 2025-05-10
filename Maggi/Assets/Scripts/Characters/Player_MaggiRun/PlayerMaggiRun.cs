using UnityEngine;

public class PlayerMaggiRun : MonoBehaviour
{
    [SerializeField] private InputReaderMaggiRun _inputReader = default;
    public InputReaderMaggiRun InputReader => _inputReader;
    [SerializeField] private TransformAnchor _gameplayCameraTransform;

    [HideInInspector] public Vector3 movementInput;
    [HideInInspector] public bool jumpInput;

    private Vector2 _inputVector;
    private float _previousSpeed;

    private bool inSlowMode = false;
    public bool InSlowMode => inSlowMode;

    private bool isDead = false;
    public bool IsDead => isDead;

    private GameObject currentSlowWall = null;
    [SerializeField] private VoidEventChannelSO slowWallClearedEvent;

    // 중력 관련
    [SerializeField] private float gravityStrength = 20f;
    private Vector3 GravityDirection = Vector3.down;
    private Vector3 MoveDirection = Vector3.forward;
    private Rigidbody rb;

    private Quaternion _targetRotation;
    private bool _needsRotationUpdate;
    private float gravityLockTimer = 0f;
    private const float GRAVITY_LOCK_DURATION = 0.2f;

    public bool NeedsGravityRotation => _needsRotationUpdate;
    public Quaternion GetTargetRotation() => _targetRotation;
    public void FinishGravityRotation() => _needsRotationUpdate = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GravityDirection = Vector3.down;
        Physics.gravity = GravityDirection * gravityStrength;
        MoveDirection = Vector3.ProjectOnPlane(transform.forward, GravityDirection).normalized;
    }

    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMovement;
        _inputReader.JumpEvent += () => jumpInput = true;
        _inputReader.JumpCancelEvent += () => jumpInput = false;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMovement;
        _inputReader.JumpEvent -= () => jumpInput = true;
        _inputReader.JumpCancelEvent -= () => jumpInput = false;
    }

    private void Update()
    {
        RecalculateMovement();
        if (gravityLockTimer > 0f) gravityLockTimer -= Time.deltaTime;
    }

    private void RecalculateMovement()
    {
        if (!_gameplayCameraTransform.isSet) return;

        Vector3 toCamera = (_gameplayCameraTransform.Value.position - transform.position).normalized;
        float dotF = Vector3.Dot(toCamera, Vector3.forward);
        float dotR = Vector3.Dot(toCamera, Vector3.right);

        Vector3 cameraForward = Vector3.zero;
        Vector3 cameraRight = Vector3.zero;

        if (Mathf.Abs(dotF) >= Mathf.Abs(dotR))
        {
            cameraForward = dotF >= 0f ? Vector3.back : Vector3.forward;
            cameraRight = dotF >= 0f ? Vector3.left : Vector3.right;
        }
        else
        {
            cameraForward = dotR >= 0f ? Vector3.left : Vector3.right;
            cameraRight = dotR >= 0f ? Vector3.forward : Vector3.back;
        }

        Vector3 adjustedMovement = cameraRight * _inputVector.x + cameraForward * _inputVector.y;
        float targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
        targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4f);

        movementInput = adjustedMovement * targetSpeed;
        _previousSpeed = targetSpeed;
    }

    private void OnMovement(Vector2 movement) => _inputVector = movement;

    public void UpdateGravity(Vector3 surfaceNormal)
    {
        if (gravityLockTimer > 0f) return;

        Vector3 newDir = -surfaceNormal.normalized;
        if (Vector3.Angle(GravityDirection, newDir) < 5f) return;

        GravityDirection = newDir;
        Physics.gravity = GravityDirection * gravityStrength;

        Vector3 projected = Vector3.ProjectOnPlane(MoveDirection, GravityDirection).normalized;
        if (Vector3.Dot(projected, MoveDirection) < 0) projected = -projected;

        MoveDirection = projected == Vector3.zero
            ? Vector3.ProjectOnPlane(transform.forward, GravityDirection).normalized
            : projected;

        _targetRotation = Quaternion.LookRotation(MoveDirection, -GravityDirection);
        _needsRotationUpdate = true;

        gravityLockTimer = GRAVITY_LOCK_DURATION;
    }

    public void FlipGravity()
    {
        UpdateGravity(-GravityDirection);
        rb.velocity = Vector3.zero;
        rb.AddForce(-GravityDirection * 10f, ForceMode.VelocityChange);
        Debug.Log("중력 반전됨 → " + GravityDirection);
    }

    public void EnterSlowMode(GameObject slowWallRoot = null)
    {
        if (inSlowMode) return;

        Time.timeScale = 0.5f;
        inSlowMode = true;
        currentSlowWall = slowWallRoot;

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