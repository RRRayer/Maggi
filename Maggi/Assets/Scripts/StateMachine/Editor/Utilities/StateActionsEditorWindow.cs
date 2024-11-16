using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Maggi.StateMachine.ScriptableObjects;
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
            var container = new VisualElement { style = { flexDirection = FlexDirection.Row, marginTop = 5 } };

            // Index Capture
            int currentIndex = i;

            // Create ObjectField that can be set to only StateActionSO Script
            var objectField = new ObjectField
            {
                objectType = typeof(StateActionSO),
                value = itemProp.objectReferenceValue,
                allowSceneObjects = false
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


            // Create Drag Handler
            var dragHandle = new VisualElement();
            dragHandle.AddToClassList("drag-handle");
            dragHandle.style.width = 10;
            dragHandle.style.marginRight = 5;
            container.Add(dragHandle);

            var dragManipulator = new DragManipulator(container, currentIndex, (newIndex) =>
            {
                MoveItem(currentIndex, newIndex);
                UpdateList();
            });

            dragHandle.AddManipulator(dragManipulator);

            container.Add(dragHandle);

            
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

    private void MoveItem(int oldIndex, int newIndex)
    {
        if (newIndex < 0 || newIndex >= _arrayProperty.arraySize || oldIndex == newIndex)
            return;

        _arrayProperty.MoveArrayElement(oldIndex, newIndex);
        _serializedObject.ApplyModifiedProperties();
    }
}

public class DragManipulator : MouseManipulator
{
    private VisualElement _element;
    private int _startIndex;
    private System.Action<int> _onDragEnd;

    public DragManipulator(VisualElement element, int startIndex, System.Action<int> onDragEnd)
    {
        _element = element;
        _startIndex = startIndex;
        _onDragEnd = onDragEnd;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        if (CanStartManipulation(evt))
        {
            _element.CaptureMouse();
            evt.StopPropagation();
        }
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        if (_element.HasMouseCapture())
        {
            // Logic to determine the new index based on mouse position goes here
        }
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
        if (CanStopManipulation(evt))
        {
            _element.ReleaseMouse();
            _onDragEnd?.Invoke(_startIndex); // Call the callback with the new index
            evt.StopPropagation();
        }
    }
}