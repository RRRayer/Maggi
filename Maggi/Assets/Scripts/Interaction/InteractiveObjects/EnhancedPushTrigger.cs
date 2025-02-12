using UnityEngine;

public class EnhancedPushTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private float pushSpeed = 5f;  // 원하는 이동 속도

    private InteractionManager _interactionManager = default;
    private Player _player = default;
    private CharacterController _controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _interactionManager = other.GetComponent<InteractionManager>();
            _player = other.GetComponent<Player>();
             _controller = other.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_interactionManager != null)
            {
                if (_interactionManager.pushInput)
                {
                    MovePlayerToTarget();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

    private void MovePlayerToTarget()
    {
        if (_controller == null)
        {
            Debug.LogWarning("There is no character controller");
            return;
        }

        Vector3 movement = direction * pushSpeed * Time.deltaTime;
        _controller.Move(movement);

        _player.movementVector.y = 0.0f;
    }
}
