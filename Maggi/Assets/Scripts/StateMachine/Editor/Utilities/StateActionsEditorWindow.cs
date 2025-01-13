using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using Maggi.StateMachine.ScriptableObjects;
using Maggi.StateMachine.Editor;

[CustomEditor(typeof(StateActionSO))]
public class StateActionsEditorWindow : EditorWindow
{
    private SerializedObject _serializedObject;
    private SerializedProperty _arrayProperty;
    private ReorderableList _reorderableList;

    private StateActionSO _targetAsset;

    public static void OpenWindow(SerializedObject serializedObject, SerializedProperty property, string title)
    {
        var window = GetWindow<StateActionsEditorWindow>(title);
        window.titleContent = new GUIContent(title);
        window._serializedObject = serializedObject;
        window._arrayProperty = property;

        if (serializedObject?.targetObject is StateActionSO so)
        {
            window._targetAsset = so;
        }

        window.Initialize();
        window.Show();
    }

    private void OnEnable()
    {
        // 만약 도중에 SerializedObject가 해제되었거나, 도메인 리로드 등으로 null이 되었다면 다시 생성
        if (_serializedObject == null && _targetAsset != null)
        {
            _serializedObject = new SerializedObject(_targetAsset);

            // 본인 상황에 맞게 필드명을 교체해주세요.
            // 예) "actions", "conditions" 등
            _arrayProperty = _serializedObject.FindProperty("actions");
        }

        // SerializedObject와 Property가 유효하다면 ReorderableList를 다시 세팅
        if (_serializedObject != null && _arrayProperty != null)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        rootVisualElement.Clear();

        // Null check for _serializedObject and _arrayProperty
        if (_serializedObject == null)
        {
            Debug.LogError("Serialized Object is NULL. Initialization aborted.");
            return;
        }

        if (_arrayProperty == null)
        {
            Debug.LogError("Array Property is NULL. Initialization aborted.");
            return;
        }

        // Create ReorderableList
        _reorderableList = new ReorderableList(_serializedObject, _arrayProperty, true, true, true, true);
        SetupActionsList(_reorderableList);

        // Create IMGUIContainer to host ReorderableList
        IMGUIContainer listContainer = new IMGUIContainer(() =>
        {
            if (_serializedObject == null || _serializedObject.targetObject == null)
            {
                Debug.LogWarning("Serialized Object is lost or destroyed during IMGUIContainer callback.");
                return;
            }

            _serializedObject.Update();
            _reorderableList.DoLayoutList();
            _serializedObject.ApplyModifiedProperties();
        });

        rootVisualElement.Add(listContainer);
    }

    private static void SetupActionsList(ReorderableList reorderableList)
    {
        reorderableList.elementHeight *= 1.5f;
        reorderableList.drawHeaderCallback += rect => GUI.Label(rect, "Actions");
        reorderableList.onAddCallback += list =>
        {
            int count = list.count;
            list.serializedProperty.InsertArrayElementAtIndex(count);
            var prop = list.serializedProperty.GetArrayElementAtIndex(count);
            prop.objectReferenceValue = null;
        };

        reorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var r = rect;
            r.height = EditorGUIUtility.singleLineHeight;
            r.y += 5;
            r.x += 5;

            var prop = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            if (prop.objectReferenceValue != null)
            {
                //The icon of the asset SO (basically an object field, cut to show just the icon)
                r.width = 35;
                EditorGUI.PropertyField(r, prop, GUIContent.none);
                r.width = rect.width - 50;
                r.x += 42;

                //The name of the StateAction
                string label = prop.objectReferenceValue.name;
                GUI.Label(r, label, EditorStyles.boldLabel);

                //The description
                r.x += 180;
                r.width = rect.width - 50 - 180;
                string description = (prop.objectReferenceValue as DescriptionSMActionBaseSO).description;
                GUI.Label(r, description);
            }
            else
                EditorGUI.PropertyField(r, prop, GUIContent.none);
        };

        reorderableList.onChangedCallback += list => list.serializedProperty.serializedObject.ApplyModifiedProperties();
        reorderableList.drawElementBackgroundCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (isFocused)
                EditorGUI.DrawRect(rect, ContentStyle.Focused);

            if (index % 2 != 0)
                EditorGUI.DrawRect(rect, ContentStyle.ZebraDark);
            else
                EditorGUI.DrawRect(rect, ContentStyle.ZebraLight);
        };
    }
}