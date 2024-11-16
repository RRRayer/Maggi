using UnityEngine;
using Maggi.StateMachine;
using Maggi.StateMachine.ScriptableObjects;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PushKeyAction", menuName = "State Machines/Actions/Push Key Action")]
public class PushKeyActionSO : StateActionSO<PushKeyAction>
{
    public float pushForce = 5.0f;
    public float pushHeight = 50.0f;
}

public class PushKeyAction : StateAction
{
    private PushKeyActionSO _originSO => (PushKeyActionSO)base.OriginSO;
    private InteractionManager _interactionManager;
    private Rigidbody _interactiveObjectRigidbody;

    public override void Awake(StateMachine stateMachine)
    {
        _interactionManager = stateMachine.GetComponent<InteractionManager>();
    }

    public override void OnUpdate() { }

    public override void OnStateEnter()
    {
        // 플레이어가 들고 있는 키와 상호작용할 수 있는 오브젝트 감지


        if (_interactionManager.currentInteractionType == InteractionType.None)
        {
            GameObject currentObject = _interactionManager.currentInteractiveObject;

            Collider[] hitColliders = Physics.OverlapSphere(currentObject.transform.position, 1.0f);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Normal"))
                {
                    if (hitCollider.TryGetComponent(out InteractionEventListener e) && currentObject.TryGetComponent(out Key key))
                    {
                        List<InteractionEventListener> listeners = new List<InteractionEventListener>(hitCollider.GetComponents<InteractionEventListener>());
                        Debug.Log(listeners.Count);
                        foreach (var listener in listeners)
                        {
                            if (listener.RequiredKey.ID == key.GetKeyID())
                            {
                                Debug.Log("키랑 동일");
                                listener.IsEnable = true;
                                InteractWithObject(hitCollider.gameObject, key);
                                continue;
                            }
                            else
                            {
                                Debug.Log("키 다름");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("There are no Interaction Event Listener or Key _ PushKeyActionSO.cs");
                    }
                }
            }

            // 상호작용 못 하면 그냥 던지는 동작
            _interactiveObjectRigidbody = _interactionManager.currentInteractiveObject.GetComponent<Rigidbody>();

            // Init Position to Player position and Add
            _interactiveObjectRigidbody.transform.position = _interactionManager.transform.position + _interactiveObjectRigidbody.transform.forward * 0.2f;
            _interactiveObjectRigidbody.velocity = _interactiveObjectRigidbody.transform.forward * _originSO.pushForce + _interactiveObjectRigidbody.transform.up * _originSO.pushHeight;
        }
    }

    private void InteractWithObject(GameObject target, Key key)
    {
        // 키 삭제
        key.Destroy();

        // 오브젝트 교체, 타임라인도 수정해야함
        if (target.TryGetComponent(out ActivateObject activeObject))
        {
            activeObject.Activate();
        }
    }

    public override void OnStateExit()
    {
        _interactionManager.currentInteractionType = InteractionType.None;
        _interactionManager.currentInteractiveObject = null;
    }
}