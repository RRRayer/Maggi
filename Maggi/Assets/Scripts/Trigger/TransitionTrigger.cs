using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private float moveDuration = 2f;  // 몇 초에 걸쳐 이동시킬지

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어의 Transform을 코루틴으로 부드럽게 이동
            StartCoroutine(MoveOverTime(other.transform, destination.position, moveDuration));
        }
    }

    private IEnumerator MoveOverTime(Transform target, Vector3 endPos, float duration)
    {
        float time = 0f;
        Vector3 startPos = target.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        // 마지막 위치 보정
        target.position = endPos;
    }

}
