using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPrototype : MonoBehaviour
{
    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;
    [SerializeField] private FadeChannelSO _fadeRequestChannel = default;

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
