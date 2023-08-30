using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Features.Cards;
using Features.GameDesign;
using Features.Shared;
using UnityEngine;

namespace Features.CardCollection
{
    public class EnergyBasedMergeStrategy : ICardMergeStrategy
    {
        private readonly GridManager _gridManager;
        private readonly ICommand<UnitSpawnInfo[]> _mergeCompletionCommand;
        private readonly IRoutineRunner _routineRunner;
        private readonly GameSettings _config;

        private List<Card> _selectedCards = new(12);
        private List<Card> _mergedCards = new();

        private bool _isStopMerge = false;

        public EnergyBasedMergeStrategy(GameSettings gameSettings,
            GridManager gridManager,
            ICommand<UnitSpawnInfo[]> mergeCompletionCommand,
            IRoutineRunner routineRunner)
        {
            _config = gameSettings;
            _gridManager = gridManager;
            _mergeCompletionCommand = mergeCompletionCommand;
            _routineRunner = routineRunner;
        }

        public void MergeCard(Card card)
        {
            _selectedCards.Add(card);

            if (_selectedCards.Count <= 1)
            {
                return;
            }

            CompareCards();
        }

        public bool TwoCardsClosed() => _config.KeepCardsOpen || _selectedCards.Count < 2;

        private async void CompareCards()
        {
            var newest = _selectedCards[^1];
            var previous = _selectedCards[^2];
            var merged = false;

            var mergePipeline = Task.CompletedTask;
            if (previous.UnitCard == newest.UnitCard)
            {
                _selectedCards.Remove(previous);

                mergePipeline = newest.Merge(previous);
                _mergedCards.Add(newest);
                _selectedCards.Remove(newest);
                merged = true;
            }

            if (merged)
            {
                for (var i = 0; i < _mergedCards.Count - 1; i++)
                {
                    previous = _mergedCards[i];

                    if (previous.UnitCard != newest.UnitCard)
                    {
                        continue;
                    }

                    await mergePipeline;
                    mergePipeline = newest.Merge(previous);
                    _mergedCards.Remove(previous);

                    i = -1;
                }
            }

            await mergePipeline;

            if (_gridManager.IsMergeCompleted(_mergedCards))
            {
                var placements = _gridManager.GetCardsGrid();
                _mergeCompletionCommand.Execute(placements);
                _isStopMerge = true;
            }

            if (merged || _config.KeepCardsOpen)
            {
                return;
            }

            foreach (var card in _selectedCards)
            {
                if (_mergedCards.Contains(card))
                {
                    continue;
                }

                if (!_isStopMerge)
                {
                    _routineRunner.StartCoroutine(FlipCardCoroutine(card));
                }
            }

            _selectedCards.Clear();
        }

        private IEnumerator FlipCardCoroutine(Card card)
        {
            var delay = _config.CardFlipDelayInSeconds;
            yield return new WaitForSeconds(delay);

            card.FlipCard();
        }
    }
}