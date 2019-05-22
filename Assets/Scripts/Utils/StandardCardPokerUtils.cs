using StandardCardDeck;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class that contains function used on a standard poker deck/hand in order to evaluate what is in it.
/// </summary>
public static class StandardCardPokerUtils
{
    private readonly static int SET_1 = 0;
    private readonly static int SET_2 = 1;

    /// <summary>
    ///  Returns true if all the cards in the hand are 10 or above in value.
    /// </summary>
    /// <param name="handToEvaluate"></param>
    /// <returns></returns>
    public static bool AreAllTenOrAbove(List<DeckConstants.StandardCard> handToEvaluate)
    {
        int matchedCount = 0;
        for (int i = 0; i < handToEvaluate.Count; ++i)
        {
            if ((int)handToEvaluate[i].value >= (int)DeckConstants.CardValue.Ten)
            {
                matchedCount++;
            }
        }

        return matchedCount == handToEvaluate.Count;
    }

    /// <summary>
    ///  Returns true if all cards in the hand are in the same suit.
    /// </summary>
    /// <param name="handToEvaluate"></param>
    /// <returns></returns>
    public static bool AreAllCardsInHandTheSameSuit(List<DeckConstants.StandardCard> handToEvaluate)
    {
        Debug.Assert(handToEvaluate.Count > 0, "AreAllCardsInHandTheSameSuit: Unable to evaluate hand which contains a single element.");
        for (int i = 1; i < handToEvaluate.Count; ++i)
        {
            if (handToEvaluate[i - 1].suit != handToEvaluate[i].suit)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns true if the cards in the hand are in sequential order.
    /// </summary>
    /// <param name="handToEvaluate"></param>
    /// <returns></returns>
    public static bool IsStraight(List<DeckConstants.StandardCard> handToEvaluate)
    {
        //Sort ascending
        handToEvaluate.Sort((pokerCard1, pokerCard2) => pokerCard1.value.CompareTo(pokerCard2.value));

        const int firstCardIndex = 0;
        return AreCardsInSequence(handToEvaluate, firstCardIndex + 2) && (IsSequentialValue(handToEvaluate[firstCardIndex], handToEvaluate[firstCardIndex + 1]) || IsFirstCardAceAndLastCardKing(handToEvaluate));
    }

    /// <summary>
    /// Utility function used to find sets of cards that have the same value.
    /// </summary>
    /// <param name="pokerHand"></param>
    /// <returns></returns>
    public static List<StandardCardValueSet> FindSetsOfCardsWithSameValue(List<DeckConstants.StandardCard> handToEvaluate)
    {
        Dictionary<int, List<int>> uniqueCardValues = new Dictionary<int, List<int>>();

        // Iterate over the hand and add unique cards to the dictionary.
        for (int i = 0; i < handToEvaluate.Count; ++i)
        {
            List<int> cardIndicies;
            if (!uniqueCardValues.TryGetValue((int)handToEvaluate[i].value, out cardIndicies))
            {
                cardIndicies = new List<int>();
                cardIndicies.Add(i);
                uniqueCardValues.Add((int)handToEvaluate[i].value, cardIndicies);
                continue;
            }

            cardIndicies.Add(i);
        }

        // Iterate over items in the dictionary and add them to the value set
        // if there is more than 1 entry.
        List<StandardCardValueSet> sameValueSet = new List<StandardCardValueSet>();
        foreach (KeyValuePair<int, List<int>> entry in uniqueCardValues)
        {
            if (entry.Value.Count > 1)
            {
                sameValueSet.Add(new StandardCardValueSet(entry.Value));
            }
        }

        return sameValueSet;
    }

    /// <summary>
    ///  Returns the type of poker hand based off the hand of cards passed in.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="pokerHand"></param>
    /// <returns></returns>
    public static DeckConstants.PokerHandType DeterminePokerHandType(StandardCardPokerDataContext dataContext, List<DeckConstants.StandardCard> handToEvaluate)
    {
        if (dataContext.AllSameSuit && dataContext.Straight && dataContext.AllRoyals)
        {
            return DeckConstants.PokerHandType.RoyalFlush;
        }

        //Determine Poker Hand Type
        if (dataContext.AllSameSuit && dataContext.Straight)
        {
            return DeckConstants.PokerHandType.StraightFlush;
        }

        if (dataContext.AllSameSuit)
        {
            return DeckConstants.PokerHandType.Flush;
        }

        if (dataContext.Straight)
        {
            return DeckConstants.PokerHandType.Straight;
        }

        List<StandardCardValueSet> sameCardSets = dataContext.SameCardSets;

        //Continue Determining Poker Hand Type
        if (sameCardSets.Count > 0 && sameCardSets[SET_1].IndexList.Count == 4)
        {
            return DeckConstants.PokerHandType.FourOfAKind;
        }

        if (sameCardSets.Count > 1 && sameCardSets[SET_1].IndexList.Count + sameCardSets[SET_2].IndexList.Count == 5)
        {
            return DeckConstants.PokerHandType.FullHouse;
        }

        if (sameCardSets.Count > 0 && sameCardSets[SET_1].IndexList.Count == 3)
        {
            return DeckConstants.PokerHandType.ThreeOfAKind;
        }

        if (sameCardSets.Count > 1 && sameCardSets[SET_1].IndexList.Count + sameCardSets[SET_2].IndexList.Count == 4)
        {
            return DeckConstants.PokerHandType.TwoPair;
        }

        if (sameCardSets.Count > 0 && sameCardSets[SET_1].IndexList.Count == 2)
        {
            return DeckConstants.PokerHandType.Pair;
        }

        return DeckConstants.PokerHandType.HighCard;
    }

    #region Local Utility Functions

    /// <summary>
    /// Utility function used to examine 2 values to determine if the two cards are sequential in order.
    /// </summary>
    /// <param name="cardA"></param>
    /// <param name="cardB"></param>
    /// <returns></returns>
    private static bool IsSequentialValue(DeckConstants.StandardCard cardA, DeckConstants.StandardCard cardB)
    {
        return cardA.value == cardB.value - 1;
    }

    /// <summary>
    /// Utility function used to determine if the first card is an ace and the last card in the hand is a king.
    /// </summary>
    /// <param name="handToEvaluate"></param>
    /// <returns></returns>
    private static bool IsFirstCardAceAndLastCardKing(List<DeckConstants.StandardCard> handToEvaluate)
    {
        return (handToEvaluate[0].value == DeckConstants.CardValue.Ace && handToEvaluate[GameUtils.GetLastElementInList<DeckConstants.StandardCard>(handToEvaluate)].value == DeckConstants.CardValue.King);
    }

    /// <summary>
    /// Utility function used to determine if cards are in sequence.
    /// </summary>
    /// <param name="handToEvaluate"></param>
    /// <param name="beginIndex"></param>
    /// <returns></returns>
    private static bool AreCardsInSequence(List<DeckConstants.StandardCard> handToEvaluate, int beginIndex)
    {
        Debug.Assert(beginIndex + 1 < handToEvaluate.Count, "AreCardsInSequence: Unable to evaluate hand. beginIndex + 1 is larger than the number of elements in the evaluation hand.");
        for (int i = beginIndex + 1; i < handToEvaluate.Count; ++i)
        {
            if (!IsSequentialValue(handToEvaluate[i - 1], handToEvaluate[i]))
            {
                return false;
            }
        }

        return true;
    }

    #endregion Local Utility Functions
}