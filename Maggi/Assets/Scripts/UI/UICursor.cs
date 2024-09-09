using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICursor : MonoBehaviour
{
    [SerializeField] private Texture2D _customCursor;

    private void Start()
    {
        Cursor.SetCursor(_customCursor, Vector2.up * 0.5f, CursorMode.Auto);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
