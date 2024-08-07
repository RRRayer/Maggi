using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISettingController : MonoBehaviour
{
    public UnityAction Closed;

    public void ClosedScreen()
    {
        Closed.Invoke();
    }
}