using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BetUpdateListener : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private string betMeterPrefix = string.Empty;

    #endregion Inspector Variables

    #region Class Variables

    private TextMeshProUGUI betMeterText = null;

    #endregion Class Variables

    #region Monobehavior Functions

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        betMeterText = GetComponent<TextMeshProUGUI>();
        Messenger.AddListener<int>(GameEvents.Bet.BetAmountChanged, HandleUpdateBet);
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<int>(GameEvents.Bet.BetAmountChanged, HandleUpdateBet);
    }

    #endregion Monobehavior Functions

    #region Event Handlers

    /// <summary>
    /// Handles the MultiplierChanged message broadcast.
    /// </summary>
    /// <param name="multiplierValue"></param>
    /// <returns></returns>
    private void HandleUpdateBet(int multiplierValue)
    {
        betMeterText.SetText(String.Format("{0}{1}", betMeterPrefix, multiplierValue));
    }

    #endregion Event Handlers
}