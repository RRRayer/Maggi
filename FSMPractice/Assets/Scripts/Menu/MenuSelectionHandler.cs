using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelectionHandler : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField][ReadOnly] private GameObject _defaultSelection;
    [SerializeField][ReadOnly] private GameObject _currentSelection;
    [SerializeField][ReadOnly] private GameObject _mouseSelection;

    private void OnEnable()
    {
        _inputReader.MenuMouseMoveEvent += HandleMoveCursor;
        _inputReader.MoveSelectionEvent += HandleMoveSelection;
    }

    private void OnDisable()
    {
        _inputReader.MenuMouseMoveEvent -= HandleMoveCursor;
        _inputReader.MoveSelectionEvent -= HandleMoveSelection;
    }

    private IEnumerator SelectDefault()
    {
        yield return new WaitForSeconds(.03f);

        if (_defaultSelection != null)
            UpdateSelection(_defaultSelection);
    }

    private void UpdateSelection(GameObject UIElement)
    {
        if (UIElement.GetComponent<MultiInputButton>() != null)
        {
            _mouseSelection = UIElement;
            _currentSelection = UIElement;
        }
    }

    private void HandleMoveCursor()
    {
        if (_mouseSelection != null)
        {
            EventSystem.current.SetSelectedGameObject(_mouseSelection);
        }

        Cursor.visible = true;
    }

    private void HandleMoveSelection()
    {
        Cursor.visible = false;

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(_currentSelection);
    }

    public void HandleMouseEnter(GameObject UIElement)
    {
        _mouseSelection = UIElement;
        EventSystem.current.SetSelectedGameObject(UIElement);
    }

    public void HandleMouseExit(GameObject UIElement)
    {
        if (EventSystem.current.currentSelectedGameObject != UIElement)
            return;

        // keep selecting the last thing the mouse has selected 
        _mouseSelection = null;
        EventSystem.current.SetSelectedGameObject(_currentSelection);
    }
}
