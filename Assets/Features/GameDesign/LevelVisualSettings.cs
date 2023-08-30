using UnityEngine;

namespace Features.GameDesign
{
    [CreateAssetMenu(menuName = "Semki Games/Settings/Create LevelVisualSettings",
        fileName = "LevelVisualSettings",
        order = 0)]
    public class LevelVisualSettings : ScriptableObject
    {
        public GameObject Arena => _arenaBox;
        public GameObject Decoration => _arenaDecor;

        [SerializeField] private GameObject _arenaBox;
        [SerializeField] private GameObject _arenaDecor;
    }
}