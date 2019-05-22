using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField]
    private PaytableConfig currentPaytableList = null;

    [SerializeField]
    private PokerBehavior pokerGameBehavior = null;

    [SerializeField, Tooltip("The amount of cash you start the game with.")]
    private double initialPlayerWalletAmount = 100.0;

    [SerializeField, Tooltip("Change this value if you want to add more or less money to your wallet via the Add Money button")]
    private double addMoneyButtonAmount = 10.0;

    #endregion Inspector Variables

    #region Class Variables

    private double playersWalletAmountInDollars = 0.0;
    private double currentDenom = 0.0;
    private int currentBetIndex = 0;
    private int currentDenomIndex = 0;
    private double currentBetAmount = 0.0;

    #endregion Class Variables

    #region Monobehavior Functions

    /// <summary>
    /// Init
    /// </summary>
    /// <returns></returns>
    private void Awake()
    {
        // Sanity checks.
        Debug.Assert(pokerGameBehavior != null, "pokerGameBehavior must be non null.");
        Debug.Assert(currentPaytableList != null, "currentPaytableList must be non null.");
        Debug.Assert(currentPaytableList.AvailableDenoms != null && currentPaytableList.AvailableDenoms.Count > 0, "currentPaytableList must contain at least 1 denom");
        Debug.Assert(currentPaytableList.BetAmounts != null && currentPaytableList.BetAmounts.Count > 0, "currentPaytableList must contain at least 2 bet multipliers");

        currentDenomIndex = GameUtils.GetLastElementInList<double>(currentPaytableList.AvailableDenoms);
        currentDenom = currentPaytableList.AvailableDenoms[currentDenomIndex];
        playersWalletAmountInDollars = initialPlayerWalletAmount;

        Messenger.AddListener(GameEvents.Buttons.AddMoney_Pressed, HandleAddMoneyButtonPressed);
        Messenger.AddListener(GameEvents.Buttons.Bet_One_Pressed, HandleBetOne_ButtonPressed);
        Messenger.AddListener(GameEvents.Buttons.Bet_Max_Pressed, HandleBetMax_ButtonPressed);
        Messenger.AddListener(GameEvents.Buttons.Deal_Pressed, HandleDeal_ButtonPressed);
        Messenger.AddListener<int>(GameEvents.Buttons.Change_Denom_Pressed, HandleChangeDenom);
        Messenger.AddListener<StandardCardDeck.DeckConstants.PokerHandType>(GameEvents.Award.ReportWin, HandleReportWinMessage);
    }

    /// <summary>
    /// Configuration
    /// </summary>
    /// <returns></returns>
    private void Start()
    {
        ConfigureGame();
    }

    /// <summary>
    /// Cleanup.
    /// </summary>
    /// <returns></returns>
    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvents.Buttons.AddMoney_Pressed, HandleAddMoneyButtonPressed);
        Messenger.RemoveListener(GameEvents.Buttons.Bet_One_Pressed, HandleBetOne_ButtonPressed);
        Messenger.RemoveListener(GameEvents.Buttons.Bet_Max_Pressed, HandleBetMax_ButtonPressed);
        Messenger.RemoveListener(GameEvents.Buttons.Deal_Pressed, HandleDeal_ButtonPressed);
        Messenger.RemoveListener<int>(GameEvents.Buttons.Change_Denom_Pressed, HandleChangeDenom);
        Messenger.RemoveListener<StandardCardDeck.DeckConstants.PokerHandType>(GameEvents.Award.ReportWin, HandleReportWinMessage);

        playersWalletAmountInDollars = 0.0;
        currentDenom = 0.0;
        currentDenomIndex = 0;
    }

    #endregion Monobehavior Functions

    #region Event Handlers

    /// <summary>
    /// Handles the change denom pressed message when broadcast.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private void HandleChangeDenom(int value)
    {
        currentDenomIndex = GameUtils.Clamp(currentDenomIndex + value, 0, GameUtils.GetLastElementInList<double>(currentPaytableList.AvailableDenoms));
        currentDenom = currentPaytableList.AvailableDenoms[currentDenomIndex];
        currentBetAmount = currentDenom * (currentBetIndex + 1);
        Messenger.Broadcast<double>(GameEvents.Bet.DenomChanged, currentDenom);
    }

    /// <summary>
    /// Handles the Deal/Draw button pressed broadcast.
    /// </summary>
    /// <returns></returns>
    private void HandleDeal_ButtonPressed()
    {
        if (PlaceBet(currentPaytableList.BetAmounts[currentBetIndex], currentDenom))
        {
            if (pokerGameBehavior.AreAllCardsBeingHeld() == false)
            {
                Messenger.Broadcast(GameEvents.Sounds.PlayCardDealSound);
            }
            
            Messenger.Broadcast(GameEvents.Award.ResetHighlightAwardedValue);
            pokerGameBehavior.PlayPoker();
        }
    }

    /// <summary>
    /// Handles the report win message.
    /// </summary>
    /// <param name="winAmountBaseCredits"></param>
    /// <returns></returns>
    private void HandleReportWinMessage(StandardCardDeck.DeckConstants.PokerHandType winType)
    {
        double winAmountBaseCredits = currentPaytableList.Paytables[currentBetIndex].GetCreditWinAmount(winType);

        playersWalletAmountInDollars += (winAmountBaseCredits * currentDenom);

        Messenger.Broadcast<double>(GameEvents.Bet.UpdatePaidCredits, winAmountBaseCredits);
        Messenger.Broadcast<double, double>(GameEvents.Bet.UpdateCredits, ConvertMoneyToCredits(playersWalletAmountInDollars, currentDenom), currentDenom);

        Messenger.Broadcast<int, StandardCardDeck.DeckConstants.PokerHandType>(GameEvents.Award.HighlightAwardedValue, currentBetIndex+1, winType);
    }

    /// <summary>
    /// Handles the Bet One button pressed broadcast.
    /// </summary>
    /// <returns></returns>
    private void HandleBetOne_ButtonPressed()
    {
        // Handle wrapping from max bet to bet 1 again.
        int nextIndex = currentBetIndex + 1;
        if (nextIndex >= currentPaytableList.BetAmounts.Count)
        {
            nextIndex = 0;
        }

        currentBetIndex = nextIndex;
        if (currentPaytableList.BetAmounts.Count > 0 && currentBetIndex > GameUtils.GetLastElementInList<int>(currentPaytableList.BetAmounts))
        {
            currentBetIndex = 0;
        }

        Messenger.Broadcast(GameEvents.Sounds.PlayBetSound);
        Messenger.Broadcast(GameEvents.Bet.BetAmountChanged, currentPaytableList.BetAmounts[currentBetIndex]);
    }

    /// <summary>
    /// Handles the BetMax button pressed broadcast.
    /// </summary>
    /// <returns></returns>
    private void HandleBetMax_ButtonPressed()
    {
        int lastElementInList = GameUtils.GetLastElementInList<int>(currentPaytableList.BetAmounts);

        // We are already at max bet.
        if (currentBetIndex == lastElementInList)
        {
            return;
        }

        currentBetIndex = lastElementInList;
        Messenger.Broadcast(GameEvents.Sounds.PlayBetSound);
        Messenger.Broadcast(GameEvents.Bet.BetAmountChanged, currentPaytableList.BetAmounts[currentBetIndex]);
    }

    /// <summary>
    /// Handles the add money event.
    /// </summary>
    /// <returns></returns>
    public void HandleAddMoneyButtonPressed()
    {
        HandleCoinIn(addMoneyButtonAmount);
    }

    #endregion Event Handlers

    /// <summary>
    /// Utility function used to configure the game the first time it's started.
    /// </summary>
    /// <returns></returns>
    private void ConfigureGame()
    {
        currentBetIndex = 0;
        Messenger.Broadcast(GameEvents.Bet.BetAmountChanged, currentPaytableList.BetAmounts[currentBetIndex]);
        for (int betAmountIndex = 0; betAmountIndex < currentPaytableList.BetAmounts.Count; ++betAmountIndex)
        {
            Messenger.Broadcast<int, List<Award>>(GameEvents.Bet.PopulateBetAmountView, currentPaytableList.BetAmounts[betAmountIndex], currentPaytableList.Paytables[betAmountIndex].paytableAwards);
        }

        Messenger.Broadcast<double, double>(GameEvents.Bet.UpdateCredits, ConvertMoneyToCredits(playersWalletAmountInDollars, currentDenom), currentDenom);

        // Manually force a denom update the first time we load the game up.
        HandleChangeDenom(0);

        pokerGameBehavior.ConfigureGame();
    }

    /// <summary>
    /// Utility function used to determine if the player has enough money in their wallet to afford the bet.
    /// </summary>
    /// <param name="betAmount"></param>
    /// <param name="denom"></param>
    /// <param name="playerWalletAmount"></param>
    /// <returns></returns>
    private bool CanPlayerAffordBet(double betAmount, double denom, double playerWalletAmount)
    {
        double totalBetAmount = betAmount * denom;
        return (totalBetAmount <= playerWalletAmount && playerWalletAmount > 0.0);
    }

    /// <summary>
    /// Utility function used to convert a money value to credits.
    /// </summary>
    /// <param name="moneyAmount"></param>
    /// <param name="denom"></param>
    /// <returns></returns>
    private double ConvertMoneyToCredits(double moneyAmount, double denom)
    {
        // Prevent divide by zero exception.
        Debug.Assert(denom > 0.0, "denom must be greater than 0.0");
        return playersWalletAmountInDollars / currentDenom;
    }

    /// <summary>
    /// Utility function used to handle adding money to the machine.
    /// </summary>
    /// <param name="valueInDollars"></param>
    /// <returns></returns>
    private void HandleCoinIn(double valueInDollars)
    {
        playersWalletAmountInDollars = GameUtils.Clamp(playersWalletAmountInDollars + valueInDollars, 0.0, double.MaxValue);
        Messenger.Broadcast<double, double>(GameEvents.Bet.UpdateCredits, ConvertMoneyToCredits(playersWalletAmountInDollars, currentDenom), currentDenom);
    }

    /// <summary>
    /// Utility function used to handle placing a wager.
    /// </summary>
    /// <param name="betAmount"></param>
    /// <param name="denom"></param>
    /// <returns></returns>
    private bool PlaceBet(int betAmount, double denom)
    {
        if (!CanPlayerAffordBet(betAmount, denom, playersWalletAmountInDollars))
        {
            return false;
        }

        playersWalletAmountInDollars -= (betAmount * denom);
        Messenger.Broadcast<double, double>(GameEvents.Bet.UpdateCredits, ConvertMoneyToCredits(playersWalletAmountInDollars, currentDenom), currentDenom);
        return true;
    }
}