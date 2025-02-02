using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "MoveOnCubeAction", menuName = "State Machines/Actions/Move On Cube Action")]
public class MoveOnCubeActionSO : StateActionSO
{
    public float moveSpeed = 3f;             // 면 위에서의 이동 속도
    public float edgeTransitionTime = 0.25f; // 모서리 넘어갈 때 부드럽게 회전하는 데 걸리는 시간
    public float turnSmoothTime = 5.0f;

    protected override StateAction CreateAction() => new MoveOnCubeAction();
}

public class MoveOnCubeAction : StateAction
{
    private MoveOnCubeActionSO _originSO => (MoveOnCubeActionSO)base.OriginSO;

    private Player _player;
    private Transform _transform;
    private Transform _cubeTransform;   // 정육면체(또는 직사육면체)
    private const float ROTATION_TRESHOLD = 0.02f;

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
        {
            Debug.LogWarning("Cube Transform not assigned!");
            return;
        }

        // 초기 면 노멀
        _currentFaceNormal = GetClosestFaceNormal(_transform.position, _cubeTransform);

        // 면 표면에 붙이기
        StickToFace(_transform, _cubeTransform, _currentFaceNormal);

        //// 모서리 전환 Flag
        //_isEdgeTransition = false;
    }

    public override void OnUpdate()
    {
        //// 1) 모서리 전환 중이면 회전 보간만 처리
        //if (_isEdgeTransition)
        //{
        //    Debug.Log("#########edge transition...###########");
        //    UpdateEdgeTransition();
        //    return;
        //}

        // 2) (정상 이동) Cube가 없다면 중단
        if (!_cubeTransform)
            return;

        // 3) 현재 면 판별 (ex. X/Y/Z 면 중 어디에 붙었는지)
        _currentFaceNormal = GetClosestFaceNormal(_transform.position, _cubeTransform);

        // 4) 이동 입력
        Vector3 input = _player.movementInput; // (x: 좌우, z: 전후)
        MoveOnFace(input);

        // 5) 모서리(Edge) 근처인지 확인 → 인접 면으로 부드럽게 전환
        //CheckAndStartEdgeTransition();
    }

    ///// <summary>
    ///// 모서리 전환 중이면, OnUpdate()에서 매 프레임 회전 보간
    ///// </summary>
    //private void UpdateEdgeTransition()
    //{
    //    _edgeTransitionElapsed += Time.deltaTime;
    //    float t = _edgeTransitionElapsed / _originSO.edgeTransitionTime;
    //    if (t >= 1f)
    //    {
    //        t = 1f;
    //        _isEdgeTransition = false;

    //        // 전환 완료 → 최종 회전
    //        _transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, t);

    //        // 새 노멀 적용
    //        _currentFaceNormal = _newNormal;
    //        _transform.up = _newNormal;

    //        // 위치 보정
    //        StickToFace(_transform, _cubeTransform, _currentFaceNormal);
    //    }
    //    else
    //    {
    //        // 전환 진행 중
    //        _transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, t);
    //    }
    //}

    /// <summary>
    /// 면 위에서 이동 처리
    /// </summary>
    private void MoveOnFace(Vector3 input)
    {
        // "Forward = 면∩YZ 교선, Right = 면∩XY 교선"
        // 교선 = cross(면노멀, 평면노멀)
        ComputeFaceAxes(_currentFaceNormal, out Vector3 faceRight, out Vector3 faceForward);

        // 이동 벡터
        // forward => input.z, right => input.x
        Vector3 newMovementVector = (faceForward * input.z + faceRight * input.x).normalized * _originSO.moveSpeed;
        _player.movementVector = newMovementVector;

        // 면 표면에 붙이기
        StickToFace(_transform, _cubeTransform, _currentFaceNormal);

        _transform.rotation = Quaternion.FromToRotation(_transform.up, _currentFaceNormal) * _transform.rotation;

        // player를 이동 벡터 방향으로 회전
        if (_player.movementVector.sqrMagnitude >= ROTATION_TRESHOLD)
        {
            Quaternion targetRotation = Quaternion.LookRotation(newMovementVector, _currentFaceNormal);
            // _transform.up을 기준으로 90도 회전
            Quaternion additionalRotation = Quaternion.AngleAxis(-90.0f, _transform.up);

            // 현재 회전에 추가 회전을 곱함
            targetRotation = additionalRotation * targetRotation;

            // 3) slerp
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, _originSO.turnSmoothTime * Time.deltaTime);
        }
    }

    ///// <summary>
    ///// 모서리 근처면 인접 면 판별 → 전환 준비
    ///// </summary>
    //private void CheckAndStartEdgeTransition()
    //{
    //    Vector3 localPos = _cubeTransform.InverseTransformPoint(_transform.position);
    //    Vector3 half = _cubeTransform.localScale * 0.5f;

    //    float eps = 0.01f;
    //    if (Mathf.Abs(Mathf.Abs(localPos.x) - half.x) < eps
    //     || Mathf.Abs(Mathf.Abs(localPos.y) - half.y) < eps
    //     || Mathf.Abs(Mathf.Abs(localPos.z) - half.z) < eps)
    //    {
    //        Vector3 newNormal = GetClosestFaceNormal(_transform.position, _cubeTransform);
    //        if (newNormal != _currentFaceNormal)
    //        {
    //            BeginEdgeTransition(_currentFaceNormal, newNormal);
    //        }
    //    }
    //}

    ///// <summary>
    ///// 실제 면 전환 시작
    ///// </summary>
    //private void BeginEdgeTransition(Vector3 oldNormal, Vector3 newNormal)
    //{
    //    float angle = Vector3.Angle(oldNormal, newNormal);
    //    if (angle < 0.01f)
    //    {
    //        _currentFaceNormal = newNormal;
    //        _transform.up = newNormal;
    //        StickToFace(_transform, _cubeTransform, _currentFaceNormal);
    //        return;
    //    }

    //    Vector3 axis = Vector3.Cross(oldNormal, newNormal).normalized;

    //    _isEdgeTransition = true;
    //    _edgeTransitionElapsed = 0f;

    //    _oldNormal = oldNormal;
    //    _newNormal = newNormal;
    //    _edgeAxis = axis;
    //    _edgeAngle = angle;

    //    _startRotation = _transform.rotation;
    //    _endRotation = Quaternion.AngleAxis(_edgeAngle, _edgeAxis) * _startRotation;
    //}

    /// <summary>
    /// 현재 위치에서 가장 가까운 면(±X/±Y/±Z) 노멀을 구함
    /// </summary>
    private Vector3 GetClosestFaceNormal(Vector3 playerPos, Transform cube)
    {
        Vector3 localPos = cube.InverseTransformPoint(playerPos);
        Vector3 half = cube.localScale * 0.5f;

        float ratioX = Mathf.Abs(localPos.x) / half.x;
        float ratioY = Mathf.Abs(localPos.y) / half.y;
        float ratioZ = Mathf.Abs(localPos.z) / half.z;

        if (ratioX > ratioY && ratioX > ratioZ)
        {
            if (localPos.x >= 0) return cube.TransformDirection(Vector3.right);
            else return cube.TransformDirection(Vector3.left);
        }
        else if (ratioY > ratioX && ratioY > ratioZ)
        {
            if (localPos.y >= 0) return cube.TransformDirection(Vector3.up);
            else return cube.TransformDirection(Vector3.down);
        }
        else
        {
            if (localPos.z >= 0) return cube.TransformDirection(Vector3.forward);
            else return cube.TransformDirection(Vector3.back);
        }
    }

    /// <summary>
    /// 면 평면에 위치 고정 (±halfExtent)
    /// </summary>
    private void StickToFace(Transform player, Transform cube, Vector3 faceNormal)
    {
        Vector3 localNormal = cube.InverseTransformDirection(faceNormal);
        Vector3 offset = (cube.localScale + player.localScale) * 0.5f;
        Vector3 newPosition = player.position;

        float dotX = Vector3.Dot(localNormal, Vector3.right);
        float dotY = Vector3.Dot(localNormal, Vector3.up);
        float dotZ = Vector3.Dot(localNormal, Vector3.forward);

        if (Mathf.Abs(dotX) > 0.99f) 
            newPosition.x = (dotX > 0f) ? cube.position.x + offset.x : cube.position.x - offset.x;
        else if (Mathf.Abs(dotY) > 0.99f) 
            newPosition.y = (dotY > 0f) ? cube.position.y + offset.y : cube.position.y - offset.y;
        else 
            newPosition.z = (dotZ > 0f) ? cube.position.z + offset.z : cube.position.z - offset.z;

        player.position = newPosition;
    }

    /// <summary>
    /// 면 노멀 faceNormal이 있을 때,
    /// 1) faceForward = (면∩YZ 교선) 을 '양의 방향'으로 보정
    /// 2) faceRight   = (면∩XY 교선) 을 '양의 방향'으로 보정
    /// </summary>
    private void ComputeFaceAxes(Vector3 faceNormal, out Vector3 faceRight, out Vector3 faceForward)
    {
        // YZ 평면 normal => (1,0,0)
        Vector3 yzNormal = _cubeTransform.right;
        // XY 평면 normal => (0,0,1)
        Vector3 xyNormal = _cubeTransform.forward;

        // 1) 교선 계산
        //    - forward: 교선( faceNormal ∩ YZ ), = cross(faceNormal, yzNormal)
        //    - right:   교선( faceNormal ∩ XY ), = cross(faceNormal, xyNormal)
        faceForward = -Vector3.Cross(faceNormal, yzNormal);
        faceRight = Vector3.Cross(faceNormal, xyNormal);

        // 혹시 교선이 0벡터?
        if (faceForward.sqrMagnitude < 1e-6f) faceForward = Vector3.forward;
        else faceForward.Normalize();

        if (faceRight.sqrMagnitude < 1e-6f) faceRight = Vector3.right;
        else faceRight.Normalize();
    }
}
