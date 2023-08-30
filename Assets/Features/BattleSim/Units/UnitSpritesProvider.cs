using System.Collections.Generic;
using Features.Cards;
using UnityEngine;
using UnityEngine.U2D;

namespace Features.BattleSim.Units
{
    public class UnitSpritesProvider : IUnitSpritesProvider
    {
        private Dictionary<string, SpriteAtlas> _cache = new(24);

        public Sprite GetUnitSprite(string unitKey, CardTier tier)
        {
            var atlas = GetUnitAtlas(unitKey);

            var key = unitKey + "_" + tier;
            return atlas.GetSprite(key);
        }

        public Sprite GetUnitFillerSprite(string unitKey)
        {
            var atlas = GetUnitAtlas(unitKey);

            var key = unitKey + "_Fill";
            return atlas.GetSprite(key);
        }

        public Sprite GetFaceImage(string unitKey, CardTier tier)
        {
            var atlas = GetUnitAtlas(unitKey);
            var key = unitKey + "_Face_" + tier;
            return atlas.GetSprite(key);
        }

        private SpriteAtlas GetUnitAtlas(string unitKey)
        {
            if (!_cache.TryGetValue(unitKey, out var atlas))
            {
                atlas = Resources.Load<SpriteAtlas>(unitKey);
                _cache.Add(unitKey, atlas);
            }

            return atlas;
        }
    }
}