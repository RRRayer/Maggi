using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton = default;

    public UnityAction StartGameAction;

    public void StartButton()
    {
        StartGameAction.Invoke();
    }
}
