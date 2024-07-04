using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool, GameObject> { } 

public class ZoneTriggerController : MonoBehaviour
{
    [SerializeField] private BoolEvent _enterZone = default;
    [SerializeField] private LayerMask _layers = default;

    private void OnTriggerEnter(Collider other)
    {
        // _layers는 여러 layer를 복수 선택할 수 있기 때문에 아래처럼 Bit 연산한다.
        if ((1 << other.gameObject.layer & _layers) != 0)
        {
            _enterZone.Invoke(true, other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & _layers) != 0)
        {
            _enterZone.Invoke(false, other.gameObject);
        }
    }
}
