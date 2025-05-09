using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{
    public RectTransform panelContainer; // A와 B를 포함한 부모
    public RectTransform panelA;
    public RectTransform panelB;
    public RectTransform transitionButton;

    public float transitionDuration = 1.5f;

    public RectTransform currentPanel;

    private void Start()
    {
        currentPanel = panelA; // 시작 시 A 패널로 초기화
    }

    public void OnClickTransition()
    {
        if (currentPanel != panelB)
        {
            // 1. 버튼 확대
            transitionButton.DOScale(1.5f, transitionDuration).SetEase(Ease.OutBack);

            // 2. 버튼 이동 (World -> Local 좌표계 주의)
            Vector3 targetPos = new Vector3(
                panelA.rect.width + 50f,
                transitionButton.anchoredPosition.y,
                0f);
            transitionButton.DOAnchorPos(targetPos, transitionDuration).SetEase(Ease.OutQuad);

            // 3. 전체 패널 오른쪽 슬라이드
            float shiftAmount = panelA.rect.width;
            panelContainer.DOAnchorPosX(-shiftAmount, transitionDuration).SetEase(Ease.InOutCubic);

            currentPanel = panelB;
        }
        else if (currentPanel == panelB)
        {
            // 1. 버튼 축소
            transitionButton.DOScale(1.0f, transitionDuration).SetEase(Ease.OutBack);

            // 3. PanelContainer → 원래 위치로 슬라이드 (PanelA가 보이게)
            panelContainer.DOAnchorPosX(0f, transitionDuration).SetEase(Ease.InOutCubic);
            
            // 2. 버튼 이동 (PanelA 왼쪽 위치로)
            Vector3 targetPos = new Vector3(
                50.0f,
                transitionButton.anchoredPosition.y,
                0f);
            transitionButton.DOAnchorPos(targetPos, transitionDuration).SetEase(Ease.OutQuad);
            
            currentPanel = panelA;
        }
    }
}