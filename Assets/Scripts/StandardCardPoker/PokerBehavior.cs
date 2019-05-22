using StandardCardDeck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerBehavior : MonoBehaviour
{
    private const string GAME_OVER_TEXT = "GAME OVER";

    #region Inspector Variables

    [SerializeField]
    private ScriptableObject activeStrategyObject = null;

    [SerializeField, Tooltip("Dependant on enum order")]
    private List<CardMaterialContainer> cardMaterials = null;

    #endregion Inspector Variables

    #region Class Variables

    private System.Random RNG;
    public List<bool> cardsToHold = null;
    public List<DeckConstants.StandardCard> deckList;
    public List<DeckConstants.StandardCard> hand = new List<DeckConstants.StandardCard>();
    public Stack<DeckConstants.StandardCard> deckStack;
    private int currentRound = 0;

    private WaitForSeconds waitForDelay = new WaitForSeconds(0.5f);

    #endregion Class Variables

    #region Properties

    public bool IsPokerGameInPlay
    {
        get;
        private set;
    }

    public bool IsWaitingForPlayerInput
    {
        get;
        private set;
    }

    public IStandardCardPokerStrategy ActiveStrategy
    {
        get;
        private set;
    }

    #endregion Properties

    #region Monobehavior Functions

    /// <summary>
    /// Init game.
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        // Sanity checks.
        Debug.Assert(cardMaterials != null && cardMaterials.Count > 0, "cardMaterials must contain at least 1 group of materials");
        Debug.Assert(activeStrategyObject != null, "activeStrategyObject must be non null.");

        RNG = new System.Random();
        ActiveStrategy = activeStrategyObject as IStandardCardPokerStrategy;
        cardsToHold = ActiveStrategy.CreateNewCardHoldArray();

        Messenger.AddListener<int>(GameEvents.Buttons.Hold_Pressed, HandleHoldButtonPress);
    }

    /// <summary>
    /// Cleanup.
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener<int>(GameEvents.Buttons.Hold_Pressed, HandleHoldButtonPress);
    }

    #endregion Monobehavior Functions

    #region Event Handlers

    /// <summary>
    /// Utility function, exposed publically so that the Hold Buttons in the unity editor can tie into the callback.
    /// </summary>
    /// <param name="holdButtonIndex"></param>
    /// <returns></returns>
    private void HandleHoldButtonPress(int holdButtonIndex)
    {
        cardsToHold[holdButtonIndex] = !cardsToHold[holdButtonIndex];
    }

    #endregion Event Handlers

    #region Class Functions

    /// <summary>
    /// Utility function used to determine if all cards are on hold.
    /// </summary>
    /// <returns></returns>
    public bool AreAllCardsBeingHeld()
    {
        bool areAllCardsBeingHeld = true;
        for(int i=0;i<cardsToHold.Count && areAllCardsBeingHeld == true;++i)
        {
            if(cardsToHold[i] == false)
            {
                areAllCardsBeingHeld = false;
            }
        }

        return areAllCardsBeingHeld;
    }

    /// <summary>
    /// The function external classes call when they want to begin playing a game.
    /// </summary>
    /// <returns></returns>
    public void PlayPoker()
    {
        if (IsPokerGameInPlay)
        {
            if (IsWaitingForPlayerInput)
            {
                // Do something else.
                IsWaitingForPlayerInput = false;
            }

            return;
        }

        EnableHoldButtons(false);
        IsPokerGameInPlay = true;
        IsWaitingForPlayerInput = false;

        currentRound = 0;
        List<DeckConstants.StandardCard> startingDeck = ActiveStrategy.CreateNewDeck();
        ActiveStrategy.ShuffleDeck(startingDeck, RNG);

        deckStack = new Stack<DeckConstants.StandardCard>(startingDeck);

        Messenger.Broadcast<double>(GameEvents.Bet.UpdatePaidCredits, 0);
        Messenger.Broadcast(GameEvents.Cards.ResetHold);
        Messenger.Broadcast<string>(GameEvents.Award.UpdateInfoText, string.Empty);

        StartCoroutine(PlayPokerCoroutine());
    }

    /// <summary>
    /// Utility function used to configure the game initally on load.
    /// </summary>
    /// <returns></returns>
    public void ConfigureGame()
    {
        List<string> winViewNames;
        ActiveStrategy.GenerateWinViewNamesList(out winViewNames);
        Messenger.Broadcast(GameEvents.Bet.PopulateWinNamesView, winViewNames);

        EnableHoldButtons(false);
    }

    #endregion Class Functions

    #region Coroutines

    /// <summary>
    /// The main play poker coroutine.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayPokerCoroutine()
    {
        while (currentRound < ActiveStrategy.GetHandsPerRound())
        {
            ActiveStrategy.DealCards(hand, cardsToHold, deckStack);

            // Reveal the cards.
            yield return RevealCardsCoroutine();

            // Display info text if this is not the last hand of the game.
            UpdateInfoTextBasedOnHandResults();

            yield return WaitForPlayerDrawConfirmationCoroutine();

            currentRound++;
            yield return null;
        }

        // Delay before all 5 cards finish their reveal.
        yield return waitForDelay;

        DeckConstants.PokerHandType type = ActiveStrategy.DeterminePokerHandType(hand);
        string infoText = ActiveStrategy.GetInfoTextStringForHandType(type);
        if (infoText == string.Empty)
        {
            infoText = GAME_OVER_TEXT;
        }

        Messenger.Broadcast<string>(GameEvents.Award.UpdateInfoText, infoText);
        Messenger.Broadcast<StandardCardDeck.DeckConstants.PokerHandType>(GameEvents.Award.ReportWin, type);

        // Reset the hold buttons for next wager.
        for (int i = 0; i < cardsToHold.Count; ++i)
        {
            cardsToHold[i] = false;
        }

        IsPokerGameInPlay = false;
        yield return null;
    }

    /// <summary>
    /// Coroutine responsible for revealing cards which are not being held.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RevealCardsCoroutine()
    {
        for (int i = 0; i < hand.Count; ++i)
        {
            if (cardsToHold[i] == false)
            {
                int suitIndex = (int)hand[i].suit;
                int arrayIndex = (int)hand[i].value - (int)DeckConstants.CardValue.Two;
                Messenger.Broadcast<int, Material>(GameEvents.Cards.RevealCard, i + 1, cardMaterials[suitIndex].CardMaterialList[arrayIndex]);
            }
        }

        // Delay before all 5 cards finish their reveal.
        yield return waitForDelay;
    }

    /// <summary>
    /// Coroutine that waits for the player to press the Deal/Draw button after the first round of cards being dealt.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForPlayerDrawConfirmationCoroutine()
    {
        EnableHoldButtons(true);

        IsWaitingForPlayerInput = true;
        while (IsWaitingForPlayerInput == true && currentRound < ActiveStrategy.GetHandsPerRound() - 1)
        {
            yield return null;
        }

        EnableHoldButtons(false);
    }

    #endregion Coroutines

    #region Local Utility Functions.

    /// <summary>
    /// Utility function used to enable/disable the hold buttons so that they can not be pressed when they shouldn't be pressed.
    /// </summary>
    /// <param name="shouldEnableHoldButtons"></param>
    /// <returns></returns>
    private void EnableHoldButtons(bool shouldEnableHoldButtons)
    {
        Messenger.Broadcast<bool>(GameEvents.Cards.EnableHoldButtons, shouldEnableHoldButtons);
    }

    /// <summary>
    /// Utility function used to populate the info text correctly based off potential win or win type.
    /// </summary>
    /// <returns></returns>
    private void UpdateInfoTextBasedOnHandResults()
    {
        DeckConstants.PokerHandType type = ActiveStrategy.DeterminePokerHandType(hand);
        string infoText = ActiveStrategy.GetInfoTextStringForHandType(type);

        Messenger.Broadcast<string>(GameEvents.Award.UpdateInfoText, infoText);
    }

    #endregion Local Utility Functions.
}