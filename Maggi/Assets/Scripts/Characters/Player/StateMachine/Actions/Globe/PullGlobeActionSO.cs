using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "PullGlobeAction", menuName = "State Machines/Actions/Pull Globe Action")]
public class PullGlobeActionSO : StateActionSO<PullGlobeAction> { }

public class PullGlobeAction : StateAction
{
    private Transform _transform;
	private InteractiveObject _interactiveObject;

    public override void Awake(InteractiveObject interactiveObject, GameObject owner)
    {
        _transform = owner.GetComponent<Transform>();
        _interactiveObject = interactiveObject;
    }

    public override void OnStateEnter()
    {
        float globeRadius = _interactiveObject.transform.localScale.x * 0.5f;
        float playerRadius = _transform.localScale.x * 0.5f;
        float offset = playerRadius + globeRadius;

        // Calculation object's normal vector
        Vector3 globeNormal = (_transform.position - _interactiveObject.transform.position).normalized;

        // Adjusts player up vector to align with the object's normal vector
        Vector3 forwardVector = Vector3.Cross(-_transform.forward, globeNormal);
        _transform.rotation = Quaternion.LookRotation(forwardVector, globeNormal);

        // Adjusts player position to stick on object's surface
        _transform.position = _interactiveObject.transform.position + _transform.up * offset;
    }

    public override void OnUpdate() { }
}
