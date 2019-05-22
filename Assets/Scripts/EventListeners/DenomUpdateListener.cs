using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DenomUpdateListener : MonoBehaviour
{
    #region Class Variables

    private TextMeshProUGUI denomMeterTextMesh = null;

    #endregion Class Variables

    #region Monobehavior Functions

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        denomMeterTextMesh = GetComponent<TextMeshProUGUI>();
        Messenger.AddListener<double>(GameEvents.Bet.DenomChanged, HandleDenomChangedMessage);
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<double>(GameEvents.Bet.DenomChanged, HandleDenomChangedMessage);
    }

    #endregion Monobehavior Functions

    #region Event Handlers

    /// <summary>
    /// Handles the DenomChanged message broadcast.
    /// </summary>
    /// <param name="meterValue"></param>
    /// <returns></returns>
    private void HandleDenomChangedMessage(double meterValue)
    {
        denomMeterTextMesh.SetText(String.Format("{0}{1:0.00}", "$", meterValue));
    }

    #endregion Event Handlers
}