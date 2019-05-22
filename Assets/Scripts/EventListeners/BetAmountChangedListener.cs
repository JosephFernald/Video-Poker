using UnityEngine;
using UnityEngine.UI;

public class BetAmountChangedListener : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private int betAmountFilter = 0;

    [SerializeField]
    private Image betAmountHighlightedImage = null;


    #endregion Inspector Variables

    #region Monobehavior Functions.

    /// <summary>
    /// Init class.
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        Messenger.AddListener<int>(GameEvents.Bet.BetAmountChanged, HandleBetAmountChangedMessage);
    }

    /// <summary>
    /// Handle cleanup.
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<int>(GameEvents.Bet.BetAmountChanged, HandleBetAmountChangedMessage);
    }

    #endregion Monobehavior Functions.

    #region Event Listeners

    /// <summary>
    /// Handles the Bet Amount Changed event broadcast.
    /// </summary>
    /// <param name="betAmountIndex"></param>
    /// <returns></returns>
    private void HandleBetAmountChangedMessage(int betAmountIndex)
    {
        // If this message was not meant for this instance, disable
        // the image and return.
        if (betAmountIndex != betAmountFilter)
        {
            betAmountHighlightedImage.enabled = false;
            return;
        }

        betAmountHighlightedImage.enabled = true;
    }

    #endregion Event Listeners
}