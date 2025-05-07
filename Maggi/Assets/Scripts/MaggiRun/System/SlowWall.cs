using UnityEngine;

public class SlowWall : MonoBehaviour
{
    // SlowWall.cs (Zone에 부착)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.EnterSlowMode(transform.root.gameObject); 
        }
    }

}