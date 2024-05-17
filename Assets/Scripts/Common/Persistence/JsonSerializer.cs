using UnityEngine;

namespace Common.Persistence
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string repr)
        {
            return JsonUtility.FromJson<T>(repr);
        }

        public string Serialize<T>(T obj)
        {
            return JsonUtility.ToJson(obj, true);
        }
    }
}
