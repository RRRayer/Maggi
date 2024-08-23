using UnityEngine;

public class BossCatch : MonoBehaviour
{
    [SerializeField]
    public TransformAnchor _playerTransform;

    private Transform _transform;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public void PlayerCatch()
    {
        Debug.Log(_transform.position + "\n" + _playerTransform.Value.position);
        _transform.position = _playerTransform.Value.position;
    }
}
