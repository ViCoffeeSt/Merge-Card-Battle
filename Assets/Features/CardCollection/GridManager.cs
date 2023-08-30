using System.Collections.Generic;
using Features.Cards;
using Features.GameDesign;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CardCollection
{
    public class GridManager : MonoBehaviour, ILevelEnergy
    {
        public bool EnoughEnergy => _isEnoughEnergy;

        [SerializeField] private GridLayoutGroup _gridContainer;
        [SerializeField] private Slider _energySlider;
        [SerializeField] private TMP_Text _levelLabel;

        private Card[][] _cardsGrid;
        private GameSettings _gameSettings;
        private float _currentEnergy;
        private bool _isEnoughEnergy;

        public void PopulateGrid(GameSettings gameSettings, ICardFactory cardFactory, CardDeckSettings cardDeckSettings)
        {
            _gameSettings = gameSettings;

            _currentEnergy = _gameSettings.MaxEnergy;
            _energySlider.maxValue = _gameSettings.MaxEnergy;
            _energySlider.value = _currentEnergy;
            _isEnoughEnergy = true;

            _energySlider.gameObject.SetActive(_gameSettings.EnergyCostPerCard > 0);

            _levelLabel.text = "Level " + cardDeckSettings.Level;

            var rowsCount = cardDeckSettings.FieldDimension.y;
            var columnsCount = cardDeckSettings.FieldDimension.x;

            _gridContainer.constraintCount = _gridContainer.constraint == GridLayoutGroup.Constraint.FixedColumnCount
                ? columnsCount
                : rowsCount;

            _cardsGrid = new Card[rowsCount][];
            for (var i = 0; i < _cardsGrid.Length; i++)
            {
                _cardsGrid[i] = new Card[columnsCount];
            }

            var counter = 0;
            foreach (var setup in cardDeckSettings.Cards)
            {
                for (var i = 0; i < setup.Amount; i += 2)
                {
                    var card = cardFactory.Create(setup.UnitKey, CardTier.Basic);
                    card.transform.SetParent(_gridContainer.transform, false);
                    card.ShowFrontImage();

                    {
                        var r = counter % rowsCount;
                        var c = counter / rowsCount;
                        _cardsGrid[r][c] = card;
                        counter++;
                    }

                    var pair = cardFactory.Create(setup.UnitKey, CardTier.Basic);
                    pair.transform.SetParent(_gridContainer.transform, false);
                    pair.ShowFrontImage();

                    {
                        var r = counter % rowsCount;
                        var c = counter / rowsCount;
                        _cardsGrid[r][c] = pair;
                        counter++;
                    }
                }
            }

            while (counter > 1)
            {
                counter--;

                var randomIndex = Random.Range(0, cardDeckSettings.TotalCards);

                var fromR = counter % rowsCount;
                var fromC = counter / rowsCount;

                var toR = randomIndex % rowsCount;
                var toC = randomIndex / rowsCount;

                var a = _cardsGrid[fromR][fromC];
                var b = _cardsGrid[toR][toC];

                _cardsGrid[fromR][fromC] = b;
                _cardsGrid[toR][toC] = a;

                a.transform.SetSiblingIndex(randomIndex);
                b.transform.SetSiblingIndex(counter);
            }
        }

        public void ShowAll()
        {
            foreach (var row in _cardsGrid)
            {
                foreach (var card in row)
                {
                    if (!card)
                    {
                        continue;
                    }

                    card.ShowFrontImage();
                }
            }
        }

        public void HideAll()
        {
            foreach (var row in _cardsGrid)
            {
                foreach (var card in row)
                {
                    if (!card)
                    {
                        continue;
                    }

                    card.FlipCard();
                }
            }
        }

        public void EnergyConsumptionPerClick()
        {
            if (_currentEnergy > _gameSettings.MinEnergy)
            {
                _currentEnergy -= _gameSettings.EnergyCostPerCard;
                _energySlider.value = _currentEnergy;
            }
            else
            {
                _isEnoughEnergy = false;
                _energySlider.value = _gameSettings.MinEnergy;
            }
        }

        public bool IsMergeCompleted(IReadOnlyCollection<Card> selectedCards)
        {
            return !EnoughEnergy || !HasClosedCards(selectedCards);
        }

        public UnitSpawnInfo[] GetCardsGrid()
        {
            var placements = new UnitSpawnInfo[_cardsGrid.Length * _cardsGrid[0].Length];
            for (var r = 0; r < _cardsGrid.Length; r++)
            {
                var row = _cardsGrid[r];
                for (var c = 0; c < row.Length; c++)
                {
                    var card = row[c];

                    var i = r * row.Length + c;
                    var placement = placements[i] =
                        card && card.Active ? new UnitSpawnInfo(card.UnitCard.UnitKey, false) : UnitSpawnInfo.Empty;

                    if (!card || !card.Active)
                    {
                        continue;
                    }

                    placement.UnitTier = card.UnitCard.Tier;
                    placement.Row = r;
                    placement.Col = c;
                }
            }

            return placements;
        }

        private bool HasClosedCards(IReadOnlyCollection<Card> selectedCards)
        {
            var activeCardsCount = 0;
            foreach (var row in _cardsGrid)
            {
                foreach (var card in row)
                {
                    if (!card)
                    {
                        continue;
                    }

                    if (card.Active)
                    {
                        activeCardsCount++;
                    }
                }
            }

            return selectedCards.Count != activeCardsCount;
        }
    }
}