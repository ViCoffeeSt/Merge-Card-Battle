using System;
using System.Linq;
using Features.Shared;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Scripting;

namespace Features.GameDesign
{
    [CreateAssetMenu(menuName = "Semki Games/Settings/Create CardDeckSettings", fileName = "CardDeckSettings",
        order = 0)]
    public class CardDeckSettings : ScriptableObject
    {
        [NotEditable] [Preserve] public int TotalCards;
        [NotEditable] public int Level = -1;
        [NotEditable] public Vector2Int FieldDimension;
        public CardSetup[] Cards;

        private void OnValidate()
        {
            if (Cards == null)
            {
                return;
            }

            var total = Cards.Sum(x => x.Amount);
            TotalCards = total;

            switch (TotalCards)
            {
                case <= 4:
                    FieldDimension = new Vector2Int(2, 2);
                    break;
                case > 4 and < 7:
                    FieldDimension = new Vector2Int(3, 2);
                    break;
                case >= 7 and < 9:
                    FieldDimension = new Vector2Int(4, 2);
                    break;
                case 9:
                    FieldDimension = new Vector2Int(3, 3);
                    break;
                case > 9 and < 13:
                    FieldDimension = new Vector2Int(4, 3);
                    break;
                default:
                    Assert.IsTrue(false, $"Not supported amount of cards - {total}. Max. allowed: 12");
                    break;
            }

            if (Level == -1)
            {
                var levelNumber = name.Substring(6);
                if (!int.TryParse(levelNumber, out var levelNumberParsed))
                {
                    throw new ArgumentException("Name of the file is in wrong format! Use Level_<number> format.");
                }

                Level = levelNumberParsed;
            }
        }
    }
}