using NUnit.Framework;
using UnityEngine;
using UnityEditor;

public class AssetValidationTests
{
    [Test]
    public void ValidateModelImportSettings()
    {
        string[] modelGuids = AssetDatabase.FindAssets("t:Model", new[] { "Assets/Models" });
        
        foreach (string guid in modelGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
            
            if (path.Contains("/Characters/"))
            {
                Assert.AreEqual(ModelImporterMeshCompression.Medium, importer.meshCompression);
                Assert.IsTrue(importer.importBlendShapes);
            }
            else if (path.Contains("/Environment/"))
            {
                Assert.IsTrue(importer.generateSecondaryUV);
            }
        }
    }

    [Test]
    public void ValidateMaterialNaming()
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Materials" });
        
        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Assert.IsTrue(path.Contains("ZOM_MAT_"), $"Material at {path} doesn't follow naming convention");
        }
    }
}
