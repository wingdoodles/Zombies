using UnityEditor;
using UnityEngine;

public class ModelImportSettings : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        
        if (assetPath.Contains("Characters"))
        {
            SetupCharacterImport(modelImporter);
        }
        else if (assetPath.Contains("Weapons"))
        {
            SetupWeaponImport(modelImporter);
        }
        else if (assetPath.Contains("Environment"))
        {
            SetupEnvironmentImport(modelImporter);
        }
    }

    private void SetupCharacterImport(ModelImporter modelImporter)
    {
        modelImporter.globalScale = 1.0f;
        modelImporter.meshCompression = ModelImporterMeshCompression.Medium;
        modelImporter.animationType = ModelImporterAnimationType.Human;
        modelImporter.generateSecondaryUV = true;
    }

    private void SetupWeaponImport(ModelImporter modelImporter)
    {
        modelImporter.globalScale = 1.0f;
        modelImporter.meshCompression = ModelImporterMeshCompression.Low;
        modelImporter.generateSecondaryUV = true;
    }

    private void SetupEnvironmentImport(ModelImporter modelImporter)
    {
        modelImporter.generateSecondaryUV = true;
        modelImporter.addCollider = true;
        SetupLODGroups(modelImporter);
    }

    private void SetupLODGroups(ModelImporter modelImporter)
    {
        modelImporter.generateSecondaryUV = true;
        modelImporter.optimizeMeshPolygons = true;
        modelImporter.optimizeMeshVertices = true;
        modelImporter.importLights = false;
        modelImporter.importCameras = false;
    }
}
