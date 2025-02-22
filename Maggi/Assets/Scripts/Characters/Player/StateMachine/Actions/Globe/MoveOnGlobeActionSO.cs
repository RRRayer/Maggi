using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "MoveOnGlobeAction", menuName = "State Machines/Actions/Move On Globe Action")]
public class MoveOnGlobeActionSO : StateActionSO<MoveOnGlobeAction>
{
    // 이동 각도 속도
    public float moveSpeedTheta = 1f;  // 세로(빨간선) 각도 변화 속도
    public float moveSpeedPhi   = 1f;  // 가로(파란선) 각도 변화 속도

    // (선택) 각도 제한
    [Header("Theta (세로) 범위 제한")]
    public bool clampTheta = false;
    public float minTheta = 0f;    // 기본: 0(북극)
    public float maxTheta = Mathf.PI; // 기본: π(남극)

    [Header("Phi (수평) 범위 제한")]
    public bool clampPhi = false;
    public float minPhi  = 0f;
    public float maxPhi  = 2f * Mathf.PI;

    [Header("기타 옵션")]
    public bool ignoreHorizontalNearPoles = true; // 꼭대기/바닥에서 좌우 입력 무시
    public float poleThreshold = 0.01f;   // θ=0 또는 θ=π에서부터 ±이 값 이내일 때
    public bool useRightAsForward = false; // transform.right가 실제 이동 방향이 되도록

    [Header("Rotation Smooth Damping")]
    public float rotationSpeed = 5.0f;
}

public class MoveOnGlobeAction : StateAction
{
    private MoveOnGlobeActionSO _originSO => (MoveOnGlobeActionSO)OriginSO;

    private Player _player;
    private Transform _transform;
    private InteractiveObject _interactiveObject;

    // 구면좌표상의 각도:
    //  - theta: 북극(0) ~ 남극(pi)  
    //  - phi:   0~2pi 범위의 수평 둘레 각
    private float _theta;
    private float _phi;

    private float _sphereRadius;
    private float _playerRadius;
    private Vector3 _sphereCenter;

    // 구의 "월드 기준" 축들
    private Vector3 _sphereUp;       // 구의 ‘북극’ 방향
    private Vector3 _sphereRight;    // 구의 ‘동쪽’ 축
    private Vector3 _sphereForward;  // 구의 ‘앞쪽’ 축

    private const float ROTATION_TRESHOLD = 0.02f;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _transform = owner.GetComponent<Transform>();
        _interactiveObject = interactiveObject;

        // 구 정보 초기화
        Transform sphereT = _interactiveObject.transform;
        _sphereCenter = sphereT.position;
        _sphereRadius = sphereT.localScale.x * 0.5f;
        _playerRadius = _transform.localScale.x * 0.5f;

        // 구의 월드 좌표계에서 Up/Right/Forward
        _sphereUp       = sphereT.up.normalized;
        _sphereRight    = sphereT.right.normalized;
        _sphereForward  = sphereT.forward.normalized;

        // 플레이어 현재 위치 -> (theta, phi) 계산
        Vector3 offset  = _transform.position - _sphereCenter;
        float r         = offset.magnitude;
        if (r < 0.0001f) r = _sphereRadius + _playerRadius;

        // theta
        float cosTheta = Vector3.Dot(offset.normalized, _sphereUp);
        cosTheta = Mathf.Clamp(cosTheta, -1f, 1f);
        _theta   = Mathf.Acos(cosTheta); // 0=북극, π=남극

        // phi
        Vector3 horizontal = Vector3.ProjectOnPlane(offset, _sphereUp);
        if (horizontal.sqrMagnitude < 0.0001f)
        {
            _phi = 0f;
        }
        else
        {
            horizontal.Normalize();
            float x = Vector3.Dot(horizontal, _sphereRight);
            float z = Vector3.Dot(horizontal, _sphereForward);
            _phi = Mathf.Atan2(z, x);
            if (_phi < 0f) _phi += 2f * Mathf.PI;
        }
    }

    public override void OnUpdate()
    {
        // 입력
        float xInput = _player.movementInput.x; // 좌/우
        float zInput = _player.movementInput.z; // 상/하

        // ─────────────────────────────────────────────
        // 1) 세로각 θ 업데이트
        //    - z>0 → θ 감소(북극 방면)
        //    - z<0 → θ 증가(남극 방면)
        // ─────────────────────────────────────────────
        float newTheta;
        if (_sphereRadius > 0.0f)
            newTheta = _theta - zInput * (_originSO.moveSpeedTheta / _sphereRadius) * Time.deltaTime;
        else
            newTheta = _theta - zInput * _originSO.moveSpeedTheta * Time.deltaTime;

        // 각도 범위 제한 (옵션)
        if (_originSO.clampTheta)
        {
            newTheta = Mathf.Clamp(newTheta, _originSO.minTheta, _originSO.maxTheta);
        }
        else
        {
            // 기본: [0..π]에서 강제 클램프 (구의 위~아래)
            newTheta = Mathf.Clamp(newTheta, 0f, Mathf.PI);
        }

        // ─────────────────────────────────────────────
        // 2) 수평각 φ 업데이트
        //    - x>0 → φ 증가 (반시계)
        //    - x<0 → φ 감소 (시계)
        // ─────────────────────────────────────────────
        float newPhi;
        if (_sphereRadius > 0.0f)
            newPhi = _phi + xInput * (_originSO.moveSpeedPhi / _sphereRadius) * Time.deltaTime;
        else
            newPhi = _phi + xInput * _originSO.moveSpeedPhi * Time.deltaTime;

        // 꼭대기(θ=0) or 바닥(θ=π) 부근에서 좌우 무시 (옵션)
        // sin(θ)가 0(또는 매우 작으면) => 수평 원 둘레가 거의 없으므로 회전 의미가 없어짐
        if (_originSO.ignoreHorizontalNearPoles)
        {
            float sinVal = Mathf.Sin(newTheta);
            if (Mathf.Abs(sinVal) < _originSO.poleThreshold)
            {
                // 수평 이동(φ변화) 무시
                newPhi = _phi;
            }
        }

        // φ 범위 제한 (옵션)
        if (_originSO.clampPhi)
        {
            newPhi = Mathf.Clamp(newPhi, _originSO.minPhi, _originSO.maxPhi);
        }
        else
        {
            // 기본: 0..2π로 래핑
            if (newPhi < 0f)              newPhi += 2f * Mathf.PI;
            else if (newPhi > 2f*Mathf.PI) newPhi -= 2f * Mathf.PI;
        }

        // 최종 반영
        _theta = newTheta;
        _phi   = newPhi;

        // ─────────────────────────────────────────────
        // 3) 구면좌표 -> 월드좌표 변환
        // ─────────────────────────────────────────────
        float R = _sphereRadius + _playerRadius;

        // sphereRight = x축, sphereForward = z축, sphereUp = y축
        // 전통적 구면좌표: 
        //   x = R * sin(θ)*cos(φ)
        //   z = R * sin(θ)*sin(φ)
        //   y = R * cos(θ)
        Vector3 cartesian =
            _sphereRight   * (R * Mathf.Sin(_theta) * Mathf.Cos(_phi)) +
            _sphereForward * (R * Mathf.Sin(_theta) * Mathf.Sin(_phi)) +
            _sphereUp      * (R * Mathf.Cos(_theta));

        Vector3 newPosition = _sphereCenter + cartesian;
        _transform.position = newPosition;

        // ─────────────────────────────────────────────
        // 4) 회전: Up = 구 표면 노멀
        //    (옵션) transform.right = 이동 방향을 'Forward'로 볼 수도 있음
        // ─────────────────────────────────────────────
        Vector3 newNormal = (newPosition - _sphereCenter).normalized;
        //_transform.up = newNormal;

        // 만약 이번 프레임에서 실제 "이동 방향"에 따라 회전시키고 싶다면:
        // (이전 프레임 위치와 비교 or 직접 구면 상의 속도벡터를 계산)
        // 여기서는 간단히 "φ 변화"로부터 방향 벡터를 유추하거나,
        // 또는 cartesian 자체의 미세 변화량으로 구할 수 있음.
        // 예시: θ,φ가 조금이라도 변했다면 그 변화량 방향을 구해서 '정면' 삼기
        if (_player.movementInput.sqrMagnitude >= ROTATION_TRESHOLD)
        {
            // 간단히 "현재 θ,φ" 부근에서 조금 변위한 위치를 미리 샘플링:
            float smallStep = 0.01f;
            float testTheta = _theta - zInput * smallStep; 
            float testPhi   = _phi   + xInput * smallStep;
            // testTheta,testPhi 보정... (생략)

            Vector3 testCartesian =
                _sphereRight   * (R * Mathf.Sin(testTheta) * Mathf.Cos(testPhi)) +
                _sphereForward * (R * Mathf.Sin(testTheta) * Mathf.Sin(testPhi)) +
                _sphereUp      * (R * Mathf.Cos(testTheta));

            Vector3 testPosition = _sphereCenter + testCartesian;
            Vector3 moveDir = (testPosition - newPosition).normalized; // 예상 이동방향

            // (기본) "Z 축이 이동 방향"이 되도록
            Quaternion targetRot = Quaternion.LookRotation(moveDir, newNormal);

            // (옵션) "X 축이 이동 방향"이 되도록 
            if (_originSO.useRightAsForward)
            {
                targetRot *= Quaternion.Euler(0, -90f, 0);
            }

            //_transform.rotation = targetRot;

            // 3) slerp
            Quaternion finalRot = Quaternion.Slerp(_transform.rotation, targetRot, _originSO.rotationSpeed * Time.deltaTime);

            // 4) 적용 (이 시점에서 transform.up도 newNormal과 일치)
            _transform.rotation = finalRot;
        }
    }
}
