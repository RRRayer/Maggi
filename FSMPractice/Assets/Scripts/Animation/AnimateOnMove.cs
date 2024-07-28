using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnMove : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3.0f;

    private Vector3 _previousPosition;
    private Vector3 _currentPosition;
    private Transform _meshTransform;

    private void Start()
    {
        //_anim = GetComponent<Animator>();
        //if (_anim == null)
        //{
        //    Debug.LogWarning("There is no Animtor in " + gameObject.name);
        //}

        _meshTransform = transform.GetChild(0);

        _previousPosition = transform.position;
    }

    private void Update()
    {
        _currentPosition = transform.position;
        if (_previousPosition == _currentPosition)
            return;

        Vector3 direction = _currentPosition - _previousPosition;
        if (direction.x > 0) // 오른쪽으로 진행
            _meshTransform.Rotate(0, -_rotateSpeed * Time.deltaTime * 100.0f, 0);
        else if (direction.x < 0)
            _meshTransform.Rotate(0, _rotateSpeed * Time.deltaTime * 100.0f, 0);


        _previousPosition = _currentPosition;
    }
}
