using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleHoldImage : MonoBehaviour
{
    #region Inspector Variabless

    [SerializeField]
    private GameObject HoldImage = null;

    #endregion Inspector Variabless

    #region Class Variables

    // Cached button component.
    private Button holdButton = null;

    #endregion Class Variables

    #region Monobehavior Functions

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        holdButton = GetComponent<Button>();
        Messenger.AddListener(GameEvents.Cards.ResetHold, HandleResetHoldMessage);
        Messenger.AddListener<bool>(GameEvents.Cards.EnableHoldButtons, HandleEnableHoldButtonsMessage);
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        holdButton = null;
        Messenger.RemoveListener(GameEvents.Cards.ResetHold, HandleResetHoldMessage);
        Messenger.RemoveListener<bool>(GameEvents.Cards.EnableHoldButtons, HandleEnableHoldButtonsMessage);
    }

    #endregion Monobehavior Functions

    #region Event Handlers

    /// <summary>
    ///  Handles the ResetHold Message Broadcast.
    /// </summary>
    /// <returns></returns>
    private void HandleResetHoldMessage()
    {
        HoldImage.SetActive(false);
    }

    /// <summary>
    /// Handles the EnableHoldButtons
    /// </summary>
    /// <param name="enableHoldButtons"></param>
    /// <returns></returns>
    private void HandleEnableHoldButtonsMessage(bool enableHoldButtons)
    {
        holdButton.enabled = enableHoldButtons;
    }

    #endregion Event Handlers

    /// <summary>
    /// Handles toggling the hold image on/off on button press.
    /// </summary>
    /// <returns></returns>
    public void HandleToggleHoldImage()
    {
        HoldImage.SetActive(!HoldImage.activeSelf);
    }
}