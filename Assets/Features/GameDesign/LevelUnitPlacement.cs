using System;
using Features.Cards;
using UnityEngine;

namespace Features.GameDesign
{
    [Serializable]
    public class UnitSpawnInfo
    {
        public static readonly UnitSpawnInfo Empty = new(string.Empty, false);
        public bool IsEmpty => string.IsNullOrWhiteSpace(_key);
        public string UnitKey => _key;
        public bool FrontFace => _frontFace;

        [SerializeField] [Range(0, 2)] public int Row;
        [SerializeField] [Range(0, 3)] public int Col;

        [Space(6)] [SerializeField] public CardTier UnitTier;

        [SerializeField] private string _key;
        private bool _frontFace;

        public UnitSpawnInfo()
        {
            _frontFace = true;
        }

        public UnitSpawnInfo(string key, bool frontFace = true)
        {
            _key = key;
            _frontFace = frontFace;
        }
    }
}