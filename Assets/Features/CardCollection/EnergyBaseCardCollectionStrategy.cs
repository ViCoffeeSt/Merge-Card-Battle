using System;
using Features.Cards;

namespace Features.CardCollection
{
    public class EnergyBasedCardCollectionStrategy : ICardCollectionStrategy
    {
        private readonly ILevelEnergy _levelEnergy;
        private readonly ICardMergeStrategy _mergeStrategy;

        public EnergyBasedCardCollectionStrategy(
            ILevelEnergy levelEnergy,
            ICardMergeStrategy mergeStrategy)
        {
            _levelEnergy = levelEnergy;
            _mergeStrategy = mergeStrategy;
        }

        public void CollectCard(Card card)
        {
            if (!_mergeStrategy.TwoCardsClosed())
            {
                return;
            }

            _levelEnergy.EnergyConsumptionPerClick();

            card.ShowFrontImage();
            _mergeStrategy.MergeCard(card);
        }
    }
}