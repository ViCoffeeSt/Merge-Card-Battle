using UnityEngine;

namespace Features.BattleSim.Progression
{
    [CreateAssetMenu(menuName = "Semki Games/Settings/Create UnitsUnlockMap", fileName = "UnitsUnlockMap", order = 0)]
    public class UnitsUnlockMap : ScriptableObject
    {
        [SerializeField] private UnitUnlockData[] _unlocks;

        public UnitUnlockData FindNextUnlock(int currentLevel)
        {
            foreach (var unlock in _unlocks)
            {
                if (unlock.UnlockLevel >= currentLevel)
                {
                    return unlock;
                }
            }

            return UnitUnlockData.Empty;
        }
    }
}