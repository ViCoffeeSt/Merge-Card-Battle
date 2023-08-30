using System;
using Features.Cards;
using UnityEngine;

namespace Features.GameDesign
{
    [Serializable]
    public class CardSetup
    {
        public string UnitKey;
        [Range(2, 12)] public int Amount;
    }
}