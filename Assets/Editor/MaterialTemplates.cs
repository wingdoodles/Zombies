using UnityEngine;
using UnityEditor;
using System.IO;

public class MaterialTemplates : EditorWindow
{
    [MenuItem("Tools/Create Material Templates")]
    public static void ShowWindow()
    {
        GetWindow<MaterialTemplates>("Material Templates");
    }

    void OnGUI()
    {
        GUILayout.Label("Create PBR Material Templates", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Character Material"))
        {
            CreateMaterial("Character", Color.white);
        }

        if (GUILayout.Button("Create Weapon Material"))
        {
            CreateMaterial("Weapon", Color.gray);
        }

        if (GUILayout.Button("Create Environment Material"))
        {
            CreateMaterial("Environment", Color.gray);
        }
    }
    private void CreateMaterial(string type, Color baseColor)
    {
        Shader urpShader = Shader.Find("Universal Render Pipeline/Lit");
        if (urpShader == null)
        {
            EditorUtility.DisplayDialog("Error", "URP Lit shader not found! Make sure URP is installed.", "OK");
            return;
        }

        Material material = new Material(urpShader);
        
        // Set default properties
        material.SetColor("_BaseColor", baseColor);
        material.SetFloat("_Metallic", 0f);
        material.SetFloat("_Smoothness", 0.5f);
        
        string path = $"Assets/Materials/Test/ZOM_MAT_{type}_Template.mat";
        
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        AssetDatabase.CreateAsset(material, path);
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = material;
    }
    
}