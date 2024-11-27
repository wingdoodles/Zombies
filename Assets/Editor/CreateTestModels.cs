using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateTestModels : EditorWindow
{
    private void OnDestroy()
    {
        // Clean up any remaining test objects
        GameObject[] testObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in testObjects)
        {
            if (obj.name.StartsWith("Test"))
            {
                DestroyImmediate(obj);
            }
        }
    }

    [MenuItem("Tools/Create Test Models")]
    public static void ShowWindow()
    {
        GetWindow<CreateTestModels>("Test Model Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Create Test FBX Files", EditorStyles.boldLabel);

        if (GUILayout.Button("Create All Test Objects"))
        {
            CreateAllTestObjects();
        }
    }

    private void CreateAllTestObjects()
    {
        CreateCharacterTest();
        CreateWeaponTest();
        CreateEnvironmentTest();
        
        EditorUtility.DisplayDialog("Objects Created", 
            "Test objects have been created in the scene.\nSelect them and use GameObject > Export To FBX... to export.", "OK");
    }

    private void CreateCharacterTest()
    {
        GameObject character = new GameObject("TestCharacter");
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        
        body.transform.parent = character.transform;
        head.transform.parent = character.transform;
        
        head.transform.localPosition = Vector3.up * 1f;
    }

    private void CreateWeaponTest()
    {
        GameObject weapon = new GameObject("TestWeapon");
        GameObject barrel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        barrel.transform.parent = weapon.transform;
        handle.transform.parent = weapon.transform;
        
        barrel.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
        handle.transform.localScale = new Vector3(0.1f, 0.2f, 0.1f);
        handle.transform.localPosition = Vector3.down * 0.3f;
        
        weapon.transform.position = Vector3.right * 2;
    }

    private void CreateEnvironmentTest()
    {
        GameObject environment = new GameObject("TestEnvironment");
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        ground.transform.parent = environment.transform;
        wall.transform.parent = environment.transform;
        
        ground.transform.localScale = new Vector3(5f, 0.1f, 5f);
        wall.transform.localScale = new Vector3(5f, 2f, 0.1f);
        wall.transform.localPosition = Vector3.forward * 2.5f + Vector3.up;
        
        environment.transform.position = Vector3.left * 2;
    }
}