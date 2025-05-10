using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "TMPFontChanger", menuName = "TMP/TMP Font Changer", order = -1) ]
public class TMPFontChanger : ScriptableObject
{
    [SerializeField] public TMP_FontAsset FontAsset;
}

#if UNITY_EDITOR
[CustomEditor(typeof(TMPFontChanger))]
public class TMPFontChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Change Font!"))
        {
            TMP_FontAsset fontAsset = ((TMPFontChanger)target).FontAsset;

            foreach(TextMeshPro textMeshPro3D in GameObject.FindObjectsByType<TextMeshPro>(FindObjectsSortMode.InstanceID)) 
            { 
                textMeshPro3D.font = fontAsset;
                textMeshPro3D.margin = new Vector4(10, 10, 10, 10);
            }
            foreach(TextMeshProUGUI textMeshProUi in GameObject.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.InstanceID)) 
            { 
                textMeshProUi.font = fontAsset;
                textMeshProUi.margin = new Vector4(10, 10, 10, 10);
            }
        }
    }
}
#endif