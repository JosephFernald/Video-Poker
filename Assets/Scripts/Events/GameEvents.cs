/// <summary>
/// Default definition of the partial GameEvents class which will contain the constant strings used in the
/// Advanced C# messenger class.
/// </summary>
public static class GameEvents
{
    /// <summary>
    /// All button events.
    /// </summary>
    public static class Buttons
    {
        public static string Bet_One_Pressed = "Buttons.Bet_One_Pressed";
        public static string Bet_Max_Pressed = "Buttons.Bet_Max_Pressed";
        public static string Deal_Pressed = "Buttons.Deal_Pressed";
        public static string AddMoney_Pressed = "Buttons.AddMoney_Pressed";
        public static string Change_Denom_Pressed = "Buttons.Change_Denom_Pressed";
        public static string Hold_Pressed = "Buttons.Hold_Pressed";
    }

    public static class Bet
    {
        public static string BetAmountChanged = "Bet.MultiplierChanged";
        public static string DenomChanged = "Bet.DenomChanged";
        public static string PopulateBetAmountView = "Bet.PopulateMultiplierView";
        public static string PopulateWinNamesView = "Bet.PopulateWinNamesView";
        public static string UpdateCredits = "Bet.UpdateCredits";
        public static string UpdatePaidCredits = "Bet.UpdatePaidCredits";
    }

    public static class Cards
    {
        public static string RevealCard = "Cards.RevealCard";
        public static string ResetCard = "Cards.ResetCard";
        public static string ResetHold = "Cards.ResetHold";
        public static string EnableHoldButtons = "Cards.EnableHoldButtons";
    }

    public static class Award
    {
        public static string UpdateInfoText = "Award.UpdateInfoText";
        public static string ReportWin = "Award.ReportWin";
        public static string HighlightAwardedValue = "Award.HighlightAwardedValue";
        public static string ResetHighlightAwardedValue = "Award.ResetHighlightAwardedValue";
    }

    public static class Sounds
    {
        public static string PlayBetSound = "Sounds.PlayBetSound";
        public static string PlayCardDealSound = "Sounds.PlayCardDealSound";
    }
}