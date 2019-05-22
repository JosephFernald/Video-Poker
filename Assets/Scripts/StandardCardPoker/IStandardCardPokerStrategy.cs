using StandardCardDeck;
using System.Collections.Generic;

/// <summary>
/// Interface class for a standard card poker game.
/// </summary>
public interface IStandardCardPokerStrategy
{
    int GetCardValueMin();

    int GetCardValueMax();

    int GetHandsPerRound();

    void GenerateWinViewNamesList(out List<string> winNames);

    string GetInfoTextStringForHandType(DeckConstants.PokerHandType handType);

    List<DeckConstants.StandardCard> CreateNewDeck();

    List<bool> CreateNewCardHoldArray();

    void ShuffleDeck(List<DeckConstants.StandardCard> deck, System.Random RNG);

    void DealCards(List<DeckConstants.StandardCard> currentHand, List<bool> cardsToHold, Stack<DeckConstants.StandardCard> deckStack);

    DeckConstants.PokerHandType DeterminePokerHandType(List<DeckConstants.StandardCard> pokerHand);
}