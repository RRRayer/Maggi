using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public void Activate()
    {
        target.SetActive(true);
    }
}
