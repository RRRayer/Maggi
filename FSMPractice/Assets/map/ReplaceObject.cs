using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceObject : MonoBehaviour
{
    [SerializeField] private GameObject _targetPrefab;

    private GameObject _originalObject;

    void Start()
    {
        _originalObject = gameObject;
    }

    public void ChangeObject()
    {
        if (_targetPrefab != null)
        {
            Vector3 position = _originalObject.transform.position;
            Quaternion rotation = _originalObject.transform.rotation;

            Destroy(_originalObject);

            GameObject newObject = Instantiate(_targetPrefab, position, rotation);

            _originalObject = newObject;
        }
    }
}
