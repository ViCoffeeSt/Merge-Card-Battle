using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Features.Cards;
using Features.GameDesign;
using Features.Shared;
using UnityEngine;

namespace Features.CardCollection
{
    public class PoolBasedMergeStrategy : ICardMergeStrategy
    {
        private readonly GridManager _gridManager;
        private readonly ICommand<UnitSpawnInfo[]> _mergeCompletionCommand;
        private readonly GameSettings _config;
        private readonly IRoutineRunner _routineRunner;

        private List<Card> _selectedCards = new(12);

        public PoolBasedMergeStrategy(GameSettings gameSettings,
            GridManager gridManager,
            ICommand<UnitSpawnInfo[]> mergeCompletionCommand,
            IRoutineRunner routineRunner)
        {
            _config = gameSettings;
            _routineRunner = routineRunner;
            _gridManager = gridManager;
            _mergeCompletionCommand = mergeCompletionCommand;
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
                merged = true;
            }

            if (merged)
            {
                for (var i = 0; i < _selectedCards.Count - 1; i++)
                {
                    previous = _selectedCards[i];
                    if (previous.UnitCard != newest.UnitCard)
                    {
                        continue;
                    }

                    await mergePipeline;
                    mergePipeline = newest.Merge(previous);
                    _selectedCards.Remove(previous);

                    i = -1;
                }
            }

            await mergePipeline;

            if (_gridManager.IsMergeCompleted(_selectedCards))
            {
                var cardsGrid = _gridManager.GetCardsGrid();
                _mergeCompletionCommand.Execute(cardsGrid);
            }

            if (merged || _config.KeepCardsOpen)
            {
                return;
            }

            foreach (var card in _selectedCards)
            {
                _routineRunner.StartCoroutine(FlipCardCoroutine(card));
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