using UnityEditor;

public class AutoModelImporter : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        
        if (assetPath.Contains("/Characters/"))
        {
            modelImporter.meshCompression = ModelImporterMeshCompression.Medium;
            modelImporter.importBlendShapes = true;
            modelImporter.generateSecondaryUV = true;
        }
        else if (assetPath.Contains("/Weapons/"))
        {
            modelImporter.meshCompression = ModelImporterMeshCompression.Low;
            modelImporter.importBlendShapes = false;
            modelImporter.generateSecondaryUV = true;
        }
        else if (assetPath.Contains("/Environment/"))
        {
            modelImporter.meshCompression = ModelImporterMeshCompression.Low;
            modelImporter.addCollider = true;
            modelImporter.generateSecondaryUV = true;
        }
    }
}
