using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeySO", menuName = "Key/Key")]
public class KeySO : ScriptableObject
{
    public string ID;

    private void Awake()
    {
        ID = Guid.NewGuid().ToString();
    }
}
