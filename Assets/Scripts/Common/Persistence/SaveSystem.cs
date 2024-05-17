using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Persistence
{
    public class SaveSystem : PersistentSingleton<SaveSystem>
    {
        public GameData Data;

        private IDataService _dataService;
        private static readonly string _saveName = "Save";

        protected override void Awake()
        {
            base.Awake();
            _dataService = new FileDataService(new JsonSerializer());
            LoadGame();
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "World") return;

            Bind<PlayerController, PlayerData>(Data.PlayerData);
            Bind<ConsumablesManager, ConsumableData>(Data.ConsumableData);
            Bind<BestiaryManager, CreatureData>(Data.CreatureData);
        }

        private void LoadGame()
        {
            try
            {
                Data = _dataService.Load(_saveName);
            }
            catch
            {
                Data = new();
            }
        }

        public void SaveGame()
        {
            _dataService.Save(Data, true);
        }

        private void Bind<T, U>(U data)
            where T : MonoBehaviour, IBind<U>
            where U : ISaveable, new()
        {
            // FIXME: may want to use FindFirstObjectByType instead
            T behavior = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            if (behavior == null) return;
            data ??= new();
            behavior.Bind(data);
        }
    }
}
