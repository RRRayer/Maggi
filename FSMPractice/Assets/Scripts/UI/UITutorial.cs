using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private TutorialSO _tutorial;
    [SerializeField] private float _duration;

    [Header("Listening to")]
    [SerializeField] private FloatEventChannelSO _floatTutorial = default;

    private void OnEnable()
    {
        _tutorial.Image = transform.GetChild(0).GetComponent<Image>();
        _tutorial.Text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();        

        _floatTutorial.OnEventRaised += UpdateFloatingUI;
    }

    private void UpdateFloatingUI(float alpha)
    {
        // 이미지 알파값 조정
        _tutorial.Image.DOFade(alpha, _duration);

        // 텍스트 알파값 조정
        _tutorial.Text.DOFade(alpha, _duration);
    }
}
