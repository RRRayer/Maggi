using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class InteractionEventListener : MonoBehaviour
{
    [SerializeField] private TimelineAsset _timeline = default;

    [Header("Broadcasting on")]
    [SerializeField] private TimelineAssetEventChannelSO _onStartTimeline = default;

    [ReadOnly] private bool _isEnable;
    public bool IsEnable => _isEnable;
    [SerializeField] private KeySO _requiredKey = default;
    public KeySO RequiredKey => _requiredKey;

    // 나중에 보스 트리거 이벤트도 발생 시켜야함.
    // 그걸 위해서 보스 이벤트 채널 하나 만들면 되고.
    // 상호작용 시 타임라인 실행 + 보스 이벤트 또는 타임라인 실행. 인데
    // 보스 이벤트 구현은 오브젝트 앞까지 이동 -> 맞는 타임라인 실행.
    // 그래서 필요한 SO 가
    // 1. 보스 이벤트 채널   2. 보스 상호작용 타임라인   3. 그냥 상호작용 타임라인\
    // 아니면 그냥 타임라인 하나에 자기 애니메이션, 보스 애니메이션 둘다 넣음 될 일 이잖어 일단 이렇게 가

    public void OnInteract()
    {
        if (_isEnable)
            _onStartTimeline.RaiseEvent(_timeline);
    }    
}
