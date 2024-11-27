using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System;

internal class ScriptTemplates
{
	private static readonly string _path = "Assets/Scripts/StateMachine/Editor/Templates";

	[MenuItem("Assets/Create/State Machines/Action Script", false, 0)]
	public static void CreateActionScript() =>
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
			ScriptableObject.CreateInstance<DoCreateStateMachineScriptAsset>(),
			"NewActionSO.cs",
			(Texture2D)EditorGUIUtility.IconContent("cs Script Icon").image,
			$"{_path}/StateAction.txt");

	[MenuItem("Assets/Create/State Machines/Condition Script", false, 0)]
	public static void CreateConditionScript() =>
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
			ScriptableObject.CreateInstance<DoCreateStateMachineScriptAsset>(),
			"NewConditionSO.cs",
			(Texture2D)EditorGUIUtility.IconContent("cs Script Icon").image,
			$"{_path}/StateCondition.txt");

    [MenuItem("Assets/Create/State Machines/Interactive Object Script", false, 0)]
    public static void CreateInteractiveObjectScript() =>
    ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
        ScriptableObject.CreateInstance<DoCreateStateMachineScriptAsset>(),
        "NewObject.cs",
        (Texture2D)EditorGUIUtility.IconContent("cs Script Icon").image,
        $"{_path}/InteractiveObject.txt");

    private class DoCreateStateMachineScriptAsset : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            string text = File.ReadAllText(resourceFile);

            string fileName = Path.GetFileNameWithoutExtension(pathName);
            {
                string newName = fileName.Replace(" ", "");

                // Ensure that names ending with "Object" do not get "SO" appended
                if (!newName.EndsWith("Object", StringComparison.OrdinalIgnoreCase) && !newName.Contains("SO"))
                    newName = newName.Insert(fileName.Length, "SO");

                pathName = pathName.Replace(fileName, newName);
                fileName = newName;
            }

            string fileNameWithoutExtension = fileName.Substring(0, fileName.Length);
            text = text.Replace("#SCRIPTNAME#", fileNameWithoutExtension);

            string runtimeName = fileNameWithoutExtension.Replace("SO", "");
            text = text.Replace("#RUNTIMENAME#", runtimeName);

            for (int i = runtimeName.Length - 1; i > 0; i--)
                if (char.IsUpper(runtimeName[i]) && char.IsLower(runtimeName[i - 1]))
                    runtimeName = runtimeName.Insert(i, " ");

            text = text.Replace("#RUNTIMENAME_WITH_SPACES#", runtimeName);

            string fullPath = Path.GetFullPath(pathName);
            var encoding = new UTF8Encoding(true);
            File.WriteAllText(fullPath, text, encoding);
            AssetDatabase.ImportAsset(pathName);
            ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object)));
        }
    }

}
