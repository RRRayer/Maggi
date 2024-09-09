using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private KeySO _key;

    public string GetKeyID()
    {
        return _key.ID;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
