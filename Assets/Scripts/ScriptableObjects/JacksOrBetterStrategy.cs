using StandardCardDeck;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JacksOrBetterStrategy", menuName = "Poker/JacksOrBetterStrategy", order = 1)]
public class JacksOrBetterStrategy : ScriptableObject, IStandardCardPokerStrategy
{
    #region Class Constants

    private const int numberOfCardsInHand = 5;
    private const int handsPerRound = 2;
    private const int numberOfWinTypesForJacksOrBetter = 9;
    private const int validCardMin = (int)DeckConstants.CardValue.Two;
    private const int validCardMax = (int)DeckConstants.CardValue.Ace;

    private const string RoyalFlushWinViewName = "Royal Flush";
    private const string StraightFlushWinViewName = "Straight Flush";
    private const string FourOfAKindWinViewName = "4 of a Kind";
    private const string FullHouseWinViewName = "Full House";
    private const string FlushWinViewName = "Flush";
    private const string StraightWinViewName = "Straight";
    private const string ThreeOfAKindWinViewName = "3 of a Kind";
    private const string TwoPairWinViewName = "2 Pair";
    private const string JacksOrBetterWinViewName = "Jacks Or Better";

    #endregion Class Constants

    #region Class Functions

    /// <summary>
    /// Generates a list of win types to display on the top screen, so that it can be populated.
    /// </summary>
    /// <param name="winNames"></param>
    /// <returns></returns>
    public void GenerateWinViewNamesList(out List<string> winNames)
    {
        string[] stringArray = CreateTemporaryStringArray();

        foreach (DeckConstants.PokerHandType item in Enum.GetValues(typeof(DeckConstants.PokerHandType)))
        {
            switch (item)
            {
                case DeckConstants.PokerHandType.RoyalFlush: stringArray[0] = RoyalFlushWinViewName; break;
                case DeckConstants.PokerHandType.StraightFlush: stringArray[1] = StraightFlushWinViewName; break;
                case DeckConstants.PokerHandType.FourOfAKind: stringArray[2] = FourOfAKindWinViewName; break;
                case DeckConstants.PokerHandType.FullHouse: stringArray[3] = FullHouseWinViewName; break;
                case DeckConstants.PokerHandType.Flush: stringArray[4] = FlushWinViewName; break;
                case DeckConstants.PokerHandType.Straight: stringArray[5] = StraightWinViewName; break;
                case DeckConstants.PokerHandType.ThreeOfAKind: stringArray[6] = ThreeOfAKindWinViewName; break;
                case DeckConstants.PokerHandType.TwoPair: stringArray[7] = TwoPairWinViewName; break;
                case DeckConstants.PokerHandType.JacksOrBetter: stringArray[8] = JacksOrBetterWinViewName; break;
                default: break;
            }
        }

        winNames = new List<string>(stringArray);
    }

    /// <summary>
    /// Utility function used to return a string to display in info text.
    /// </summary>
    /// <param name="handType"></param>
    /// <returns></returns>
    public string GetInfoTextStringForHandType(DeckConstants.PokerHandType handType)
    {
        switch (handType)
        {
            case DeckConstants.PokerHandType.RoyalFlush: return RoyalFlushWinViewName;
            case DeckConstants.PokerHandType.StraightFlush: return StraightFlushWinViewName; 
            case DeckConstants.PokerHandType.FourOfAKind: return FourOfAKindWinViewName;
            case DeckConstants.PokerHandType.FullHouse: return FullHouseWinViewName; 
            case DeckConstants.PokerHandType.Flush: return FlushWinViewName;
            case DeckConstants.PokerHandType.Straight: return StraightWinViewName;
            case DeckConstants.PokerHandType.ThreeOfAKind: return ThreeOfAKindWinViewName;
            case DeckConstants.PokerHandType.TwoPair: return TwoPairWinViewName;
            case DeckConstants.PokerHandType.JacksOrBetter: return JacksOrBetterWinViewName;
            default: break;
        }

        return string.Empty;
    }

    /// <summary>
    /// Overloading the regular DeterminePokerHandType function to check for JacksOrWild specific conditions.
    /// </summary>
    /// <param name="pokerHand"></param>
    /// <returns></returns>
    public DeckConstants.PokerHandType DeterminePokerHandType(List<DeckConstants.StandardCard> pokerHand)
    {
        // Copy the hand as to not to destroy the order of the original hand when we sort it.
        List<DeckConstants.StandardCard> handCopy = CopyHand(pokerHand);

        // NOTE: It would be possible to save a bit of performance by doing these calculations individually and returning immediately if they succeed or fail,
        //       however this implementation allows for the ability to over ride the standard poker return values and do additional evaluations to check for
        //       strategy specific information such as JacksOrBetter.
        StandardCardPokerDataContext jacksOrBetterDataContext = new StandardCardPokerDataContext(StandardCardPokerUtils.AreAllCardsInHandTheSameSuit(handCopy),
                                                                                                 StandardCardPokerUtils.IsStraight(handCopy),
                                                                                                 StandardCardPokerUtils.AreAllTenOrAbove(handCopy),
                                                                                                 StandardCardPokerUtils.FindSetsOfCardsWithSameValue(handCopy));

        DeckConstants.PokerHandType handType = StandardCardPokerUtils.DeterminePokerHandType(jacksOrBetterDataContext, handCopy);

        if (handType == DeckConstants.PokerHandType.Pair)
        {
            bool isJackOrGreater = (jacksOrBetterDataContext.SameCardSets.Count > 0 &&
                                    (int)handCopy[jacksOrBetterDataContext.SameCardSets[0].IndexList[0]].value >= (int)DeckConstants.CardValue.Jack);
            if (isJackOrGreater)
            {
                return DeckConstants.PokerHandType.JacksOrBetter;
            }
        }

        return handType;
    }

    /// <summary>
    /// Generates a new card hold array.
    /// </summary>
    /// <returns></returns>
    public List<bool> CreateNewCardHoldArray()
    {
        List<bool> cardHoldArray = new List<bool>();

        for (int i = 0; i < numberOfCardsInHand; ++i)
        {
            cardHoldArray.Add(false);
        }

        return cardHoldArray;
    }

    /// <summary>
    /// Generate a deck for use with the Jacks or Wild Poker Game.
    /// </summary>
    /// <returns></returns>
    public List<DeckConstants.StandardCard> CreateNewDeck()
    {
        return StandardCardUtils.CreateNewDeck(validCardMin, validCardMax);
    }

    /// <summary>
    /// Shuffles the Jacks or Wild poker game deck.
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="RNG"></param>
    /// <returns></returns>
    public void ShuffleDeck(List<DeckConstants.StandardCard> deck, System.Random RNG)
    {
        StandardCardUtils.ShuffleDeck(deck, RNG);
    }

    /// <summary>
    /// Utility function used to return the lowest card in play for this type of poker game.
    /// </summary>
    /// <returns></returns>
    public int GetCardValueMin()
    {
        return validCardMin;
    }

    /// <summary>
    /// Utility function used to return the highest card in play for this type of poker game.
    /// </summary>
    /// <returns></returns>
    public int GetCardValueMax()
    {
        return validCardMax;
    }

    /// <summary>
    /// Utility function used to return number of hands per round this poker game has.
    /// </summary>
    /// <returns></returns>
    public int GetHandsPerRound()
    {
        return handsPerRound;
    }

    /// <summary>
    /// Handles dealing cards for the jacks or better poker game.
    /// </summary>
    /// <param name="currentHand"></param>
    /// <param name="cardsToHold"></param>
    /// <param name="deckStack"></param>
    /// <returns></returns>
    public void DealCards(List<DeckConstants.StandardCard> currentHand, List<bool> cardsToHold, Stack<DeckConstants.StandardCard> deckStack)
    {
        Stack<DeckConstants.StandardCard> currentDeal = Deal(CalculateNumberOfCardsToDraw(cardsToHold), deckStack);

        if (currentHand == null || currentHand.Count == 0)
        {
            InitializeHand(currentHand);
        }

        for (int i = 0; i < cardsToHold.Count; ++i)
        {
            if (cardsToHold[i] == false)
            {
                currentHand[i] = currentDeal.Pop();
            }
        }
    }

    #endregion Class Functions

    #region Local Utility Functions

    /// <summary>
    /// Utility function that handles dealing cards for the jacks or better poker game.
    /// </summary>
    /// <param name="numberOfCards"></param>
    /// <param name="deckStack"></param>
    /// <returns></returns>
    private Stack<DeckConstants.StandardCard> Deal(int numberOfCards, Stack<DeckConstants.StandardCard> deckStack)
    {
        Stack<DeckConstants.StandardCard> cards = new Stack<DeckConstants.StandardCard>();
        for (int i = 0; i < numberOfCards; ++i)
        {
            cards.Push(deckStack.Pop());
        }

        return cards;
    }

    /// <summary>
    /// Utility function that returns the number of cards to draw from the deck based off the number of cards being held, and the size of the hand.
    /// </summary>
    /// <param name="cardsToHold"></param>
    /// <returns></returns>
    private int CalculateNumberOfCardsToDraw(List<bool> cardsToHold)
    {
        int counter = 0;
        for (int i = 0; i < cardsToHold.Count; ++i)
        {
            if (cardsToHold[i])
            {
                counter++;
            }
        }

        return numberOfCardsInHand - counter;
    }

    /// <summary>
    /// Utility function used to initialize an empty hand the first time a hand is dealt.
    /// </summary>
    /// <param name="currentHand"></param>
    /// <returns></returns>
    private void InitializeHand(List<DeckConstants.StandardCard> currentHand)
    {
        for (int i = 0; i < numberOfCardsInHand; ++i)
        {
            currentHand.Add(null);
        }
    }

    /// <summary>
    /// Utility function used to make a copy of the passed in hand so that when we sort the hand
    /// in order to evaluate the values in it we aren't actually changing the positions or the
    /// original cards in the hand as this causes the held card positions to rotate.
    /// </summary>
    /// <param name="pokerHand"></param>
    /// <returns></returns>
    private List<DeckConstants.StandardCard> CopyHand(List<DeckConstants.StandardCard> pokerHand)
    {
        List<DeckConstants.StandardCard> handCopy = new List<DeckConstants.StandardCard>(pokerHand.Count);
        for (int i = 0; i < pokerHand.Count; ++i)
        {
            handCopy.Add(new DeckConstants.StandardCard(pokerHand[i].suit, pokerHand[i].value));
        }

        return handCopy;
    }

    /// <summary>
    /// Utility function used to generate a temporary string array.
    /// </summary>
    /// <returns></returns>
    private string[] CreateTemporaryStringArray()
    {
        string[] stringArray = new string[numberOfWinTypesForJacksOrBetter]
        {
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
        };

        return stringArray;
    }

    #endregion Local Utility Functions
}