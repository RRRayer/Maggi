using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private int count = 3;

    private bool isBroken = false;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return;

        if (collision.gameObject.CompareTag("Light"))
        {
            count -= 1;
            if (count <= 0)
            {
                isBroken = true;
                Broken();
            }
        }
    }

    private void Broken()
    {
        _collider.enabled = false;
    }
}
