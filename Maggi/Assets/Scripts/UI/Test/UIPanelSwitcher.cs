using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelSwitcher : MonoBehaviour
{
    // 패널들 부모 설정
    [SerializeField] private RectTransform home;
    // 패널 간 전환되는 속도 설정
    [SerializeField] private float transitionDuration = 1.5f;
    // Switch 시 따라가는 UI
    [SerializeField] private RectTransform followedObject;
    public Vector2 followedObjectOriginalSize;

    private RectTransform panelContainer; // Pause 화면
    private RectTransform currentPanel;

    private void Awake()
    {
        panelContainer = GetComponent<RectTransform>();
        followedObjectOriginalSize = followedObject.sizeDelta;
    }

    /// <summary>
    /// 현재 위치에서 target 위치로 이동
    /// </summary>
    public void SwitchToTarget(RectTransform target)
    {
        if (target == null || panelContainer == null || followedObject == null) 
            return;

        // 1. 타겟 패널의 전역 위치(world position) 값 계산
        Vector2 targetPosition = panelContainer.anchoredPosition - (Vector2)target.anchoredPosition;

        // 2. 패널 이동 애니메이션 실행
        panelContainer.DOAnchorPos(targetPosition, transitionDuration)
            .SetEase(Ease.InOutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
            });

        // 3. `followedObject` width와 height를 타겟 높이로 서서히 변경
        Vector2 targetSize = new Vector2(target.rect.height, target.rect.height);
        
        followedObject.DOSizeDelta(targetSize, transitionDuration)
            .SetEase(Ease.InOutCubic).SetUpdate(true);

        // 4. `followedObject`의 위치를 타겟의 왼쪽 가장자리 중앙으로 서서히 변경
        Vector3 targetWorldPosition = target.position; // target의 월드 좌표
        Vector3 followedObjectWorldPosition = targetWorldPosition + new Vector3(-target.rect.width * 0.5f, 0f, 0f); // 왼쪽 가장자리

        // World Position을 Local Position으로 변환
        Vector3 followedObjectLocalPosition = panelContainer.InverseTransformPoint(followedObjectWorldPosition);

        // 위치 이동 애니메이션
        followedObject.DOAnchorPos(
            new Vector2(followedObjectLocalPosition.x, followedObjectLocalPosition.y), 
            transitionDuration
        ).SetEase(Ease.InOutCubic).SetUpdate(true);

        // 5. 현재 패널 갱신
        currentPanel = target;
    }
    
    public void SwitchToHome()
    {
        Debug.Log("SwitchToHome");
        
        if (home == null || panelContainer == null) 
            return;

        // 패널 이동 애니메이션 실행
        panelContainer.DOAnchorPos(home.anchoredPosition, transitionDuration)
            .SetEase(Ease.InOutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
            });
        
        // followedObject 사이즈 원상복귀
        followedObject.DOSizeDelta(followedObjectOriginalSize, transitionDuration)
            .SetEase(Ease.InOutCubic).SetUpdate(true);

        // `followedObject` 위치 이동 애니메이션
        followedObject.DOAnchorPos(home.anchoredPosition, transitionDuration)
            .SetEase(Ease.InOutCubic).SetUpdate(true);

        // 현재 패널 갱신
        currentPanel = home;
    }
}