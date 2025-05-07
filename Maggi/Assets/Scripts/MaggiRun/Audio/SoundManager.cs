using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    public VoidEventChannelSO slowWallClearedEvent;
    public AudioClip slowWallClearSFX;
    public AudioSource audioSource;

    private void OnEnable()
    {
        slowWallClearedEvent.OnEventRaised += PlayClearSFX;
    }

    private void OnDisable()
    {
        slowWallClearedEvent.OnEventRaised -= PlayClearSFX;
    }

    private void PlayClearSFX()
    {
        audioSource.PlayOneShot(slowWallClearSFX);
    }
}
