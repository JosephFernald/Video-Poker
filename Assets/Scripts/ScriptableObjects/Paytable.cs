using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Award
{
    #region Inspector Variables

    [SerializeField]
    private StandardCardDeck.DeckConstants.PokerHandType payType = StandardCardDeck.DeckConstants.PokerHandType.Unspecified;

    [SerializeField]
    private double baseValue = 0.0;

    #endregion Inspector Variables

    #region Class Variables

    /// <summary>
    /// Unity can't serialize properties and I want to restrict set access to only the editor.
    /// Returns the payType set in the scriptable object.
    /// </summary>
    public StandardCardDeck.DeckConstants.PokerHandType PayType
    {
        get { return payType; }
        private set { value = payType; }
    }

    /// <summary>
    /// Unity can't serialize properties and I want to restrict set access to only the editor.
    /// Returns the baseValue set in the scriptable object.
    /// </summary>
    public double BaseValue
    {
        get { return baseValue; }
        private set { value = baseValue; }
    }

    #endregion Class Variables
}

[CreateAssetMenu(fileName = "AwardList", menuName = "Poker/Math/AwardList", order = 1)]
public class Paytable : ScriptableObject
{
    #region Inspector Variables

    [SerializeField]
    public List<Award> paytableAwards;

    #endregion Inspector Variables

    #region Class Functions.

    /// <summary>
    /// Unity can't serialize properties and I want to restrict set access to only the editor.
    /// Returns the paytableAwards set in the scriptable object.
    /// </summary>
    public List<Award> PaytableAwards
    {
        get { return paytableAwards; }
        private set { value = paytableAwards; }
    }

    /// <summary>
    /// Utility function used to get the base amount won based off the hand.
    /// </summary>
    /// <param name="handType"></param>
    /// <returns></returns>
    public double GetCreditWinAmount(StandardCardDeck.DeckConstants.PokerHandType handType)
    {
        for (int i = 0; i < paytableAwards.Count; ++i)
        {
            if (paytableAwards[i].PayType == handType)
            {
                return paytableAwards[i].BaseValue;
            }
        }

        return 0.0;
    }

    #endregion Class Functions.
}