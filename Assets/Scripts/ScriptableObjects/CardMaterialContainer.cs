using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object that contains references to all card materials of a particular suit.
/// </summary>
[CreateAssetMenu(fileName = "CardMaterialContainer", menuName = "Poker/CardMaterialContainer", order = 1)]
public class CardMaterialContainer : ScriptableObject
{
    #region Inspector Variables

    [SerializeField]
    private StandardCardDeck.DeckConstants.CardSuit suit = StandardCardDeck.DeckConstants.CardSuit.Club;

    // In the same order as listed in StandardCardDeck.DeckConstants.CardValue
    [SerializeField]
    private List<Material> cardMaterialList = null;

    #endregion Inspector Variables

    #region Class Functions.

    /// <summary>
    /// Unity can't serialize properties and I want to restrict set access to only the editor.
    /// Returns the suit set in the scriptable object.
    /// </summary>
    public StandardCardDeck.DeckConstants.CardSuit Suit
    {
        get { return suit; }
        private set { value = suit; }
    }

    /// <summary>
    /// Unity can't serialize properties and I want to restrict set access to only the editor.
    /// Returns the card material list set in the scriptable object.
    /// </summary>
    public List<Material> CardMaterialList
    {
        get { return cardMaterialList; }
        private set { value = cardMaterialList; }
    }

    #endregion Class Functions.
}