using UnityEngine;
using UnityEngine.Events;

public class SceneReloader : MonoBehaviour
{
    public VoidEventChannelSO restartEvent;

    public VoidEventChannelSO onPlayerDied;

    private void OnEnable()
    {
        if (onPlayerDied != null)
            onPlayerDied.OnEventRaised += Restart;
    }

    private void OnDisable()
    {
        if (onPlayerDied != null)
            onPlayerDied.OnEventRaised -= Restart;
    }


    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}

