using System.Collections;
using UnityEngine;

public class TimedDeactivator : MonoBehaviour
{
    [SerializeField] private float _timeToDeactivate = 5.0f;

    private Color _originalColor;

    private void OnEnable()
    {
        

        StartCoroutine(FadeOutAndDeactivate());
    }

    private IEnumerator FadeOutAndDeactivate()
    {
        // 서서히 사라지기 시작
        float fadeSpeed = 1.0f / _timeToDeactivate;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            progress += Time.deltaTime * fadeSpeed;
            Color color = _originalColor;
            color.a = Mathf.Lerp(1.0f, 0.0f, progress); // 투명도 조절
            //foreach (var item in _renderers)
            //{
            //    item.material.color = color;
            //}

            yield return null;
        }

        // 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
