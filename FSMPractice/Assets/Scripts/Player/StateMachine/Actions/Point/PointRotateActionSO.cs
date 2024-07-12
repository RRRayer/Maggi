using UnityEngine;
using Pudding.StateMachine;
using Pudding.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PointRotateAction", menuName = "State Machines/Actions/Point Rotate Action")]
public class PointRotateActionSO : StateActionSO<PointRotateAction>
{
    public LayerMask PointLayerMask;
    public float rotationSpeed = 2.0f;
}

public class PointRotateAction : StateAction
{
	protected new PointRotateActionSO _originSO => (PointRotateActionSO)base.OriginSO;

    private Player _player;
    private Transform _transform;
	private InteractionManager _interactionManager;


    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _transform = stateMachine.GetComponent<Transform>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnUpdate()
    {
        // 1. Raycast로 _interactionManager.transform의 표면에 수평이 되도록 자기 오브젝트를 회전
        RotateToSurface();

        // 2. 방향키 입력을 통해 _interactionManager.transform의 transform.up 축을 기준으로 주위를 이동
        HandleMovement();
    }

    void RotateToSurface()
    {
        RaycastHit hit;
        Vector3 directionToTarget = (_interactionManager.transform.position - _transform.position).normalized;

        if (Physics.Raycast(_transform.position, directionToTarget, out hit))
        {
            // Raycast로 얻은 표면의 법선 벡터를 이용해 오브젝트를 회전
            Vector3 normal = hit.normal;
            Quaternion targetRotation = Quaternion.FromToRotation(_transform.up, normal) * _transform.rotation;
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, _originSO.rotationSpeed);
        }
    }

    void HandleMovement()
    {
        _transform.RotateAround(_interactionManager.transform.position, _interactionManager.transform.up, _player.movementInput.x * _originSO.rotationSpeed);
    }

}

