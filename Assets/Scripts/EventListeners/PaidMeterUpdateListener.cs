using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PaidMeterUpdateListener : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private string paidMeterPrefix = string.Empty;

    #endregion Inspector Variables

    #region Class Variables

    private TextMeshProUGUI paidMeterTextMesh = null;

    #endregion Class Variables

    #region Monobehavior Functions

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        paidMeterTextMesh = GetComponent<TextMeshProUGUI>();
        Messenger.AddListener<double>(GameEvents.Bet.UpdatePaidCredits, HandleUpdatePaidCreditsMessage);
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<double>(GameEvents.Bet.UpdatePaidCredits, HandleUpdatePaidCreditsMessage);
    }

    #endregion Monobehavior Functions

    #region Event Handlers

    /// <summary>
    /// Handles the UpdatePaidCredits message broadcast.
    /// </summary>
    /// <param name="meterValue"></param>
    /// <returns></returns>
    private void HandleUpdatePaidCreditsMessage(double meterValue)
    {
        if (meterValue > 0.0)
        {
            paidMeterTextMesh.SetText(String.Format("{0}{1:0.##}", paidMeterPrefix, meterValue));
        }
        else
        {
            // I'm not sure if this is the same with video poker, but typically it is frowned
            // on to show a 0 win value to the player on a slot game, so I'm going to hide the
            // 0 win value here.
            paidMeterTextMesh.SetText(paidMeterPrefix);
        }
    }

    #endregion Event Handlers
}