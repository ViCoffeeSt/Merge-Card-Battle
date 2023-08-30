using Features.Cards;

namespace Features.CardCollection
{
    public class PoolBasedCardCollectionStrategy : ICardCollectionStrategy
    {
        private readonly ICardMergeStrategy _mergeStrategy;

        public PoolBasedCardCollectionStrategy(ICardMergeStrategy mergeStrategy)
        {
            _mergeStrategy = mergeStrategy;
        }

        public void CollectCard(Card card)
        {
            if (!_mergeStrategy.TwoCardsClosed())
            {
                return;
            }

            card.ShowFrontImage();
            _mergeStrategy.MergeCard(card);
        }
    }
}