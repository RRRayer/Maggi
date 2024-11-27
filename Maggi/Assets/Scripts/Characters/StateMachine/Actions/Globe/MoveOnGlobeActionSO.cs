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

    //public override void OnUpdate()
    //{
    //    _player.movementVector.x = _player.movementInput.x * _originSO.speed;
    //    _player.movementVector.y = _player.movementInput.y * _originSO.speed;

    //    float globeRadius = _interactiveObject.transform.localScale.x * 0.5f;
    //    float playerRadius = _transform.localScale.x * 0.5f;
    //    float offset = playerRadius + globeRadius;

    //    // Calculation object's normal vector
    //    Vector3 globeNormal = (_transform.position - _interactiveObject.transform.position).normalized;

    //    // Adjusts player up vector to align with the object's normal vector
    //    Vector3 forwardVector = Vector3.Cross(-_transform.forward, globeNormal);
    //    _transform.rotation = Quaternion.LookRotation(forwardVector, globeNormal);

    //    // Adjusts player position to stick on object's surface
    //    _transform.position = _interactiveObject.transform.position + _transform.up * offset;
    //}

    Vector3 ProjectMovementOntoSphere(Vector3 movementVector, Vector3 sphereCenter, float sphereRadius)
    {
        // 현재 위치에서 구의 중심으로 향하는 방향 벡터 계산
        Vector3 surfaceNormal = (_transform.position - sphereCenter).normalized;

        // 월드 공간의 이동 벡터를 로컬 공간으로 변환
        Vector3 localMovement = _transform.InverseTransformDirection(movementVector);

        // 로컬 공간에서 구면 표면에 투영된 이동 벡터 계산
        Vector3 projectedLocalMovement = Vector3.ProjectOnPlane(localMovement, surfaceNormal);

        // 다시 월드 공간으로 변환
        Vector3 projectedMovement = _transform.TransformDirection(projectedLocalMovement);

        // 구면 표면 위의 새 위치 계산
        Vector3 newPosition = sphereCenter + surfaceNormal * sphereRadius + projectedMovement;

        return newPosition - _transform.position;
    }
    
    public override void OnUpdate()
    {
        Vector3 sphereCenter = _interactiveObject.transform.position;
        float sphereRadius = _interactiveObject.transform.localScale.x * 0.5f;
        Vector3 surfaceNormal = (_transform.position - sphereCenter).normalized;

        float playerRadius = _transform.localScale.x * 0.5f;
        float offset = playerRadius + sphereRadius;

        Vector3 sphereSurfaceMovement = ProjectMovementOntoSphere(_player.movementInput, sphereCenter, sphereRadius);
        _player.movementVector = sphereSurfaceMovement;

        // Adjusts player up vector to align with the object's normal vector
        Vector3 forwardVector = Vector3.Cross(-_transform.forward, surfaceNormal);
        _transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(_player.movementInput, Vector3.right).normalized, surfaceNormal);

        // Adjusts player position to stick on object's surface
        _transform.position = _interactiveObject.transform.position + _transform.up * offset;
    }
}
