using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinNamesListener : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private List<TextMeshProUGUI> winNameTextMeshes = null;

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
        Messenger.AddListener<List<string>>(GameEvents.Bet.PopulateWinNamesView, HandlePopulateWinNameView);
        Messenger.AddListener<int, StandardCardDeck.DeckConstants.PokerHandType>(GameEvents.Award.HighlightAwardedValue, HandleHighlightAwardedValue);
        Messenger.AddListener(GameEvents.Award.ResetHighlightAwardedValue, HandleResetHighlightAwardedValue);
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<List<string>>(GameEvents.Bet.PopulateWinNamesView, HandlePopulateWinNameView);
        Messenger.RemoveListener<int, List<Award>>(GameEvents.Bet.PopulateBetAmountView, HandlePopulateBetAmountView);
        Messenger.RemoveListener<int, StandardCardDeck.DeckConstants.PokerHandType>(GameEvents.Award.HighlightAwardedValue, HandleHighlightAwardedValue);
        Messenger.RemoveListener(GameEvents.Award.ResetHighlightAwardedValue, HandleResetHighlightAwardedValue);

        enumToArrayIndexMap.Clear();
    }

    #endregion Monobehavior Functions.

    #region Event Handlers

    /// <summary>
    /// Handles the PopulateWinNamesView Message broadcast.
    /// </summary>
    /// <param name="winNames"></param>
    /// <returns></returns>
    private void HandlePopulateWinNameView(List<string> winNames)
    {
        if (winNameTextMeshes != null)
        {
            // This means there aren't enough names, or enough textmeshes in the list of text elements to populate to fit all the data, which is most likely a unity editor user error.
            Debug.Assert(winNames.Count == winNameTextMeshes.Count, "PopulateWinNamesListener: The winNames size does not match the size of the winNameTextMeshes to populate.");

            for (int i = 0; i < winNameTextMeshes.Count && i < winNames.Count; ++i)
            {
                winNameTextMeshes[i].SetText(winNames[i]);
            }
        }
    }

    private void HandlePopulateBetAmountView(int betAmountIndex, List<Award> awards)
    {
        // only do this for the first bet index.
        if (betAmountIndex != 1)
        {
            return;
        }

        // Sets up a dictionary for later use in highlighting awards.
        enumToArrayIndexMap.Clear();

        // If there aren't enough awards, or not enough text elements to populate everything,
        // populate what it is possible to populate and then exit.
        for (int i = 0; i < awards.Count; ++i)
        {
            enumToArrayIndexMap.Add(awards[i].PayType, i);
        }
    }

    /// <summary>
    /// Message handler used to highlight the awarded value on the top screen.
    /// </summary>
    /// <param name="betAmountIndex"></param>
    /// <param name="payType"></param>
    /// <returns></returns>
    private void HandleHighlightAwardedValue(int betAmountIndex, StandardCardDeck.DeckConstants.PokerHandType payType)
    {
        highlightedAwardIndex = GameUtils.Constants.INVALID_AWARD_INDEX;
        if (enumToArrayIndexMap.TryGetValue(payType, out highlightedAwardIndex))
        {
            initialFontColor = winNameTextMeshes[highlightedAwardIndex].color;
            winNameTextMeshes[highlightedAwardIndex].color = winHighlightColor;
        }
    }

    /// <summary>
    /// Message handler used to reset the highlight awarded value on the top screen.
    /// </summary>
    /// <returns></returns>
    private void HandleResetHighlightAwardedValue()
    {
        if (highlightedAwardIndex != GameUtils.Constants.INVALID_AWARD_INDEX)
        {
            winNameTextMeshes[highlightedAwardIndex].color = initialFontColor;
            highlightedAwardIndex = GameUtils.Constants.INVALID_AWARD_INDEX;
        }
    }

    #endregion Event Handlers
}