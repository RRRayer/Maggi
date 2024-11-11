using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Pudding.StateMachine.ScriptableObjects;
using UnityEditor.UIElements;

[CustomEditor(typeof(InteractiveObject), true)]
public class InteractiveObjectEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        root.Add(CreateNewProperty("Idle Action", "m_IdleScriptableActions", "Edit Idle Actions")); ;
        root.Add(CreateNewProperty("Walk Action", "m_WalkScriptableActions", "Edit Walking Actions"));
        root.Add(CreateNewProperty("JumpAscending Action", "m_JumpAscendingScriptableActions", "Edit JumpAscending Actions"));
        root.Add(CreateNewProperty("JumpDescending Action", "m_JumpDescendingScriptableActions", "Edit JumpDescending Actions"));
        root.Add(CreateNewProperty("Pull Action", "m_PullScriptableActions", "Edit Pull Actions"));
        root.Add(CreateNewProperty("Push Action", "m_PushScriptableActions", "Edit Push Actions"));

        return root;
    }

    private VisualElement CreateNewProperty(string labelName, string propertyName, string windowName)
    {
        // Create Edit Idle Action Button
        var buttonContainer = new VisualElement { style = { flexDirection = FlexDirection.Row } };

        // Add Container Label
        var label = new Label(labelName) 
        { 
            style = 
            { 
                marginRight = 5,
                alignSelf = Align.Center,
            } 
        };
        buttonContainer.Add(label);

        // Add Edit Button
        var button = new Button(() =>
        {
            SerializedProperty idleProperty = serializedObject.FindProperty(propertyName);
            StateActionsEditorWindow.OpenWindow(serializedObject, idleProperty, windowName);
        })
        {
            text = "", // 버튼에 텍스트는 비워둠
            style = 
            { 
                width = 24, 
                height = 24, 
                paddingLeft = 2, 
                paddingRight = 2,
                alignSelf = Align.FlexEnd,
            }
        };

        // Set Icon Image
        var iconImage = new Image
        {
            image = EditorGUIUtility.IconContent("d_UnityEditor.AnimationWindow").image,
            scaleMode = ScaleMode.ScaleToFit,
            style = { width = 16, height = 16 }
        };
        button.Add(iconImage);
        buttonContainer.Add(button);

        return buttonContainer;
    }
}
