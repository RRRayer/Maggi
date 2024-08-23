using UnityEngine;

public class BossSight : MonoBehaviour
{
    [SerializeField]
    private GameObject player; // 플레이어 오브젝트를 참조하는 변수
    public float sightRange = 50f; // 시야 거리
    public int numberOfRays = 5; // 레이의 개수

    void Start()
    {
        // 0초 후, 0.5초마다 CheckForPlayer 메서드를 호출
        InvokeRepeating("CheckForPlayer", 0f, 0.5f);
    }

    void CheckForPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Debug.Log("Player found.");
            }
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.Log("Player not found.");
            return; // 플레이어가 없다면 더 이상 진행하지 않음
        }

        // 플레이어가 존재하는 경우 감지 로직 실행
        DetectPlayer();
    }

    void DetectPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // 다중 레이캐스트로 플레이어 감지
        for (int i = 0; i < numberOfRays; i++)
        {
            Vector3 offset = new Vector3(Mathf.Sin(i * Mathf.PI / (numberOfRays - 1) - Mathf.PI / 2), 0, Mathf.Cos(i * Mathf.PI / (numberOfRays - 1) - Mathf.PI / 2)) * 0.5f;
            Vector3 rayOrigin = transform.position + offset;
            Ray ray = new Ray(rayOrigin, directionToPlayer);
            RaycastHit hit;

            // 레이 시각화 (노란색)
            Debug.DrawRay(ray.origin, ray.direction * sightRange, Color.yellow);

            if (Physics.Raycast(ray, out hit, sightRange))
            {
                Debug.Log(hit.collider);
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Catch");
                    Debug.Break();
                    // 플레이어를 감지했을 때 레이를 빨간색으로 표시
                    Debug.DrawRay(ray.origin, ray.direction * sightRange, Color.red);
                    return;
                }
            }
        }

        Debug.Log("Player is not in sight or blocked by obstacles.");
    }
}
