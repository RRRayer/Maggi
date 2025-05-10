using UnityEngine;
using System.Collections.Generic;

public class InteractionManagerMaggiRun : MonoBehaviour
{
    [SerializeField] private InputReaderMaggiRun _inputReader = default;

    [ReadOnly] public GameObject currentInteractiveObject;
    private LinkedList<GameObject> _potentialInteractions = new LinkedList<GameObject>();

    private PlayerMaggiRun _player;

    private void Awake()
    {
        _player = GetComponent<PlayerMaggiRun>();
    }

    private void OnEnable()
    {
        _inputReader.AttackEvent += OnAttackInitiated;
    }

    private void OnDisable()
    {
        _inputReader.AttackEvent -= OnAttackInitiated;
    }

    private void OnAttackInitiated()
    {
        if (currentInteractiveObject != null && currentInteractiveObject.CompareTag("SlowWall"))
        {
            _player.ExitSlowMode();
            Destroy(currentInteractiveObject);
            currentInteractiveObject = null;
        }
    }

    public void OnTriggerChangeDetected(bool entered, GameObject obj)
    {
        if (entered)
        {
            AddPotentialInteraction(obj);
        }
        else
        {
            RemovePotentialInteraction(obj);
        }
    }

    private void AddPotentialInteraction(GameObject obj)
    {
        if (obj.CompareTag("SlowWall")) // 추후 확장 가능
        {
            _potentialInteractions.AddFirst(obj);
            currentInteractiveObject = obj;
        }
    }

    private void RemovePotentialInteraction(GameObject obj)
    {
        var node = _potentialInteractions.First;

        while (node != null)
        {
            if (node.Value == obj)
            {
                var toRemove = node;
                node = node.Next;
                _potentialInteractions.Remove(toRemove);
                break;
            }
            else
            {
                node = node.Next;
            }
        }

        // 현재 상호작용 대상이 사라졌다면 갱신
        if (currentInteractiveObject == obj)
        {
            currentInteractiveObject = _potentialInteractions.Count > 0 ? _potentialInteractions.First.Value : null;
        }
    }
}