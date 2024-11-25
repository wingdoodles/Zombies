using UnityEngine;
using UnityEditor;

public class MaterialQualityChecker : EditorWindow
{
    private Material selectedMaterial;
    private Vector2 scrollPosition;

    [MenuItem("Tools/Material Quality Checker")]
    public static void ShowWindow()
    {
        GetWindow<MaterialQualityChecker>("Material QC");
    }

    void OnGUI()
    {
        GUILayout.Label("Material Quality Checker", EditorStyles.boldLabel);
        
        selectedMaterial = (Material)EditorGUILayout.ObjectField("Material", selectedMaterial, typeof(Material), false);

        if (selectedMaterial == null) return;

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Display shader name
        EditorGUILayout.LabelField("Shader:", selectedMaterial.shader.name);

        EditorGUILayout.Space(10);
        GUILayout.Label("Naming Convention", EditorStyles.boldLabel);
        bool correctNaming = selectedMaterial.name.StartsWith("ZOM_MAT_");
        DrawCheckbox("Correct Prefix (ZOM_MAT_)", correctNaming);

        EditorGUILayout.Space(10);
        GUILayout.Label("Texture Maps", EditorStyles.boldLabel);
        
        // List all texture properties
        SerializedObject serializedMaterial = new SerializedObject(selectedMaterial);
        SerializedProperty texEnvs = serializedMaterial.FindProperty("m_SavedProperties.m_TexEnvs");
        
        if (texEnvs != null)
        {
            for (int i = 0; i < texEnvs.arraySize; i++)
            {
                SerializedProperty texEnv = texEnvs.GetArrayElementAtIndex(i);
                string propertyName = texEnv.FindPropertyRelative("first").stringValue;
                EditorGUILayout.LabelField($"Found texture property: {propertyName}");
            }
        }

        DrawCheckbox("Base Color Map", HasTexture(selectedMaterial, "_BaseMap"));
        DrawCheckbox("Normal Map", HasTexture(selectedMaterial, "_BumpMap"));
        DrawCheckbox("Metallic-Smoothness Map", HasTexture(selectedMaterial, "_MetallicGlossMap"));

        EditorGUILayout.Space(10);
        GUILayout.Label("Material Properties", EditorStyles.boldLabel);
        DrawPropertyRange("Metallic", selectedMaterial.GetFloat("_Metallic"), 0f, 1f);
        DrawPropertyRange("Smoothness", selectedMaterial.GetFloat("_Smoothness"), 0f, 1f);

        EditorGUILayout.EndScrollView();
    }
    private bool HasTexture(Material material, string propertyName)
    {
        if (!material.HasProperty(propertyName))
            return false;
        
        Texture tex = material.GetTexture(propertyName);
        bool hasTexture = tex != null;
        
        // Debug output
        EditorGUILayout.LabelField($"{propertyName}: {(hasTexture ? tex.name : "Not assigned")}");
        
        return hasTexture;
    }
    

    private void DrawCheckbox(string label, bool value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label);
        GUI.enabled = false;
        EditorGUILayout.Toggle(value);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
    }

    private void DrawPropertyRange(string label, float value, float min, float max)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label);
        bool inRange = value >= min && value <= max;
        GUI.color = inRange ? Color.green : Color.red;
        EditorGUILayout.LabelField(value.ToString("F2"));
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
    }
}