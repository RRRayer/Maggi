using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    [SerializeField] private Button _restartButton = default;
    [SerializeField] private Button _newGameButton = default;
    [SerializeField] private Button _goMainMenuButton = default;

    public UnityAction RestartButtonAction;
    public UnityAction NewGameButtonAction;
    public UnityAction GoMainMenuButtonAction;

    public void RestartButton()
    {
        RestartButtonAction.Invoke();
    }

    public void NewGameButton()
    {
        NewGameButtonAction.Invoke();
    }

    public void GoMainMenuButton()
    {
        GoMainMenuButtonAction.Invoke();
    }
}
