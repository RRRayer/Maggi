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

        if (_previousPosition.z == _currentPosition.z)
            return;

        Vector3 direction = _currentPosition - _previousPosition;
        if (direction.z > 0) // 오른쪽으로 진행
            transform.Rotate(_rotateSpeed * Time.deltaTime * 100.0f, 0, 0);
        else if (direction.z < 0)
            transform.Rotate(-_rotateSpeed * Time.deltaTime * 100.0f, 0, 0);


        _previousPosition = _currentPosition;
    }
}
