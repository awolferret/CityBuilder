using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class Tools
    {
        private static string saveName = "SaveData_0";
        
        [MenuItem("Tools/Clear Save")]
        public static void ClearSave()
        {
            string fullPath = Path.Combine(Application.persistentDataPath, saveName);
            File.Delete(fullPath);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
