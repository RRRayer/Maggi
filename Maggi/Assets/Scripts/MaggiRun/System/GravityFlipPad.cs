using UnityEngine;

public class GravityFlipPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.FlipGravity(); // ✅ 중력 반전 요청만
            }
        }
    }
}