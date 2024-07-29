using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceObject : MonoBehaviour
{
    public GameObject TargetPrefab;

    private GameObject _OriginalObject;

    [SerializeField]
    public bool IsChanged = false;

    void ChangeObject()
    {
        
        if (TargetPrefab != null)
        {
            Vector3 position = _OriginalObject.transform.position;
            Quaternion rotation = _OriginalObject.transform.rotation;

            Destroy(_OriginalObject);

            GameObject newObject = Instantiate(TargetPrefab, position, rotation);

            newObject.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);

            _OriginalObject = newObject;
        }
    }

    void Start()
    {
        _OriginalObject = this.gameObject;
        IsChanged = false;
    }


    void OnValidate()
    {
        if (IsChanged == true)
        {
            ChangeObject();
        }
    }
}
