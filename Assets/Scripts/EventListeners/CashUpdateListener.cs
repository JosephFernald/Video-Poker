﻿using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CashUpdateListener : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private string cashMeterPrefix = string.Empty;

    #endregion Inspector Variables

    #region Class Variables

    private TextMeshProUGUI creditMeterTextMesh = null;

    #endregion Class Variables

    #region Monobehavior Functions

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        creditMeterTextMesh = GetComponent<TextMeshProUGUI>();
        Messenger.AddListener<double, double>(GameEvents.Bet.UpdateCredits, HandleUpdateCreditsMessage);
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<double, double>(GameEvents.Bet.UpdateCredits, HandleUpdateCreditsMessage);
    }

    #endregion Monobehavior Functions

    #region Event Handlers

    /// <summary>
    /// Handles the UpdateCredits message broadcast.
    /// </summary>
    /// <param name="meterValue"></param>
    /// <param name="denom"></param>
    /// <returns></returns>
    private void HandleUpdateCreditsMessage(double meterValue, double denom)
    {
        double cashValue = meterValue * denom;
        creditMeterTextMesh.SetText(String.Format("{0}{1}{2:0.00}", cashMeterPrefix, "$", meterValue * denom));
    }

    #endregion Event Handlers
}