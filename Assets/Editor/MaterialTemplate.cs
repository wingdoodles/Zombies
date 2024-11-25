using UnityEngine;
using UnityEditor;

public class MaterialTemplate : ScriptableObject
{
    [MenuItem("Assets/Create/Material Templates/ZOM Standard Material")]
    public static void CreateMaterial()
    {
        Material material = new Material(Shader.Find("HDRP/Lit"));
        
        // Set default properties
        material.SetFloat("_Metallic", 0f);
        material.SetFloat("_Smoothness", 0.5f);
        material.EnableKeyword("_NORMALMAP");
        
        // Save the material
        string path = EditorUtility.SaveFilePanel(
            "Save Material",
            "Assets",
            "ZOM_MAT_New",
            "mat"
        );
        
        if (path.Length != 0)
        {
            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            AssetDatabase.CreateAsset(material, relativePath);
            AssetDatabase.SaveAssets();
        }
    }
}
