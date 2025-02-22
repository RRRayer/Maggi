using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullLightAction", menuName = "State Machines/Actions/PullLightAction")]
public class PullLightActionSO : StateActionSO<PullLightAction> 
{
    public LayerMask mouseClickLayerMask;
}

public class PullLightAction : StateAction
{
	protected new PullLightActionSO _originSO => (PullLightActionSO)base.OriginSO;
	private Player _player;
	private InteractionManager _interactionManager;
    private InteractiveObject _interactiveObject;
    private Collider _interactiveObjectCollider;

	public override void Awake(StateMachine stateMachine)
	{
		_player = stateMachine.GetComponent<Player>();
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _interactionManager = owner.GetComponent<InteractionManager>();
        _interactiveObject = interactiveObject;
    }

    public override void OnStateEnter()
    {
        _interactiveObjectCollider = _interactionManager.currentInteractiveObject.GetComponent<Collider>();
        _interactiveObjectCollider.enabled = false;

        // When Pulling Light Object, it needs to remove from list of Potential Interactions
        _interactionManager.OnTriggerChangeDetected(false, _interactionManager.currentInteractiveObject);
    }

    public override void OnUpdate()
    {
        // 플레이어 높이와 동일한 y 위치의 평면 생성
        Plane plane = new Plane(Vector3.up, _player.transform.position);

        // 카메라에서 마우스 위치로의 Ray 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Ray가 평면과 교차하는 지점 계산
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(distance);

            // 오브젝트 위치 및 방향 조정
            Vector3 direction = (mouseWorldPosition - _player.transform.position).normalized;
            _interactiveObject.transform.position = _player.transform.position + direction * 0.4f + Vector3.up * 0.2f;
            _interactiveObject.transform.LookAt(mouseWorldPosition);
        }
    }

    public override void OnStateExit()
    {
        _interactiveObjectCollider.enabled = true;
    }
}
