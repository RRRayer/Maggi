using UnityEngine;
using UnityEngine.Events;

public class SlowWallEventListener : MonoBehaviour
{
    public VoidEventChannelSO slowWallClearedEvent;

    private void OnEnable()
    {
        slowWallClearedEvent.OnEventRaised += DoSomething;
    }

    private void OnDisable()
    {
        slowWallClearedEvent.OnEventRaised -= DoSomething;
    }

    private void DoSomething()
    {
        Debug.Log("슬로우월 해제됨! 사운드 or UI 실행 가능");
    }
}
