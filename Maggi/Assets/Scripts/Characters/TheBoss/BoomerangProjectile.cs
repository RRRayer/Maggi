using System.Collections;
using UnityEngine;

public class BoomerangProjectile : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveDuration = 1.0f;
    [SerializeField] private float _arcHeight = 2.0f;

    [Header("References")]
    [SerializeField] private TestBoss _boss;

    private Transform _start;
    private Transform _target;

    private Coroutine _currentCoroutine; // 현재 진행 중인 코루틴(Throw or Return)

    public void Init(TestBoss boss, Transform target, Transform start = default)
    {
        _boss = boss;
        _start = start;
        _target = target;
    }

    public void SetStartPoint(Transform start)
    {
        _start = start;
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
        _currentCoroutine = StartCoroutine(MoveToTarget(_target, _start));
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //// 현재 진행 중이던 Throw 코루틴을 중단하고 Return 코루틴으로
            //StartReturn();

            // 카메라 쉐이크, 화면 색상 조정, 사운드 등등..
        }
    }
}
