using UnityEditor;
using System.IO;

public class AssetFolderCreator : EditorWindow
{
    [MenuItem("Tools/Create Asset Folders")]
    public static void CreateAssetFolders()
    {
        string[] folders = new string[]
        {
            "Assets/Models/Characters",
            "Assets/Models/Weapons",
            "Assets/Models/Environment",
            "Assets/Materials/Characters",
            "Assets/Materials/Weapons",
            "Assets/Materials/Environment",
            "Assets/Textures/Characters",
            "Assets/Textures/Weapons",
            "Assets/Textures/Environment",
            "Assets/Prefabs/Characters",
            "Assets/Prefabs/Weapons",
            "Assets/Prefabs/Environment"
        };

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
        
        AssetDatabase.Refresh();
    }
}
