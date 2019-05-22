
namespace StandardCardDeck
{
    public static class DeckConstants
    {
        public enum CardSuit
        {
            Heart,
            Club,
            Diamond,
            Spade
        };

        public enum CardValue
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
            Ace = 14,
            Joker = 15
        };

        public enum PokerHandType
        {
            Unspecified = 0,
            HighCard = 1,
            Pair = 2,
            JacksOrBetter = 3,
            TwoPair = 4,
            ThreeOfAKind = 5,
            Straight = 6,
            Flush = 7,
            FullHouse = 8,
            FourOfAKind = 9,
            StraightFlush = 10,
            FiveOfAKind = 11,
            RoyalFlush = 12
        }

        [System.Serializable]
        public class StandardCard
        {
            public DeckConstants.CardSuit suit;
            public DeckConstants.CardValue value;

            public StandardCard(DeckConstants.CardSuit _suit, DeckConstants.CardValue _value)
            {
                suit = _suit;
                value = _value;
            }
        }
    }
}