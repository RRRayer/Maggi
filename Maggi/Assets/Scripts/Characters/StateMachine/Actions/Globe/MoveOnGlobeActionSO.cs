using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "MoveOnGlobeAction", menuName = "State Machines/Actions/Move On Globe Action")]
public class MoveOnGlobeActionSO : StateActionSO<MoveOnGlobeAction>
{
    public float speed;
}

public class MoveOnGlobeAction : StateAction
{
    private MoveOnGlobeActionSO _originSO => (MoveOnGlobeActionSO)base.OriginSO;
    private Player _player;
    private Transform _transform;
    private InteractiveObject _interactiveObject;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _transform = owner.GetComponent<Transform>();
        _interactiveObject = interactiveObject;
    }

    public override void OnUpdate()
    {
        // 구(InteractiveObject)의 중심과 반지름
        Vector3 sphereCenter = _interactiveObject.transform.position;
        float sphereRadius = _interactiveObject.transform.localScale.x * 0.5f;

        // 플레이어의 반지름
        float playerRadius = _transform.localScale.x * 0.5f;

        // 현재 플레이어 위치 기준 노멀 벡터(구 표면의 Up 방향)
        Vector3 normal = (_transform.position - sphereCenter).normalized;

        // 구 표면 상에서 '전진(상하) 축'과 '우측(좌우) 축'을 정의
        // - sphereForward: 노멀과 플레이어의 오른쪽 축(_transform.right)의 외적
        // - sphereRight: 노멀과 sphereForward의 외적
        Vector3 sphereForward = Vector3.Cross(_transform.forward, normal).normalized;
        Vector3 sphereRight = Vector3.Cross(normal, sphereForward).normalized;

        // 입력 벡터( x: 좌우, y: 상하 )
        Vector2 input = _player.movementInput;

        // sphereMovement = (수평이동 * sphereRight) + (수직이동 * sphereForward)
        Vector3 sphereMovement = (sphereRight * input.x + sphereForward * input.y) * _originSO.speed * Time.deltaTime;

        // 새 위치 = 현재 위치 + 이동 벡터
        Vector3 newPosition = _transform.position + sphereMovement;

        // 새 위치가 구의 표면에 붙도록 보정
        Vector3 newNormal = (newPosition - sphereCenter).normalized;

        // 구 표면에 플레이어가 붙도록 거리(구 반지름 + 플레이어 반지름) 유지
        newPosition = sphereCenter + newNormal * (sphereRadius + playerRadius);

        // 실제 위치 적용
        _transform.position = newPosition;

        // 캐릭터 Up 방향을 구 표면 노멀에 맞추고,
        // Forward 방향을 이번 프레임 이동 방향으로 맞춤
        // (단, 이동량이 거의 없는 경우에는 회전이 급격히 튀지 않도록 보정할 수도 있음)
        if (sphereMovement.sqrMagnitude > 0.0001f)
        {
            _transform.rotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(sphereMovement, newNormal).normalized,
                newNormal
            );
        }
        else
        {
            // 이동이 거의 없는 경우, 그냥 노멀만 맞춰줌 (필요 시 로직 조정)
            _transform.up = newNormal;
        }
    }
}
