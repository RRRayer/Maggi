using UnityEngine;

public class InteractionManagerMaggiRun : MonoBehaviour
{
    [SerializeField] private InputReaderMaggiRun _inputReader = default;

    private GameObject currentSlowWall = null;
    private PlayerMaggiRun player;

    private void Awake()
    {
        player = GetComponent<PlayerMaggiRun>();
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
        if (currentSlowWall != null)
        {
            player.ExitSlowMode();
            Destroy(currentSlowWall);
            currentSlowWall = null;
        }
    }

    public void OnTriggerChangeDetected(bool entered, GameObject obj)
    {
        if (!entered)
        {
            if (currentSlowWall == obj)
                currentSlowWall = null;
            return;
        }

        if (obj.CompareTag("SlowWall"))
        {
            currentSlowWall = obj;
        }
    }
}