using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class InfoTextListener : MonoBehaviour
{
    #region Class Variables

    private TextMeshProUGUI infoTextTextMesh = null;

    #endregion Class Variables

    #region Monobehavior Functions

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        infoTextTextMesh = GetComponent<TextMeshProUGUI>();

        Messenger.AddListener<string>(GameEvents.Award.UpdateInfoText, HandleUpdateInfoText);
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<string>(GameEvents.Award.UpdateInfoText, HandleUpdateInfoText);
    }

    #endregion Monobehavior Functions

    #region Event Handlers

    /// <summary>
    /// Handles the UpdateInfoText message broadcast.
    /// </summary>
    /// <param name="newInfoText"></param>
    /// <returns></returns>
    private void HandleUpdateInfoText(string newInfoText)
    {
        infoTextTextMesh.SetText(newInfoText);
    }

    #endregion Event Handlers
}