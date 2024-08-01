using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 조작법 바꾸기

public class UIControl : MonoBehaviour
{
    public UnityAction Closed;

    public void ClosedScreen()
    {
        Closed.Invoke();
    }
}
