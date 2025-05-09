using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private TutorialSO _tutorial;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _text;

    [Header("Broadcasting on")]
    [SerializeField] private FloatEventChannelSO _floatTutorial = default;

    private Collider _collider = default;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _tutorial.Image.sprite = _sprite;
            _tutorial.Text.text = _text;
        } 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // collider와 player의 거리에 비례해 floating image의 alpha 값을 건든다
            Vector3 center = transform.position;
            center.y = other.transform.position.y;
            float distance = Vector3.Distance(center, other.transform.position);

            float maxDistance = _collider.bounds.size.x / 2;
            float alpha = Mathf.Clamp01(1.0f - distance / maxDistance) * 2.0f;
            _floatTutorial.RaiseEvent(alpha);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _floatTutorial.RaiseEvent(0.0f);
    }
}
