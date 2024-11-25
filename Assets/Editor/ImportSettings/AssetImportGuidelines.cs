using UnityEditor;
using UnityEngine;

public class AssetImportGuidelines : AssetPostprocessor
{
    // Character Model Settings
    private void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        string lowercasePath = assetPath.ToLower();

        if (lowercasePath.Contains("/characters/"))
        {
            SetCharacterImportSettings(modelImporter);
        }
        else if (lowercasePath.Contains("/weapons/"))
        {
            SetWeaponImportSettings(modelImporter);
        }
        else if (lowercasePath.Contains("/environment/"))
        {
            SetEnvironmentImportSettings(modelImporter);
        }
    }

    private void SetCharacterImportSettings(ModelImporter modelImporter)
    {
        modelImporter.meshCompression = ModelImporterMeshCompression.Medium;
        modelImporter.importNormals = ModelImporterNormals.Import;
        modelImporter.importBlendShapes = true;
        modelImporter.importVisibility = true;
        modelImporter.importCameras = false;
        modelImporter.importLights = false;
        modelImporter.preserveHierarchy = true;
        modelImporter.animationType = ModelImporterAnimationType.Human;
        modelImporter.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
    }

    private void SetWeaponImportSettings(ModelImporter modelImporter)
    {
        modelImporter.meshCompression = ModelImporterMeshCompression.Low;
        modelImporter.importNormals = ModelImporterNormals.Import;
        modelImporter.importBlendShapes = false;
        modelImporter.importVisibility = true;
        modelImporter.importCameras = false;
        modelImporter.importLights = false;
        modelImporter.preserveHierarchy = true;
    }

    private void SetEnvironmentImportSettings(ModelImporter modelImporter)
    {
        modelImporter.meshCompression = ModelImporterMeshCompression.Low;
        modelImporter.importNormals = ModelImporterNormals.Calculate;
        modelImporter.generateSecondaryUV = true;
        modelImporter.addCollider = true;
    }

    // Texture Import Settings
    private void OnPreprocessTexture()
    {
        TextureImporter textureImporter = assetImporter as TextureImporter;
        string lowercasePath = assetPath.ToLower();

        if (lowercasePath.Contains("_normal"))
        {
            SetNormalMapSettings(textureImporter);
        }
        else if (lowercasePath.Contains("_metallic"))
        {
            SetMetallicMapSettings(textureImporter);
        }
        else
        {
            SetDefaultTextureSettings(textureImporter);
        }
    }

    private void SetNormalMapSettings(TextureImporter textureImporter)
    {
        textureImporter.textureType = TextureImporterType.NormalMap;
        textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
    }

    private void SetMetallicMapSettings(TextureImporter textureImporter)
    {
        textureImporter.textureType = TextureImporterType.Default;
        textureImporter.sRGBTexture = false;
        textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
    }

    private void SetDefaultTextureSettings(TextureImporter textureImporter)
    {
        textureImporter.textureType = TextureImporterType.Default;
        textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
        textureImporter.mipmapEnabled = true;
    }
}
