using UnityEditor;
using UnityEngine;

public class ModelChecker : EditorWindow
{
    [MenuItem("Tools/Model Checker")]
    public static void ShowWindow()
    {
        GetWindow<ModelChecker>("Model Checker");
    }

    void OnGUI()
    {
        GUILayout.Label("Model Quality Control", EditorStyles.boldLabel);

        if (GUILayout.Button("Check Selected Model"))
        {
            CheckSelectedModel();
        }
    }

    private void CheckSelectedModel()
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null)
        {
            EditorUtility.DisplayDialog("Error", "Please select a model to check", "OK");
            return;
        }

        MeshFilter[] meshFilters = selectedObject.GetComponentsInChildren<MeshFilter>();
        bool hasIssues = false;

        foreach (MeshFilter mf in meshFilters)
        {
            Mesh mesh = mf.sharedMesh;
            if (mesh != null)
            {
                // Check vertex count
                if (mesh.vertexCount > 10000)
                {
                    Debug.LogWarning($"High vertex count in {mf.name}: {mesh.vertexCount} vertices");
                    hasIssues = true;
                }

                // Check UV maps
                if (!mesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.TexCoord0))
                {
                    Debug.LogError($"Missing UV map in {mf.name}");
                    hasIssues = true;
                }
            }
        }

        if (!hasIssues)
        {
            EditorUtility.DisplayDialog("Check Complete", "No issues found!", "OK");
        }
    }
}
