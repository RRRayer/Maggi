using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Restart Settings")]
    public float restartDelay = 2f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RestartGame()
    {
        Debug.Log("게임 리스타트 예약됨");
        Invoke(nameof(ReloadScene), restartDelay);
    }

    private void ReloadScene()
    {
        Debug.Log("씬 다시 로드");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}