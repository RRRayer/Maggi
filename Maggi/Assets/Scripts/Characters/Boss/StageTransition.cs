using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTransition : MonoBehaviour
{
    [Header("Broadcasting")]
    [SerializeField] private VoidEventChannelSO _stageInitializer = default;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _stageInitializer.OnEventRaised();
            
            // // 필요하다면 없애자
            //gameObject.SetActive(false);
        }
    }
}
