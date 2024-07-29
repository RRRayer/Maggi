using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "PlayableDirector", menuName = "Timeline/PlayableDirector")]
public class PlayableDirectorSO : DescriptionBaseSO
{
    [SerializeField] private TimelineAsset _playable = default;
}
