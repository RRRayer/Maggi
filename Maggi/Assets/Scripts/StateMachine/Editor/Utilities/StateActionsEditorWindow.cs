using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Pudding.StateMachine.ScriptableObjects;
using UnityEditor.UIElements;

public class StateActionsEditorWindow : EditorWindow
{
    private SerializedObject _serializedObject;
    private SerializedProperty _arrayProperty;
    private VisualElement _rootElement;
    private VisualElement _listContainer;

    public static void OpenWindow(SerializedObject serializedObject, SerializedProperty property, string title)
    {
        var window = GetWindow<StateActionsEditorWindow>(title); // Get Window
        window.titleContent = new GUIContent(title); // Update Window Title
        window._serializedObject = serializedObject;
        window._arrayProperty = property;
        window.Initialize(); // Init Window Elements
        window.Show();
    }

    private void Initialize()
    {
        _rootElement = new VisualElement();
        rootVisualElement.Clear(); // Clear Existed Elements 
        rootVisualElement.Add(_rootElement);

        // Create Label
        var label = new Label("Actions") { style = { unityFontStyleAndWeight = FontStyle.Bold } };
        _rootElement.Add(label);

        // Init State Action List Container
        _listContainer = new VisualElement();
        UpdateList();
        _rootElement.Add(_listContainer);

        // Create State Action SO Add Button
        var addButton = new Button(() =>
        {
            AddNewItem();
            UpdateList();
        })
        {
            text = "Add Action"
        };
        _rootElement.Add(addButton);
    }

    private void UpdateList()
    {
        _listContainer.Clear();

        for (int i = 0; i < _arrayProperty.arraySize; i++)
        {
            var itemProp = _arrayProperty.GetArrayElementAtIndex(i);
            var container = new VisualElement { style = { flexDirection = FlexDirection.Row } };

            // Index Capture
            int currentIndex = i;

            // Create ObjectField that can be set to only StateActionSO Script
            var objectField = new ObjectField
            {
                objectType = typeof(StateActionSO),
                value = itemProp.objectReferenceValue,
                allowSceneObjects = false,
                label = $"Action {currentIndex + 1}"
            };

            objectField.style.width = 300; // Constrain Width of ObjectField

            objectField.RegisterValueChangedCallback(evt =>
            {
                itemProp.objectReferenceValue = evt.newValue as StateActionSO;
                _serializedObject.ApplyModifiedProperties();
            });

            // Create State Action SO Remove Button
            var removeButton = new Button(() =>
            {
                RemoveItemAtIndex(currentIndex);
                UpdateList();
            })
            {
                text = "-"
            };

            container.Add(objectField);
            container.Add(removeButton);
            _listContainer.Add(container);
        }
    }

    private void AddNewItem()
    {
        _arrayProperty.InsertArrayElementAtIndex(_arrayProperty.arraySize);
        _arrayProperty.GetArrayElementAtIndex(_arrayProperty.arraySize - 1).objectReferenceValue = null;
        _serializedObject.ApplyModifiedProperties();
    }

    private void RemoveItemAtIndex(int index)
    {
        if (index >= 0 && index < _arrayProperty.arraySize)
        {
            _arrayProperty.DeleteArrayElementAtIndex(index);
            _serializedObject.ApplyModifiedProperties();
        }

        if (index >= 0 && index < _arrayProperty.arraySize && _arrayProperty.GetArrayElementAtIndex(index).objectReferenceValue == null)
        {
            _arrayProperty.DeleteArrayElementAtIndex(index);
            _serializedObject.ApplyModifiedProperties();
        }
    }
}
