using Maggi.StateMachine.ScriptableObjects;
using UnityEngine;

public class EnhancedPullTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _cube;

    private bool _triggerFlag = false;
    private InteractionManager _interactionManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _interactionManager = other.GetComponent<InteractionManager>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_interactionManager.pushInput)
                _triggerFlag = false;

            if (_triggerFlag)
                return;

            if (_interactionManager != null)
            {
                if (_interactionManager.pullInput)
                {
                    _triggerFlag = true;
                    _interactionManager.currentInteractiveObject = _cube;
                    _interactionManager.currentInteractionType = InteractionType.Possession;
                }
            }
        }     
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _triggerFlag = false;
        }
    }
}
