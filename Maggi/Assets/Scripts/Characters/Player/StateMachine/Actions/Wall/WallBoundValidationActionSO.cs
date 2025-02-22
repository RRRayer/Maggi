using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "WallBoundValidationAction", menuName = "State Machines/Actions/Wall Bound Validation Action")]
public class WallBoundValidationActionSO : StateActionSO
{
    public LayerMask wallLayerMask;

	protected override StateAction CreateAction() => new WallBoundValidationAction();
}

public class WallBoundValidationAction : StateAction
{
	private new WallBoundValidationActionSO _originSO => (WallBoundValidationActionSO)base.OriginSO;

    private Player _player = default;
    private InteractionManager _interactionManager = default;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _player = owner.GetComponent<Player>();
        _interactionManager = owner.GetComponent<InteractionManager>();
    }

    public override void OnUpdate()
    {
        Ray ray = new Ray(_player.transform.position - _player.transform.right * 0.2f, -_player.transform.up);
        Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2.0f, _originSO.wallLayerMask))
            return;

        _interactionManager.currentInteractionType = InteractionType.None;
        _interactionManager.currentInteractiveObject = null;
    }
}
