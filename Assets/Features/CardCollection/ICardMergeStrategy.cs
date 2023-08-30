using Features.Cards;

namespace Features.CardCollection
{
    public interface ICardMergeStrategy
    {
        bool TwoCardsClosed();
        void MergeCard(Card card);
    }
}