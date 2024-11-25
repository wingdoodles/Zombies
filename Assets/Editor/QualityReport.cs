using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class QualityReport : EditorWindow
{
    [MenuItem("Tools/Generate Quality Report")]
    public static void ShowWindow()
    {
        GetWindow<QualityReport>("Quality Report");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Generate Report"))
        {
            GenerateReport();
        }
    }

    void GenerateReport()
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine("Asset Quality Report");
        report.AppendLine("==================");

        // Check Models
        report.AppendLine("\nModel Analysis:");
        string[] modelGuids = AssetDatabase.FindAssets("t:Model");
        foreach (string guid in modelGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
            report.AppendLine($"\n{path}");
            report.AppendLine($"- Compression: {importer.meshCompression}");
            report.AppendLine($"- Generate Lightmap UVs: {importer.generateSecondaryUV}");
        }

        // Check Materials
        report.AppendLine("\nMaterial Analysis:");
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            report.AppendLine($"\n{path}");
            report.AppendLine($"- Shader: {material.shader.name}");
        }

        // Save Report
        string reportPath = "Assets/QualityReport.txt";
        File.WriteAllText(reportPath, report.ToString());
        AssetDatabase.Refresh();
        Debug.Log($"Report generated at: {reportPath}");
    }
}
