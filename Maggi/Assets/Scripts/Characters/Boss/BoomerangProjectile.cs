using System.Collections;
using UnityEngine;

public class BoomerangProjectile : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;

    [Header("Movement Settings")]
    [SerializeField] private float _moveDuration = 1.0f;
    [SerializeField] private float _arcHeight = 2.0f;

    private TestBoss _boss;
    private Transform _start;
    private Transform _target;
    private Coroutine _currentCoroutine; // 현재 진행 중인 코루틴(Throw or Return)

    public void Init(TestBoss boss, Transform start, Transform target)
    {
        _boss = boss;
        _start = start;
        _target = target;
    }

    public void StartThrow()
    {
        // 혹시 진행 중인 코루틴이 있다면 중단
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        // 이동 코루틴 시작
        _currentCoroutine = StartCoroutine(MoveToTarget(_start, _target));
    }

    private void StartReturn()
    {
        // 혹시 진행 중인 코루틴이 있다면 중단
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        // 이동 코루틴 시작 (시작점/목표점을 바꿔서 사용)
        _currentCoroutine = StartCoroutine(MoveToTarget(_target, _boss.transform));

        // 시간 정상화
        Time.timeScale = 1.0f;

        // 우클릭 이벤트 제거
        _inputReader.PushEvent -= StartReturn;
    }

    private IEnumerator MoveToTarget(Transform startPos, Transform endPos)
    {
        float elapsed = 0f;
        while (elapsed < _moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _moveDuration);

            // 직선 보간
            Vector3 currentPos = Vector3.Lerp(startPos.position, endPos.position, t);

            // 포물선 Y축 오프셋
            float heightOffset = Mathf.Sin(Mathf.PI * t) * _arcHeight;
            currentPos.y += heightOffset;

            transform.position = currentPos;
            yield return null; // 다음 프레임까지 대기
        }

        // 이동 완료 후 최종 위치 보정
        transform.position = endPos.position;

        // 예: 이동이 끝나면 풀로 되돌리는 동작 (보스 측 메서드)
        if (_boss != null)
        {
            _boss.ReturnProjectile(gameObject);

            // 보스가 맞았으면 체력 깎기
            if (endPos == _boss.transform)
            {
                _boss.OnDamaged();
            }
        }

        // player가 맞았으면 카메라 쉐이크, 화면 색상 조정, 사운드 등등..
        if (endPos == _target)
        {
            // Debug.Log("플레이어가 맞음");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 시간 느리게 설정
            Time.timeScale = 0.05f;

            // 우클릭 이벤트 추가
            _inputReader.PushEvent += StartReturn;
        }
    }

    private void OnDisable()
    {
        // 시간 정상화
        Time.timeScale = 1.0f;

        // 우클릭 이벤트 제거
        _inputReader.PushEvent -= StartReturn;
    }
}
