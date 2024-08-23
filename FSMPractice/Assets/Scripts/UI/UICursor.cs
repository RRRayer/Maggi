using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    [SerializeField] private RectTransform cursorImage;
    [SerializeField] private CanvasGroup cursorCanvasGroup;

    private void Start()
    {
        // 기본 시스템 커서 숨기기
        Cursor.visible = false;

        // UI 커서가 클릭 이벤트를 방해하지 않도록 설정
        if (cursorCanvasGroup != null)
        {
            cursorCanvasGroup.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        // 마우스 위치에 따라 UI 커서를 이동
        Vector2 cursorPos = Input.mousePosition;
        cursorImage.position = cursorPos;
    }
}
