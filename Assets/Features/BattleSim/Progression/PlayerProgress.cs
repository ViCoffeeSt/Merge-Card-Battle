using System;
using UnityEngine;

namespace Features.BattleSim.Progression
{
    public class PlayerProgress
    {
        [Serializable]
        private class SerializableData
        {
            public int LastLevelCompleted = 1;
        }

        private const string PLAYER_PROGRESS_DATA = "player_progress_data";

        public int CurrentLevel => _data.LastLevelCompleted;
        private SerializableData _data;

        public PlayerProgress()
        {
            var json = PlayerPrefs.GetString(PLAYER_PROGRESS_DATA, "{\"LastLevelCompleted\": 1}");
            _data = JsonUtility.FromJson<SerializableData>(json);
        }

        public void ProgressToNextLevel()
        {
            _data.LastLevelCompleted++;
            Save();
        }

        private void Save()
        {
            var json = JsonUtility.ToJson(_data);
            PlayerPrefs.SetString(PLAYER_PROGRESS_DATA, json);
            PlayerPrefs.Save();
        }
    }
}