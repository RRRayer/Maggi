using UnityEngine;

public class AnimateOnMove : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3.0f;

    private Vector3 _previousPosition;
    private Vector3 _currentPosition;

    private void Start()
    {
        _previousPosition = base.transform.position;
    }

    private void Update()
    {
        _currentPosition = base.transform.position;

        if (_previousPosition == _currentPosition)
            return;

        Vector3 direction = _currentPosition - _previousPosition;
        if (direction.x > 0) // 오른쪽으로 진행
            transform.Rotate(0, -_rotateSpeed * Time.deltaTime * 100.0f, 0);
        else if (direction.x < 0)
            transform.Rotate(0, _rotateSpeed * Time.deltaTime * 100.0f, 0);


        _previousPosition = _currentPosition;
    }
}
