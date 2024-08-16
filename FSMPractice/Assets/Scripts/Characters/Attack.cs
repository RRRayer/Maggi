using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //private void Awake()
    //{
    //    gameObject.SetActive(false);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(gameObject.tag))
        {
            if (other.TryGetComponent(out Damagable damagableComp))
            {
                damagableComp.Die();
            }
        }
    }
}
