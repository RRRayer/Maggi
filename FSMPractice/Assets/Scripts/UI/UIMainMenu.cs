using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button _newGameButton = default;
    [SerializeField] private Button _continueButton = default;

    public UnityAction NewGameButtonAction;
    public UnityAction ContinueButtonAction;

    public void NewGameButton()
    {
        NewGameButtonAction.Invoke();
    }

    public void ContinueButton()
    {
        ContinueButtonAction.Invoke();
    }
}
