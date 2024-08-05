using UnityEngine;

public class AnimateOnMove : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3.0f;

    private Vector3 _previousPosition;
    private Vector3 _currentPosition;

    private void Start()
    {
        _previousPosition = transform.parent.position;
    }

    private void Update()
    {
        _currentPosition = transform.parent.position;

        Vector3 moveDirection = (_currentPosition - _previousPosition).normalized;

        // 이동 방향이 거의 없는 경우 무시
        if (moveDirection.magnitude < 0.005f)
            return;

        // 부모 오브젝트의 회전을 무시하고 월드 좌표계에서의 이동 방향을 사용하여 회전
        float dotProduct = Vector3.Dot(transform.parent.forward, moveDirection);

        // 진행 방향에 따라 자식 오브젝트 회전
        if (dotProduct > 0) // 전방으로 진행
        {
            transform.Rotate(Vector3.right, _rotateSpeed * Time.deltaTime * 100.0f);
        }
        else if (dotProduct < 0) // 후방으로 진행
        {
            transform.Rotate(Vector3.right, -_rotateSpeed * Time.deltaTime * 100.0f);
        }

        _previousPosition = _currentPosition;
    }
}
