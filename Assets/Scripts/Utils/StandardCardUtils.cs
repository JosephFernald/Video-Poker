using StandardCardDeck;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class that function used to operate on a standard card deck.
/// </summary>
public static class StandardCardUtils
{
    /// <summary>
    /// Utility function used to create a new standard card deck.
    /// </summary>
    /// <param name="cardValueMin"></param>
    /// <param name="cardValueMax"></param>
    /// <returns></returns>
    public static List<DeckConstants.StandardCard> CreateNewDeck(int cardValueMin, int cardValueMax)
    {
        Debug.Assert(cardValueMin != cardValueMax, "Unable to create a new deck because the minimum value card is the same as the maximum valid card.");
        List<DeckConstants.StandardCard> tmpList = new List<DeckConstants.StandardCard>();
        foreach (var suit in Enum.GetValues(typeof(DeckConstants.CardSuit)))
        {
            for (int j = cardValueMin; j <= cardValueMax; ++j)
            {
                tmpList.Add(new DeckConstants.StandardCard((DeckConstants.CardSuit)suit, (DeckConstants.CardValue)j));
            }
        }

        return tmpList;
    }

    /// <summary>
    /// Utility function used to shuffle a standard card deck.
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="RNG"></param>
    /// <returns></returns>
    public static void ShuffleDeck(List<DeckConstants.StandardCard> deck, System.Random RNG)
    {
        Debug.Assert(deck.Count > 0, "Unable to shuffle a deck that has a size of 0.");
        for (int i = deck.Count - 1; i > 0; --i)
        {
            SwapCards(deck, i, RNG.Next(i + 1));
        }
    }

    #region Local Utility Functions

    /// <summary>
    /// Swap function used to swap positions of 2 cards in a list.
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    /// <returns></returns>
    private static void SwapCards(List<DeckConstants.StandardCard> cards, int indexA, int indexB)
    {
        DeckConstants.StandardCard tempswap = cards[indexA];
        cards[indexA] = cards[indexB];
        cards[indexB] = tempswap;
    }

    #endregion Local Utility Functions
}