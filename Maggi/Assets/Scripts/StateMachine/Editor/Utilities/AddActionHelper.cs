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
            // 멀티 오브젝트 편집 지원
            serializedObject.Update();

            EditorGUILayout.LabelField("This is a custom inspector");

            // 기본 인스펙터 필드 및 기타 UI 추가
            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }

        public void Dispose()
        {
            // 필요시 정리 코드 작성
        }
    }
}
