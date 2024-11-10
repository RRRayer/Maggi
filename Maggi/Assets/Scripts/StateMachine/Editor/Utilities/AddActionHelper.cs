using Pudding.StateMachine.ScriptableObjects;
using System;
using UnityEditor;
using UnityEngine;

namespace Pudding.StateMachine.Editor
{
    [CustomEditor(typeof(InteractiveObject), true)]
    public class AddActionHelper : UnityEditor.Editor, IDisposable
    {
        public override void OnInspectorGUI()
        {
            // ��Ƽ ������Ʈ ���� ����
            serializedObject.Update();

            EditorGUILayout.LabelField("This is a custom inspector");

            // �⺻ �ν����� �ʵ� �� ��Ÿ UI �߰�
            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }

        public void Dispose()
        {
            // �ʿ�� ���� �ڵ� �ۼ�
        }
    }
}
