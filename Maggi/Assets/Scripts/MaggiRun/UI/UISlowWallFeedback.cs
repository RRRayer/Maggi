using UnityEngine;
using UnityEngine.Events;

public class UISlowWallFeedback : MonoBehaviour
{
    public VoidEventChannelSO slowWallClearedEvent;
    public TMPro.TextMeshProUGUI statusText;

    private void OnEnable()
    {
        slowWallClearedEvent.OnEventRaised += ShowMessage;
    }

    private void OnDisable()
    {
        slowWallClearedEvent.OnEventRaised -= ShowMessage;
    }

    private void ShowMessage()
    {
        statusText.text = "슬로우월 해제!";
        Invoke(nameof(ClearText), 2f);
    }

    private void ClearText()
    {
        statusText.text = "";
    }
}
