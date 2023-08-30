using System;
using UnityEngine;

namespace Features.BattleSim.Progression
{
    public sealed class UnlockProgress
    {
        [Serializable]
        private class SerializableData
        {
            public bool ExistingData;
            public int PreviousUnitUnlockedAtLevel;
            public int LevelToUnlockNextUnit;
        }

        private const string PLAYER_UNLOCKS_DATA = "player_unlocks_data";

        public int PreviousUnlockLevel => _data.PreviousUnitUnlockedAtLevel;

        private readonly UnitsUnlockMap _unlockMap;
        private readonly SerializableData _data;

        public UnlockProgress(UnitsUnlockMap unlockMap)
        {
            _unlockMap = unlockMap;
            var json = PlayerPrefs.GetString(PLAYER_UNLOCKS_DATA, "{ }");
            _data = JsonUtility.FromJson<SerializableData>(json);

            if (_data.ExistingData)
            {
                return;
            }

            var unlock = _unlockMap.FindNextUnlock(0);
            _data.LevelToUnlockNextUnit = unlock.UnlockLevel;
            _data.PreviousUnitUnlockedAtLevel = 0;
            _data.ExistingData = true;
            Save();
        }

        public void UpdateCompletedLevel(int levelCompleted)
        {
            if (_data.LevelToUnlockNextUnit != levelCompleted)
            {
                return;
            }

            _data.PreviousUnitUnlockedAtLevel = _data.LevelToUnlockNextUnit;
            _data.LevelToUnlockNextUnit = _unlockMap.FindNextUnlock(_data.PreviousUnitUnlockedAtLevel + 1).UnlockLevel;
            Save();
        }

        public UnitUnlockData FindCurrentUnlock(int level) => _unlockMap.FindNextUnlock(level);

        private void Save()
        {
            var json = JsonUtility.ToJson(_data);
            PlayerPrefs.SetString(PLAYER_UNLOCKS_DATA, json);
            PlayerPrefs.Save();
        }
    }
}