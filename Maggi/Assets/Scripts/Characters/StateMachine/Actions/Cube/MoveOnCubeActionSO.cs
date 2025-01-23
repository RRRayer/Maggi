using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "MoveOnCubeAction", menuName = "State Machines/Actions/Move On Cube Action")]
public class MoveOnCubeActionSO : StateActionSO
{
    public float moveSpeed = 3f;             // 면 위에서의 이동 속도
    public float edgeTransitionTime = 0.25f; // 모서리 넘어갈 때 부드럽게 회전하는 데 걸리는 시간

    protected override StateAction CreateAction() => new MoveOnCubeAction();
}

public class MoveOnCubeAction : StateAction
{
    private MoveOnCubeActionSO _originSO => (MoveOnCubeActionSO)base.OriginSO;

    private Player _player;
    private Transform _transform;
    private Transform _cubeTransform;   // 정육면체(또는 직사육면체)

    // 현재 서 있는 면의 노멀
    private Vector3 _currentFaceNormal;

    // -- 모서리 전환(Edge Transition) 관련 --
    private bool _isEdgeTransition;           // 모서리 전환 중인지
    private float _edgeTransitionElapsed;     // 전환 경과 시간
    private Quaternion _startRotation;        // 전환 시작 시 회전
    private Quaternion _endRotation;          // 전환 목표 회전
    private Vector3 _oldNormal;               // 전환 시작 시 면 노멀
    private Vector3 _newNormal;               // 전환 목표 면 노멀
    private Vector3 _edgeAxis;                // 회전 축(두 면 노멀의 외적)
    private float _edgeAngle;                 // 두 면 노멀 사이 각도(보통 90도)

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _cubeTransform = interactiveObject.transform;
        _player = owner.GetComponent<Player>();
        _transform = _player.transform;
    }

    public override void OnStateEnter()
    {
        if (!_cubeTransform)
            Debug.LogWarning("Cube Transform not assigned!");

        // 초기 면 노멀 설정 (단순히 cubeTransform.up, 없으면 Vector3.up)
        _currentFaceNormal = (_cubeTransform) ? _cubeTransform.up : Vector3.up;
        _isEdgeTransition = false;
    }

    public override void OnUpdate()
    {
        // 1) 모서리 전환 중이면, 회전 보간만 처리
        if (_isEdgeTransition)
        {
            UpdateEdgeTransition();
            return;
        }

        // 2) (정상 이동) Cube가 없다면 중단
        if (!_cubeTransform)
            return;

        // 3) 현재 면 판별
        _currentFaceNormal = GetClosestFaceNormal(_transform.position, _cubeTransform);

        // 4) 이동 입력
        Vector3 input = _player.movementInput;
        MoveOnFace(input);

        // 5) 모서리(Edge) 근처인지 확인 → 인접 면으로 부드럽게 전환 시도
        CheckAndStartEdgeTransition();
    }

    /// <summary>
    /// 모서리 전환 중일 때, OnUpdate()에서 매 프레임 회전 보간 진행
    /// </summary>
    private void UpdateEdgeTransition()
    {
        // 1) 시간 경과
        _edgeTransitionElapsed += Time.deltaTime;
        float t = _edgeTransitionElapsed / _originSO.edgeTransitionTime;
        if (t >= 1f)
        {
            // 전환 완료
            t = 1f;
            _isEdgeTransition = false;

            // 최종 회전 설정
            _transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, t);

            // 새 노멀 적용
            _currentFaceNormal = _newNormal;
            _transform.up = _newNormal;

            // 위치 재보정
            StickToFace(_transform, _cubeTransform, _currentFaceNormal);
        }
        else
        {
            // 전환 진행 중
            _transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, t);
        }
    }

    /// <summary>
    /// 면 위에서 이동 처리 (이동 후 StickToFace로 클램프, Up 정렬)
    /// </summary>
    private void MoveOnFace(Vector3 input)
    {
        // 면 위에서 이동할 축 계산
        ComputeFaceAxes(_currentFaceNormal, out Vector3 faceRight, out Vector3 faceForward);

        // 이동
        Vector3 moveDir = (faceRight * input.x + faceForward * input.z) * _originSO.moveSpeed * Time.deltaTime;
        _transform.position += moveDir;

        // 면 표면에 붙이기
        StickToFace(_transform, _cubeTransform, _currentFaceNormal);

        // Up = faceNormal
        _transform.up = _currentFaceNormal;
    }

    /// <summary>
    /// 모서리 부근이면 인접 면 판별 → 전환 준비
    /// </summary>
    private void CheckAndStartEdgeTransition()
    {
        Vector3 localPos = _cubeTransform.InverseTransformPoint(_transform.position);
        Vector3 half = _cubeTransform.localScale * 0.5f;

        float eps = 0.01f;  // 모서리에 근접한 threshold

        // 단순히 x, y, z 축 각각 체크
        if (Mathf.Abs(Mathf.Abs(localPos.x) - half.x) < eps
            || Mathf.Abs(Mathf.Abs(localPos.y) - half.y) < eps
            || Mathf.Abs(Mathf.Abs(localPos.z) - half.z) < eps)
        {
            Vector3 newNormal = GetClosestFaceNormal(_transform.position, _cubeTransform);
            if (newNormal != _currentFaceNormal)
            {
                BeginEdgeTransition(_currentFaceNormal, newNormal);
            }
        }
    }

    /// <summary>
    /// "현재 면 노멀 → 새 면 노멀" 로 모서리 전환을 시작하기
    /// </summary>
    private void BeginEdgeTransition(Vector3 oldNormal, Vector3 newNormal)
    {
        // 1) 각도 계산
        float angle = Vector3.Angle(oldNormal, newNormal);
        if (angle < 0.01f)
        {
            // 거의 차이 없으면, 그냥 갱신만
            _currentFaceNormal = newNormal;
            _transform.up = newNormal;
            StickToFace(_transform, _cubeTransform, _currentFaceNormal);
            return;
        }

        // 2) 회전 축
        Vector3 axis = Vector3.Cross(oldNormal, newNormal).normalized;

        // 3) 전환 상태 세팅
        _isEdgeTransition = true;
        _edgeTransitionElapsed = 0f;

        _oldNormal = oldNormal;
        _newNormal = newNormal;
        _edgeAxis = axis;
        _edgeAngle = angle;

        // 4) 시작 회전, 끝 회전
        _startRotation = _transform.rotation;
        _endRotation = Quaternion.AngleAxis(_edgeAngle, _edgeAxis) * _startRotation;
    }

    /// <summary>
    /// _cubeTransform 기준으로, "가장 가까운 면(Face)" 노멀 반환
    /// </summary>
    private Vector3 GetClosestFaceNormal(Vector3 playerPos, Transform cube)
    {
        Vector3 localPos = cube.InverseTransformPoint(playerPos);
        Vector3 half = cube.localScale * 0.5f;

        // 정규화해서 0~1 스케일로 환산
        float ratioX = Mathf.Abs(localPos.x) / half.x;
        float ratioY = Mathf.Abs(localPos.y) / half.y;
        float ratioZ = Mathf.Abs(localPos.z) / half.z;

        // 가장 큰 ratio를 갖는 축이 "현재 붙어 있는 면" 
        if (ratioX > ratioY && ratioX > ratioZ)
        {
            // X 면
            if (localPos.x >= 0) return cube.TransformDirection(Vector3.right);
            else return cube.TransformDirection(Vector3.left);
        }
        else if (ratioY > ratioX && ratioY > ratioZ)
        {
            // Y 면
            if (localPos.y >= 0) return cube.TransformDirection(Vector3.up);
            else return cube.TransformDirection(Vector3.down);
        }
        else
        {
            // Z 면
            if (localPos.z >= 0) return cube.TransformDirection(Vector3.forward);
            else return cube.TransformDirection(Vector3.back);
        }

    }

    /// <summary>
    /// "면 평면" 위에 클램프 (±halfExtent)에 붙이기
    /// </summary>
    private void StickToFace(Transform player, Transform cube, Vector3 faceNormal)
    {
        Vector3 localPos = cube.InverseTransformPoint(player.position);
        Vector3 localNormal = cube.InverseTransformDirection(faceNormal);
        Vector3 half = cube.localScale * 0.5f;

        float dotX = Vector3.Dot(localNormal, Vector3.right);
        float dotY = Vector3.Dot(localNormal, Vector3.up);
        float dotZ = Vector3.Dot(localNormal, Vector3.forward);

        if (Mathf.Abs(dotX) > 0.99f)
        {
            // ±X 면
            localPos.x = (dotX > 0f) ? +half.x : -half.x;
        }
        else if (Mathf.Abs(dotY) > 0.99f)
        {
            // ±Y 면
            localPos.y = (dotY > 0f) ? +half.y : -half.y;
        }
        else
        {
            // ±Z 면
            localPos.z = (dotZ > 0f) ? +half.z : -half.z;
        }

        Debug.Log(localPos);

        Vector3 newWorldPos = cube.TransformPoint(localPos);
        player.position = newWorldPos;
    }

    /// <summary>
    /// 면 노멀(faceNormal)에 대해 '수평 이동'할 수 있는 두 축(faceRight, faceForward) 계산
    /// </summary>
    private void ComputeFaceAxes(Vector3 faceNormal, out Vector3 faceRight, out Vector3 faceForward)
    {
        faceForward = Vector3.Cross(faceNormal, Vector3.up);
        if (faceForward.sqrMagnitude < 0.001f)
        {
            faceForward = Vector3.Cross(faceNormal, Vector3.forward);
        }
        faceForward.Normalize();

        faceRight = Vector3.Cross(faceNormal, faceForward).normalized;
    }
}
