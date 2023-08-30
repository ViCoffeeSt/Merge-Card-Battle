using Features.Cards;
using UnityEngine;

namespace Features.BattleSim.Units
{
    public interface IUnitSpritesProvider
    {
        Sprite GetUnitSprite(string unitKey, CardTier tier);
        Sprite GetUnitFillerSprite(string unitKey);

        Sprite GetFaceImage(string unitKey, CardTier tier);
    }
}