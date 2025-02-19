using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Conveyor : MonoBehaviour
{

    [SerializeField, Range(min: 0f, max: float.MaxValue)] private float _speed = 1f;
    private HashSet<GameObject> _objectsOnConveyor = new(); // 효율적인 삭제를 위해 HashSet 사용

    private void FixedUpdate()
    {
        foreach (var obj in _objectsOnConveyor)
        {
            obj.transform.position += transform.forward * (_speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _objectsOnConveyor.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        _objectsOnConveyor.Remove(other.gameObject);
    }
}