using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    private UnityAction _interactEvent = default;

    private void Interact()
    {
        _interactEvent.Invoke();
    }
}
