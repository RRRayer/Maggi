using UnityEngine;

public class InteractionTriggerMaggiRun : MonoBehaviour
{
    [SerializeField] private InteractionManagerMaggiRun interactionManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionManager.OnTriggerChangeDetected(true, gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionManager.OnTriggerChangeDetected(false, gameObject);
        }
    }
}