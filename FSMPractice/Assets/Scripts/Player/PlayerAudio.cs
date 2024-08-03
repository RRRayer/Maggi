using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : CharacterAudio
{
    [SerializeField] private AudioCueSO _liftoff;
    [SerializeField] private AudioCueSO _land;
    [SerializeField] private AudioCueSO _footsteps;
    [SerializeField] private AudioCueSO _die;

    public void PlayFootstep() => PlayAudio(_footsteps, _audioConfig, transform.position);
    public void PlayJumpLiftoff() => PlayAudio(_liftoff, _audioConfig, transform.position);
    public void PlayJumpLand() => PlayAudio(_land, _audioConfig, transform.position);
    public void PlayDie() => PlayAudio(_die, _audioConfig, transform.position);
}
