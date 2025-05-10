using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 20f;
    public float gravityStrength = 20f;

    private Rigidbody rb;
    private bool isDead = false;
    public bool IsDead => isDead;

    private bool isGrounded = false;
    private Vector3 GravityDirection = Vector3.down;
    private Vector3 MoveDirection = Vector3.forward; // 전진 = transform.forward 기준

    private bool inSlowMode = false;
    private float slowTimer = 0f;
    private float slowDuration = 3f;
    private GameObject currentSlowWall = null;

    private float gravityLockTimer = 0f;
    private const float GRAVITY_LOCK_DURATION = 0.2f;

    public VoidEventChannelSO playerDiedEvent;
    public VoidEventChannelSO sceneRestartEvent;
    public VoidEventChannelSO slowWallClearedEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GravityDirection = Vector3.down;
        Physics.gravity = GravityDirection * gravityStrength;

        MoveDirection = Vector3.ProjectOnPlane(transform.forward, GravityDirection).normalized;
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (Vector3.Dot(rb.linearVelocity, GravityDirection) > 0 && !isGrounded)
        {
            rb.AddForce(GravityDirection * gravityStrength * 1.5f);
        }
    }

    private void Update()
    {
        if (isDead) return;

        if (gravityLockTimer > 0f)
            gravityLockTimer -= Time.deltaTime;

        HandleJump();
        HandleSlowInput();
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !inSlowMode)
        {
            Vector3 jumpDirection = -GravityDirection;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
            return;
        }

        if (TryUpdateGravityFromCollision(collision))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (TryUpdateGravityFromCollision(collision))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private bool TryUpdateGravityFromCollision(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 normal = contact.normal.normalized;

            UpdateGravity(normal);
            return true;
        }

        return false;
    }

    private void UpdateGravity(Vector3 surfaceNormal)
    {
        if (gravityLockTimer > 0f) return;

        Vector3 newDir = -surfaceNormal.normalized;
        if (Vector3.Angle(GravityDirection, newDir) < 5f) return;

        GravityDirection = newDir;
        Physics.gravity = GravityDirection * gravityStrength;

        // 기존 이동 방향을 새 중력 기준으로 보정
        MoveDirection = Vector3.ProjectOnPlane(MoveDirection, GravityDirection).normalized;
        if (MoveDirection == Vector3.zero)
            MoveDirection = Vector3.ProjectOnPlane(transform.forward, GravityDirection).normalized;

        Quaternion targetRot = Quaternion.LookRotation(MoveDirection, -GravityDirection);
        rb.MoveRotation(targetRot);

        gravityLockTimer = GRAVITY_LOCK_DURATION;
    }

    public void FlipGravity()
    {
        GravityDirection = -GravityDirection;
        Physics.gravity = GravityDirection * gravityStrength;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(-GravityDirection * 10f, ForceMode.VelocityChange);

        MoveDirection = Vector3.ProjectOnPlane(MoveDirection, GravityDirection).normalized;
        if (MoveDirection == Vector3.zero)
            MoveDirection = Vector3.ProjectOnPlane(transform.forward, GravityDirection).normalized;

        Quaternion targetRot = Quaternion.LookRotation(MoveDirection, -GravityDirection);
        rb.MoveRotation(targetRot);

        gravityLockTimer = GRAVITY_LOCK_DURATION;

        Debug.Log("중력 반전됨 → " + GravityDirection);
    }

    public void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        Time.timeScale = 1f;

        playerDiedEvent?.RaiseEvent();
        Invoke(nameof(Restart), 2f);
    }

    private void Restart()
    {
        sceneRestartEvent?.RaiseEvent();
    }

    public void EnterSlowMode(GameObject slowWallRoot)
    {
        if (inSlowMode) return;

        inSlowMode = true;
        slowTimer = 0f;
        currentSlowWall = slowWallRoot;

        Time.timeScale = 0.5f;
    }

    private void HandleSlowInput()
    {
        if (!inSlowMode) return;

        slowTimer += Time.unscaledDeltaTime;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 1f;
            inSlowMode = false;

            slowWallClearedEvent?.RaiseEvent();

            if (currentSlowWall != null)
                Destroy(currentSlowWall);

            currentSlowWall = null;
        }
        else if (slowTimer >= slowDuration)
        {
            Time.timeScale = 1f;
            inSlowMode = false;
            currentSlowWall = null;
        }
    }
}