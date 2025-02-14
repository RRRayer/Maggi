using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss : MonoBehaviour
{
    [SerializeField] private Transform[] _spots;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private int _health = 3;

    public bool _isThrowing = true;
    private int _currentSpotIndex = 0;

    private Stack<GameObject> _pool = new Stack<GameObject>();
    private int _poolSize = 5;

    private void Awake()
    {
        for (int i=0; i<_poolSize; ++i)
        {
            GameObject obj = Instantiate(_projectilePrefab, transform);
            obj.SetActive(false);
            _pool.Push(obj);
        }
    }

    private void Update()
    {
        if (_target == null)
        {
            _target = FindFirstObjectByType<Player>()?.transform;
        }

        // Projectile�� ������ Target ��ġ�� �̵���Ŵ
        if (!_isThrowing)
        {
            _isThrowing = true;

            // Ǯ���� ������ ������
            Transform currentSpot = _spots[_currentSpotIndex];
            GameObject newProjectile = GetProjectile();
            newProjectile.transform.position = currentSpot.position;

            // ������ ������ �������� ���� ����
            BoomerangProjectile boomerang = newProjectile.GetComponent<BoomerangProjectile>();
            boomerang.Init(this, start : _spots[_currentSpotIndex], target : _target);
            boomerang.StartThrow();

            // ���� ������ ������Ʈ�� ��ġ�� �ٲ�
            _currentSpotIndex = (_currentSpotIndex + 1) % _spots.Length;
        }
    }
    
    private GameObject GetProjectile()
    {
        if (_pool.Count > 0)
        {
            GameObject projectile = _pool.Pop();
            projectile.SetActive(true);
            return projectile;
        }
        else
        {
            Debug.LogWarning("Pool is empty! Consider increasing pool size or handle this case.");
            GameObject obj = Instantiate(_projectilePrefab, transform);
            obj.SetActive(true);
            return obj;
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        projectile.transform.SetParent(transform, false); // Ǯ �θ� ������ �̵�
        _pool.Push(projectile);
    }

    public void OnDamaged()
    {
        _health -= 1;
        if (_health <= 0)
        {
            Debug.Log("���� ����");
        }
    }
}