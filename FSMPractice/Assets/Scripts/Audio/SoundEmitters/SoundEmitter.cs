using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

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

    public void FadeMusicIn(AudioClip musicClip, AudioConfigurationSO settings, float duration, float startTime = 0f)
    {
        PlayAudioClip(musicClip, settings, true);
        _audioSource.volume = 0f;

        //Start the clip at the same time the previous one left, if length allows
        //TODO: find a better way to sync fading songs
        if (startTime <= _audioSource.clip.length)
            _audioSource.time = startTime;

        _audioSource.DOFade(settings.Volume, duration);
    }

    public float FadeMusicOut(float duration)
    {
        _audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;

        return _audioSource.time;
    }

    private void OnFadeOutComplete()
    {
        NotifyBeingDone();
    }

    /// <summary>
	/// Used to check which music track is being played.
	/// </summary>
	public AudioClip GetClip()
    {
        return _audioSource.clip;
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void Finish()
    {
        if (_audioSource.loop)
        {
            _audioSource.loop = false;
            float timeRemaining = _audioSource.clip.length - _audioSource.time;
            StartCoroutine(FinishedPlaying(timeRemaining));
        }
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    public bool IsLooping()
    {
        return _audioSource.loop;
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
