using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BetAmountViewListener : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private int betAmountFilter = 0;

    [SerializeField]
    private List<TextMeshProUGUI> listOfTextElementsToPopulate = null;

    [SerializeField]
    private Color winHighlightColor = Color.white;

    #endregion Inspector Variables

    #region Class Variables

    private Dictionary<StandardCardDeck.DeckConstants.PokerHandType, int> enumToArrayIndexMap = null;
    private int highlightedAwardIndex = GameUtils.Constants.INVALID_AWARD_INDEX;
    private Color initialFontColor = Color.yellow;

    #endregion Class Variables

    #region Monobehavior Functions.

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        enumToArrayIndexMap = new Dictionary<StandardCardDeck.DeckConstants.PokerHandType, int>();
        Messenger.AddListener<int, List<Award>>(GameEvents.Bet.PopulateBetAmountView, HandlePopulateBetAmountView);
        Messenger.AddListener<int, StandardCardDeck.DeckConstants.PokerHandType>(GameEvents.Award.HighlightAwardedValue, HandleHighlightAwardedValue);
        Messenger.AddListener(GameEvents.Award.ResetHighlightAwardedValue, HandleResetHighlightAwardedValue);
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<int, List<Award>>(GameEvents.Bet.PopulateBetAmountView, HandlePopulateBetAmountView);
        Messenger.RemoveListener<int, StandardCardDeck.DeckConstants.PokerHandType>(GameEvents.Award.HighlightAwardedValue, HandleHighlightAwardedValue);
        Messenger.RemoveListener(GameEvents.Award.ResetHighlightAwardedValue, HandleResetHighlightAwardedValue);
        enumToArrayIndexMap.Clear();
    }

    #endregion Monobehavior Functions.

    #region Event Handlers

    /// <summary>
    /// Handle the Populate multiplier view message broadcast.
    /// </summary>
    /// <param name="multiplierIndex"></param>
    /// <param name="awards"></param>
    /// <returns></returns>
    private void HandlePopulateBetAmountView(int betAmountIndex, List<Award> awards)
    {
        // If this message is not for us, return.
        if (betAmountIndex != betAmountFilter)
        {
            return;
        }

        // This means there aren't enough awards, or enough elements in the list of text elements to populate to fit all the data, which is most likely a unity editor user error.
        Debug.Assert(awards.Count == listOfTextElementsToPopulate.Count, "PopulateMultiplierViewListener: The awards size does not match the size of the list of text elements to populate.");

        // Sets up a dictionary for later use in highlighting awards.
        enumToArrayIndexMap.Clear();

        // If there aren't enough awards, or not enough text elements to populate everything,
        // populate what it is possible to populate and then exit.
        for (int i = 0; i < awards.Count && i < listOfTextElementsToPopulate.Count; ++i)
        {
            enumToArrayIndexMap.Add(awards[i].PayType, i);
            listOfTextElementsToPopulate[i].SetText(String.Format("{0:0.##}", awards[i].BaseValue * betAmountIndex));
        }
    }

    /// <summary>
    /// Message handler used to reset the highlight awarded value on the top screen.
    /// </summary>
    /// <returns></returns>
    private void HandleHighlightAwardedValue(int betAmountIndex, StandardCardDeck.DeckConstants.PokerHandType payType)
    {
        // If this message is not for us, return.
        if (betAmountIndex != betAmountFilter)
        {
            return;
        }

        highlightedAwardIndex = GameUtils.Constants.INVALID_AWARD_INDEX;
        if (enumToArrayIndexMap.TryGetValue(payType, out highlightedAwardIndex))
        {
            initialFontColor = listOfTextElementsToPopulate[highlightedAwardIndex].color;
            listOfTextElementsToPopulate[highlightedAwardIndex].color = winHighlightColor;
        }
    }

    private void HandleResetHighlightAwardedValue()
    {
        if (highlightedAwardIndex != GameUtils.Constants.INVALID_AWARD_INDEX)
        {
            listOfTextElementsToPopulate[highlightedAwardIndex].color = initialFontColor;
            highlightedAwardIndex = GameUtils.Constants.INVALID_AWARD_INDEX;
        }
    }

    #endregion Event Handlers
}