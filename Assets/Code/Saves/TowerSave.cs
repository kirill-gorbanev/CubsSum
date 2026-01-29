using System;
using System.Linq;
using UnityEngine;
using System.IO;
using Code.Build;

namespace Code.Saves
{    [Serializable]

    public struct TowerData
    {
        public Color color;
        public Vector2 point;
    }

    [Serializable]
    public class TowerSave
    {
        public TowerData[] Items;

        public void Save(DragBreak[] items)
        {
            Items = items
                .Select(e => new TowerData { point = e.transform.position, color = e.color })
                .ToArray();
        }
    }

    [System.Serializable]
    public class SaveManager
    {
        public TowerSave SaveTower = new();

        public void SaveToFile(string fileName = "tower_save.json")
        {
            string json = JsonUtility.ToJson(SaveTower, prettyPrint: true);
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, json);
            Debug.Log($"Сохранено: {path}");
        }

        public void LoadFromFile(string fileName = "tower_save.json")
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл не найден: {path}");
                return;
            }

            string json = File.ReadAllText(path);
            SaveTower = JsonUtility.FromJson<TowerSave>(json);
        }
    }
}