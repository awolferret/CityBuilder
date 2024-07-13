using System;
using System.IO;
using UnityEngine;

namespace SaveLoadSystem
{
    public class SaveSystem : MonoBehaviour
    {
        [SerializeField] private string saveName = "SaveData_";
        [SerializeField] private int saveDataIndex = 0;

        public void SaveData(string dataToSave)
        {
            if (WriteToFile(saveName + saveDataIndex, dataToSave))
                Debug.Log("Success Save");
        }

        public string LoadData()
        {
            string data = "";

            if (ReadFromFile(saveName + saveDataIndex, out data))
                Debug.Log("Success Load");

            return data;
        }

        private bool WriteToFile(string name, string content)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, name);

            try
            {
                File.WriteAllText(fullPath, content);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Save Error " + e.Message);
            }

            return false;
        }

        private bool ReadFromFile(string name, out string content)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, name);

            try
            {
                content = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Load Error " + e.Message);
                content = "";
            }

            return false;
        }
    }
}