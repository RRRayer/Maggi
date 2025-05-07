using UnityEngine;
using UnityEngine.Events;

public class GameOverUI : MonoBehaviour
{
    public VoidEventChannelSO playerDiedEvent;
    public GameObject gameOverText;

    private void OnEnable() => playerDiedEvent.OnEventRaised += ShowUI;
    private void OnDisable() => playerDiedEvent.OnEventRaised -= ShowUI;

    private void ShowUI()
    {
        gameOverText.SetActive(true);
    }
}
