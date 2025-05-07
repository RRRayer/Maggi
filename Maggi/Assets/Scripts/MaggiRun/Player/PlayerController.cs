using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 8f;
    public float sideSpeed = 8f;
    public float jumpForce = 20f;
    public float gravityStrength = 20f;

    private Rigidbody rb;
    private bool isDead = false;
    public bool IsDead => isDead;

    private bool isGrounded = false;
    private Vector3 currentGravityDir = Vector3.down;


    // 슬로우 관련 변수
    private bool inSlowMode = false;
    private float slowTimer = 0f;
    private float slowDuration = 3f;
    private GameObject currentSlowWall = null;
    public VoidEventChannelSO playerDiedEvent;
    public VoidEventChannelSO sceneRestartEvent;
    public VoidEventChannelSO slowWallClearedEvent;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentGravityDir = Vector3.down;
        Physics.gravity = currentGravityDir * gravityStrength;

        SnapToStartFace();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (Vector3.Dot(rb.velocity, currentGravityDir) > 0 && !isGrounded)
        {
            rb.AddForce(currentGravityDir * gravityStrength * 1.5f);
        }

        MoveForward();
        HandleSideMovement();
    }

    private void Update()
    {
        if (isDead) return;

        HandleJump();
        HandleSlowInput();
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !inSlowMode)
        {
            Vector3 up = -currentGravityDir;
            rb.AddForce(up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }
    }

    private void MoveForward()
    {
        Vector3 forwardVel = transform.forward * forwardSpeed;
        Vector3 gravityVel = Vector3.Project(rb.velocity, currentGravityDir);
        rb.velocity = forwardVel + gravityVel;
    }

    private void HandleSideMovement()
    {
        Vector3 right = Vector3.Cross(-currentGravityDir, transform.forward).normalized;

        if (Input.GetMouseButton(0))
            rb.MovePosition(rb.position - right * sideSpeed * Time.fixedDeltaTime);
        else if (Input.GetMouseButton(1))
            rb.MovePosition(rb.position + right * sideSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
            return;
        }

        if (collision.gameObject.CompareTag("Face"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 normal = contact.normal;

            UpdateGravity(normal);
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Face"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Face"))
        {
            isGrounded = false;
        }
    }

    private void UpdateGravity(Vector3 surfaceNormal)
    {
        Vector3 newUp = surfaceNormal.normalized;
        currentGravityDir = -newUp;
        Physics.gravity = currentGravityDir * gravityStrength;

        Quaternion targetRot = Quaternion.LookRotation(transform.forward, newUp);
        rb.MoveRotation(targetRot);
    }

    private void SnapToStartFace()
    {
        GameObject face0 = GameObject.Find("Face_4");
        if (face0 == null) return;

        Transform face = face0.transform;

        // 면 방향 기준 벡터
        Vector3 normal = face.forward;    // 바닥 방향
        Vector3 forward = face.up;        // 복도 달리는 방향
        Vector3 right = face.right;       // 좌우 방향

        // 목표 시작 위치 (절대 좌표)
        Vector3 targetPos = new Vector3(0f, -24f, -200f);

        // 상대 오프셋 계산 (dot product)
        float sideOffset     = Vector3.Dot(targetPos - face.position, right);
        float forwardOffset  = Vector3.Dot(targetPos - face.position, forward);
        float verticalOffset = Vector3.Dot(targetPos - face.position, normal);

        // 최종 위치 = 기준 face에서 방향 벡터 * 오프셋
        Vector3 spawnPos = face.position
                        + right   * sideOffset
                        + forward * forwardOffset
                        + normal  * verticalOffset;

        transform.position = spawnPos;

        UpdateGravity(normal);
        isGrounded = true;
    }


    public void Die()
    {
        isDead = true;
        rb.velocity = Vector3.zero;
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

    public void FlipGravity()
    {
        currentGravityDir = -currentGravityDir;
        Physics.gravity = currentGravityDir * gravityStrength;

        Quaternion targetRot = Quaternion.LookRotation(transform.forward, -currentGravityDir);
        rb.MoveRotation(targetRot);

        Debug.Log("중력 반전됨 → " + currentGravityDir);
    }

}