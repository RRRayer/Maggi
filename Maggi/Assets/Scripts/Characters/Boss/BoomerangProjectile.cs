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
    private Coroutine _currentCoroutine; // ���� ���� ���� �ڷ�ƾ(Throw or Return)

    public void Init(TestBoss boss, Transform start, Transform target)
    {
        _boss = boss;
        _start = start;
        _target = target;
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
        _currentCoroutine = StartCoroutine(MoveToTarget(_target, _boss.transform));

        // �ð� ����ȭ
        Time.timeScale = 1.0f;

        // ��Ŭ�� �̺�Ʈ ����
        _inputReader.PushEvent -= StartReturn;
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

            // ������ �¾����� ü�� ���
            if (endPos == _boss.transform)
            {
                _boss.OnDamaged();
            }
        }

        // player�� �¾����� ī�޶� ����ũ, ȭ�� ���� ����, ���� ���..
        if (endPos == _target)
        {
            // Debug.Log("�÷��̾ ����");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �ð� ������ ����
            Time.timeScale = 0.05f;

            // ��Ŭ�� �̺�Ʈ �߰�
            _inputReader.PushEvent += StartReturn;
        }
    }

    private void OnDisable()
    {
        // �ð� ����ȭ
        Time.timeScale = 1.0f;

        // ��Ŭ�� �̺�Ʈ ����
        _inputReader.PushEvent -= StartReturn;
    }
}
