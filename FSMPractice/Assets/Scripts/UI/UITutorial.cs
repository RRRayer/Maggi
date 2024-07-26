using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private TutorialSO _tutorial;

    [Header("Listening to")]
    [SerializeField] private FloatEventChannelSO _floatTutorial = default;

    private void OnEnable()
    {
        _floatTutorial.OnEventRaised += UpdateFloatingUI;
    }

    private void UpdateFloatingUI(float alpha)
    {
        Color imageColor = _tutorial.Image.color;
        imageColor.a = alpha;
        _tutorial.Image.color = imageColor;

        Color textColor = _tutorial.Text.color;
        textColor.a = alpha;
        _tutorial.Text.color = textColor;
    }
}
