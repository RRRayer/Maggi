using UnityEngine;

public class UICursor : MonoBehaviour
{
    [SerializeField] private RectTransform cursorImage;

    private void Start()
    {
        // 기본 시스템 커서 숨기기
        Cursor.visible = false;
    }

    private void Update()
    {
        // 마우스 위치에 따라 UI 커서를 이동
        Vector2 cursorPos = Input.mousePosition;
        cursorImage.position = cursorPos;
    }
}
