using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
        public List<Transform[]> patrolAreas = new List<Transform[]>();
        public Mode CurrentMode => _currentMode;
        public Transform Target => _target;
        public int CurrentRootIndex { set { _currentRootIndex = value; } get { return _currentRootIndex; } }

        [HideInInspector] public PlayableDirector timelineDirector = default;
        [HideInInspector] public bool isTrigger = false;
        public GameObject hand;

        [SerializeField] private List<Transform> patrolAreaRoot; // patrol area에 저장된 위치로 walk 한다.
        [SerializeField] private Transform _target;
        [SerializeField] private Mode _currentMode = Mode.Idle;        
        private int _currentRootIndex;
        private NavMeshAgent _agent;
        

        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO _stageTransition = default; // 다음 스테이지로 바꾸고 모드를 초기화 한다.
        [SerializeField] private TransformEventChannelSO _moveToTargetEvent = default; // target position으로 이동한다.
        [SerializeField] private TimelineAssetEventChannelSO _setTimelineAssetEvent = default; // 타임라인을 재생한다.

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
            _agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            _stageTransition.OnEventRaised += Init;
            _moveToTargetEvent.OnEventRaised += DetectTarget;
            _setTimelineAssetEvent.OnEventRaised += InitializeTimeline;
        }

        private void OnDisable()
        {
            _stageTransition.OnEventRaised -= Init;
            _moveToTargetEvent.OnEventRaised -= DetectTarget;
            _setTimelineAssetEvent.OnEventRaised -= InitializeTimeline;
        }

        private void Update()
        {
            //Debug.Log($"실시간 모드 확인 {_currentMode}");
        }

        private void DetectTarget(Transform target)
        {
            // detect action
            _target = target;
            isTrigger = true;
            SetMode(Mode.Detect, "detecttarger");
        }

        private void InitializeTimeline(TimelineAsset timeline)
        {
            timelineDirector.playableAsset = timeline;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is AnimationTrack animationTrack)
                {
                    timelineDirector.SetGenericBinding(track, gameObject);
                }
            }
        }

        private void Init()
        {
            _currentMode = Mode.Idle;
            // Patrol next area
            _currentRootIndex = (_currentRootIndex + 1) % patrolAreaRoot.Count;
        }

        public void SetMode(Mode newMode, string org)
        {
            _currentMode = newMode;
            Debug.Log($"현재 모드 : {_currentMode}, 출처 : {org}");
        }

        public void OnTriggerChangeDetected(bool entered, GameObject obj)
        {
            if (entered && obj.CompareTag("Player"))
            {
                // 여기서 Ray를 쏴서 장애물 없는지 확인 해야해
                {
                    _target = obj.transform;
                    SetMode(Mode.Detect, "OnTriggerChangeDetected");
                }
            }
        }

        public void OnTriggerChangeCatched(bool entered, GameObject obj)
        {
            if (entered && obj.CompareTag("Player"))
            {
                _target = obj.transform;
                SetMode(Mode.Catch, "ontriggherchangecathed");
            }
        }

        public bool IsStopped()
        {
            if (!_agent.pathPending                                         // 경로 계산이 완료되었고
                && _agent.remainingDistance <= _agent.stoppingDistance      // 목표 지점까지 남은 거리가 stoppingDistance 이하이며
                && (!_agent.hasPath || _agent.velocity.sqrMagnitude <= 0f)) // 이동 중이 아니면
            {
                return true;
            }
            return false;
        }
    }
}
