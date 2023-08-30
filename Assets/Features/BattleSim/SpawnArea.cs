using Features.Shared;
using UnityEngine;

namespace Features.BattleSim
{
    public class SpawnArea : MonoBehaviour
    {
        public Bounds UserUnitsArea => _userUnitsArea;
        public Bounds LevelUnitsArea => _levelUnitsArea;

        [SerializeField] private float _centerOffset = 7.5f;
        [SerializeField] [Range(0.01f, 100)] private float _width = 35f;
        [SerializeField] [Range(0.01f, 100)] private float _height = 28f;

        [Space(6f)] [SerializeField] [NotEditable]
        private Bounds _userUnitsArea;

        [SerializeField] [NotEditable] private Bounds _levelUnitsArea;

        private void OnValidate()
        {
            var tr = transform;
            var localPosition = tr.position;

            var size = new Vector3(_width * 0.5f, 2, _height * 0.5f);

            var center = localPosition + Vector3.back * _centerOffset;
            center.y = 1;

            _userUnitsArea = new Bounds(center, size);

            center = localPosition + Vector3.forward * _centerOffset;
            center.y = 1;

            _levelUnitsArea = new Bounds(center, size);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_userUnitsArea.center, _userUnitsArea.size);

            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawWireCube(_levelUnitsArea.center, _levelUnitsArea.size);
        }
    }
}