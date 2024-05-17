using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace Common.Persistence
{
    public class FileDataService : IDataService
    {
        private ISerializer serializer;
        private string dataPath;
        private string fileExtension;

        public FileDataService(ISerializer serializer)
        {
            dataPath = Application.persistentDataPath;
            fileExtension = "sd";
            this.serializer = serializer;
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(dataPath, string.Concat(fileName, ".", fileExtension));
        }

        public void Save(GameData data, bool overwrite = true)
        {
            string filePath = GetFilePath(data.Name);

            if (!overwrite && File.Exists(filePath))
                throw new IOException($"{filePath} already exists.");

            File.WriteAllText(filePath, serializer.Serialize(data));
        }

        public GameData Load(string name)
        {
            string filePath = GetFilePath(name);
            if (!File.Exists(filePath))
                throw new ArgumentException($"Save data of name {name} does not exist.");

            string repr = File.ReadAllText(filePath);
            return serializer.Deserialize<GameData>(repr);
        }

    }
}
