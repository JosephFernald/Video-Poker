using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioListener : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private AudioClip betButtonSound = null;

    [SerializeField]
    private AudioClip cardDealSound = null;

    #endregion Inspector Variables

    #region Class Variables

    private AudioSource audioSource = null;

    #endregion Class Variables

    #region Monobehavior Functions

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Messenger.AddListener(GameEvents.Sounds.PlayBetSound, HandlePlayBetSound);
        Messenger.AddListener(GameEvents.Sounds.PlayCardDealSound, HandlePlayCardDealSound);
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvents.Sounds.PlayBetSound, HandlePlayBetSound);
        Messenger.RemoveListener(GameEvents.Sounds.PlayCardDealSound, HandlePlayCardDealSound);
    }

    #endregion Monobehavior Functions

    #region Event Handlers

    /// <summary>
    /// Handles the Sounds.PlayBetSound message broadcast.
    /// </summary>
    /// <returns></returns>
    private void HandlePlayBetSound()
    {
        audioSource.PlayOneShot(betButtonSound);
    }

    /// <summary>
    /// Handles the Sounds.PlayCardDealSound message broadcast.
    /// </summary>
    /// <returns></returns>
    private void HandlePlayCardDealSound()
    {
        audioSource.PlayOneShot(cardDealSound);
    }

    #endregion Event Handlers
}