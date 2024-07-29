using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeactivator : MonoBehaviour
{
    [SerializeField] private float _timeToDeactivate = 5.0f;

    private void OnEnable()
    {
        StartCoroutine(DeactivateObject());
    }

    private IEnumerator DeactivateObject()
    {
        yield return new WaitForSeconds(_timeToDeactivate);

        gameObject.SetActive(false);
    }
}
