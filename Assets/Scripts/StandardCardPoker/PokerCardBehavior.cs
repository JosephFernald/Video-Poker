using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PokerCardBehavior : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private int cardIndexFilter = 0;

    [SerializeField]
    private float revealStaggerTime = .01f;

    [SerializeField]
    private float afterResetDelay = .01f;

    [SerializeField]
    private float rotationTime = 0.25f;

    #endregion Inspector Variables

    #region Class Variables


    private MeshRenderer cachedMeshRenderer = null;

    private const int FRONT_MATERIAL_INDEX = 0;
    private const int BACK_MATERIAL_INDEX = 1;

    private WaitForSeconds delayTimeBeforeCardReveal = null;
    private WaitForSeconds delayTimeAfterCardReset = null;

    private Quaternion revealTargetLocalRotation = Quaternion.Euler(0.0f, 0.0f, -180.0f);
    private Quaternion unrevealedTargetLocalRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

    MaterialPropertyBlock facePropertyBlock = null;

    #endregion Class Variables

    #region Monobehavior Functions

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        // Cache the front and back card materials for later use.
        cachedMeshRenderer = GetComponent<MeshRenderer>();

        Messenger.AddListener<int, Material>(GameEvents.Cards.RevealCard, HandleRevealCardMessage);

        delayTimeBeforeCardReveal = new WaitForSeconds(revealStaggerTime);
        delayTimeAfterCardReset = new WaitForSeconds(afterResetDelay);

        facePropertyBlock = new MaterialPropertyBlock();
    }

    /// <summary>
    /// Cleanup.
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<int, Material>(GameEvents.Cards.RevealCard, HandleRevealCardMessage);
    }

    #endregion Monobehavior Functions

    #region Class Functions

    /// <summary>
    /// Coroutine that is triggered when a card reveals.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RevealCardCoroutine()
    {
        if (gameObject.transform.localRotation.Equals(unrevealedTargetLocalRotation) == false)
        {
            gameObject.transform.localRotation = unrevealedTargetLocalRotation;
            yield return delayTimeAfterCardReset;
        }

        if (revealStaggerTime > 0.0f)
        {
            yield return delayTimeBeforeCardReveal;
        }

        float elapsedTime = 0.0f;
        while (elapsedTime < rotationTime)
        {
            gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, revealTargetLocalRotation, elapsedTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // It's possible that we haven't reached the end rotation by the time the while loop exits, so make sure the target rotation is set.
        gameObject.transform.localRotation = revealTargetLocalRotation;

        yield return null;
    }

    #endregion Class Functions

    #region Event Handlers

    /// <summary>
    /// Handles the Reveal Card broadcast message.
    /// </summary>
    /// <param name="cardIndex"></param>
    /// <param name="revealCardMaterial"></param>
    /// <returns></returns>
    public void HandleRevealCardMessage(int cardIndex, Material revealCardMaterial)
    {
        if (cardIndexFilter != cardIndex)
        {
            return;
        }

        // Get the current value of the material properties in the renderer.
        cachedMeshRenderer.GetPropertyBlock(facePropertyBlock);

        facePropertyBlock.SetTexture("_MainTex", revealCardMaterial.GetTexture("_MainTex"));

        // Apply the edited values to the renderer.
        cachedMeshRenderer.SetPropertyBlock(facePropertyBlock, FRONT_MATERIAL_INDEX);

        StartCoroutine(RevealCardCoroutine());
    }

    #endregion Event Handlers
}