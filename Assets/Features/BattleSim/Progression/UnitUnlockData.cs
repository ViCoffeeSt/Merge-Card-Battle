using System;

namespace Features.BattleSim.Progression
{
    [Serializable]
    public class UnitUnlockData
    {
        public static UnitUnlockData Empty = new UnitUnlockData(string.Empty, -1);
        public bool IsAvailable => !string.IsNullOrEmpty(UnitKey) && UnlockLevel > 0;
        public string UnitKey;
        public int UnlockLevel;

        private UnitUnlockData(string unitKey, int unlockLevel)
        {
            UnitKey = unitKey;
            UnlockLevel = unlockLevel;
        }
    }

    public class UnitUnlockProgress
    {
        public bool IsAvailable => _unlockData.IsAvailable;
        public string UnitKey => _unlockData.UnitKey;
        public float Progress => 1f - (_unlockData.UnlockLevel - _currentLevel) / (float) (_unlockData.UnlockLevel - _previousLevel);

        private readonly UnitUnlockData _unlockData;
        private readonly int _previousLevel;
        private readonly int _currentLevel;

        public UnitUnlockProgress(UnitUnlockData unlockData, int previousLevel, int currentLevel)
        {
            _unlockData = unlockData;
            _previousLevel = previousLevel;
            _currentLevel = currentLevel;
        }
    }
}