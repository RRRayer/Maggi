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

    private Coroutine _currentCoroutine; // ���� ���� ���� �ڷ�ƾ(Throw or Return)

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
        // Ȥ�� ���� ���� �ڷ�ƾ�� �ִٸ� �ߴ�
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        // �̵� �ڷ�ƾ ����
        _currentCoroutine = StartCoroutine(MoveToTarget(_start, _target));
    }

    private void StartReturn()
    {
        // Ȥ�� ���� ���� �ڷ�ƾ�� �ִٸ� �ߴ�
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        // �̵� �ڷ�ƾ ���� (������/��ǥ���� �ٲ㼭 ���)
        _currentCoroutine = StartCoroutine(MoveToTarget(_target, _start));
    }

    private IEnumerator MoveToTarget(Transform startPos, Transform endPos)
    {
        float elapsed = 0f;
        while (elapsed < _moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _moveDuration);

            // ���� ����
            Vector3 currentPos = Vector3.Lerp(startPos.position, endPos.position, t);

            // ������ Y�� ������
            float heightOffset = Mathf.Sin(Mathf.PI * t) * _arcHeight;
            currentPos.y += heightOffset;

            transform.position = currentPos;
            yield return null; // ���� �����ӱ��� ���
        }

        // �̵� �Ϸ� �� ���� ��ġ ����
        transform.position = endPos.position;

        // ��: �̵��� ������ Ǯ�� �ǵ����� ���� (���� �� �޼���)
        if (_boss != null)
        {
            _boss.ReturnProjectile(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //// ���� ���� ���̴� Throw �ڷ�ƾ�� �ߴ��ϰ� Return �ڷ�ƾ����
            //StartReturn();

            // ī�޶� ����ũ, ȭ�� ���� ����, ���� ���..
        }
    }
}
