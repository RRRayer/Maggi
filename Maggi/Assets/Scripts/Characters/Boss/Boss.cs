using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Maggi.Character.Boss
{
    public enum Mode
    {
        Idle, Walk, Detect, Trigger, Catch
    }

    public class Boss : MonoBehaviour
    {
        // Logic
        [SerializeField] private List<Transform> patrolAreaRoot; // patrol area에 저장된 위치로 walk 한다.
        private Mode _currentMode = Mode.Idle;
        private Transform _target;
        private int _currentRootIndex;

        public List<Transform[]> patrolAreas = new List<Transform[]>();
        public Mode CurrentMode => _currentMode;
        public Transform Target => _target;
        public int CurrentRootIndex { set { _currentRootIndex = value; } get { return _currentRootIndex; } }

        // Flag
        public bool isTrigger = false;

        // Timeline
        //[HideInInspector] 
        public TimelineAsset currentTimeline;
        //[HideInInspector] 
        public PlayableDirector timelineDirector = default;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO _stageTransition = default; // 다음 스테이지로 바꾸고 모드를 초기화 한다.
        [SerializeField] private TransformEventChannelSO _moveToTargetEvent = default; // target position으로 이동한다.
        public TimelineAssetEventChannelSO timelineEvent = default; // 타임라인을 재생한다.

        private void Awake()
        {
            foreach (var item in patrolAreaRoot)
            {
                Transform[] children = new Transform[item.childCount];

                for (int i = 0; i < item.childCount; i++)
                {
                    children[i] = item.GetChild(i);
                }

                patrolAreas.Add(children);
            }

            timelineDirector = GetComponent<PlayableDirector>();
        }

        private void OnEnable()
        {
            _stageTransition.OnEventRaised += Init;
            _moveToTargetEvent.OnEventRaised += MoveToTarget;
            timelineEvent.OnEventRaised += InitializeTimeline;
        }

        private void OnDisable()
        {
            _stageTransition.OnEventRaised -= Init;
            _moveToTargetEvent.OnEventRaised -= MoveToTarget;
            timelineEvent.OnEventRaised -= InitializeTimeline;
        }

        private void MoveToTarget(Transform target)
        {
            // detect action
            _target = target;
            SetMode(Mode.Detect);
        }

        private void InitializeTimeline(TimelineAsset timeline)
        {
            timelineDirector.playableAsset = timeline;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is AnimationTrack animationTrack)
                {
                    Debug.Log(track);
                    timelineDirector.SetGenericBinding(track, gameObject);
                }
            }

            isTrigger = true;

            // 이건 trigger action에서 실행
            //_timelineDirector.Play();
        }

        private void Init()
        {
            Debug.Log("보스 초기화");
            _currentMode = Mode.Idle;
            // Patrol next area
            _currentRootIndex = (_currentRootIndex + 1) % patrolAreaRoot.Count;
        }

        public void SetMode(Mode newMode)
        {
            _currentMode = newMode;
            Debug.Log($"현재 모드 : {newMode}");
        }

        public void OnTriggerChangeDetected(bool entered, GameObject obj)
        {
            if (entered && obj.CompareTag("Player"))
            {
                Debug.Log("플레이어 감지");
                _target = obj.transform;
                SetMode(Mode.Detect);
            }
        }
    }

}
