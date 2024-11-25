using UnityEngine;
using UnityEditor;

public class CreateTestModels : EditorWindow
{
    [MenuItem("Tools/Create Test Models")]
    public static void ShowWindow()
    {
        GetWindow<CreateTestModels>("Test Model Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Create Test FBX Files", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Character Test"))
        {
            CreateCharacterTest();
        }

        if (GUILayout.Button("Create Weapon Test"))
        {
            CreateWeaponTest();
        }

        if (GUILayout.Button("Create Environment Test"))
        {
            CreateEnvironmentTest();
        }
    }

    private void CreateCharacterTest()
    {
        // Create a simple humanoid shape
        GameObject character = new GameObject("TestCharacter");
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        
        body.transform.parent = character.transform;
        head.transform.parent = character.transform;
        
        head.transform.localPosition = Vector3.up * 1f;
        
        ExportFBX(character, "Characters/TEST_CHARACTER");
    }

    private void CreateWeaponTest()
    {
        // Create a simple weapon shape
        GameObject weapon = new GameObject("TestWeapon");
        GameObject barrel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        barrel.transform.parent = weapon.transform;
        handle.transform.parent = weapon.transform;
        
        barrel.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
        handle.transform.localScale = new Vector3(0.1f, 0.2f, 0.1f);
        handle.transform.localPosition = Vector3.down * 0.3f;
        
        ExportFBX(weapon, "Weapons/TEST_WEAPON");
    }

    private void CreateEnvironmentTest()
    {
        // Create a simple environment piece
        GameObject environment = new GameObject("TestEnvironment");
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        ground.transform.parent = environment.transform;
        wall.transform.parent = environment.transform;
        
        ground.transform.localScale = new Vector3(5f, 0.1f, 5f);
        wall.transform.localScale = new Vector3(5f, 2f, 0.1f);
        wall.transform.localPosition = Vector3.forward * 2.5f + Vector3.up;
        
        ExportFBX(environment, "Environment/TEST_ENVIRONMENT");
    }

    private void ExportFBX(GameObject obj, string fileName)
    {
        string path = $"Assets/Models/Test/{fileName}.fbx";
        FbxExporter.ExportObject(path, obj);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Export Complete", $"Test model exported to {path}", "OK");
        DestroyImmediate(obj);
    }
}
