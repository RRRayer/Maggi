using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private TutorialSO _tutorial;

    [Header("Broadcasting on")]
    [SerializeField] private FloatEventChannelSO _floatTutorial = default;

    private Collider _collider = default;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _tutorial.Image = transform.GetChild(0).GetComponent<Image>();
        _tutorial.Text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // collider와 player의 거리에 비례해 floating image의 alpha 값을 건든다
            float distance = Vector3.Distance(transform.position, other.transform.position);

            float maxDistance = _collider.bounds.size.x / 2;
            float alpha = Mathf.Clamp01(0.5f - distance / maxDistance);
            _floatTutorial.RaiseEvent(alpha);   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _floatTutorial.RaiseEvent(0.0f);
    }
}
