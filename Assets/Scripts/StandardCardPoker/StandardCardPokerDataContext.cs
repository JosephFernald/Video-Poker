using System.Collections.Generic;

/// <summary>
/// Class used to store data about a standard card game of poker
/// this data is then passed around to various functions for evaluation
/// in order to determine what if anything was awarded.
/// </summary>
public class StandardCardPokerDataContext
{
    #region Properties

    public bool AllSameSuit
    {
        get;
        private set;
    }

    public bool Straight
    {
        get;
        private set;
    }

    public bool AllRoyals
    {
        get;
        private set;
    }

    public List<StandardCardValueSet> SameCardSets
    {
        get;
        private set;
    }

    #endregion Properties

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="_allSameSuit"></param>
    /// <param name="_straight"></param>
    /// <param name="_allRoyals"></param>
    /// <param name="_sameCardSets"></param>
    /// <returns></returns>
    public StandardCardPokerDataContext(bool allSameSuit, bool straight, bool allRoyals, List<StandardCardValueSet> sameCardSets)
    {
        AllSameSuit = allSameSuit;
        Straight = straight;
        AllRoyals = allRoyals;
        SameCardSets = sameCardSets;
    }
}