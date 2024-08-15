using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSoundEffect : MonoBehaviour
{
    [SerializeField] private AudioCueSO audioCue = default;
    [SerializeField] private AudioCueEventChannelSO audioCueEventChannel = default;
    [SerializeField] private AudioConfigurationSO audioConfiguration = default;

    private void OnCollisionEnter(Collision collision)
    {
        audioCueEventChannel.RaisePlayEvent(audioCue, audioConfiguration, transform.position);
    }
}
