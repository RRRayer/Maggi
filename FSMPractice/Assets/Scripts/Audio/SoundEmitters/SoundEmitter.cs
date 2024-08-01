using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundEmitter : MonoBehaviour
{
    private AudioSource _audioSource;

    public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool hasToLoop, Vector3 position = default)
    {
        _audioSource.clip = clip;
        settings.ApplyTo(_audioSource);

        _audioSource.transform.position = position;
        _audioSource.loop = hasToLoop;
        _audioSource.time = 0.0f;
        _audioSource.Play();

        if (!hasToLoop)
        {
            StartCoroutine(FinishedPlaying(clip.length));
        }
    }

    private IEnumerator FinishedPlaying(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        NotifyBeingDone();
    }

    private void NotifyBeingDone()
    {
        OnSoundFinishedPlaying.Invoke(this);
    }
}
