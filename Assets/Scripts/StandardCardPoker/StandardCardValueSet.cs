using System.Collections.Generic;

/// <summary>
/// Class used to store the indices of cards in a hand that have the same value
/// in order to determine what poker hand could have been awarded.
/// </summary>
public class StandardCardValueSet
{
    #region Properties

    public List<int> IndexList
    {
        get;
        private set;
    }

    #endregion Properties

    #region Class Variables

    public StandardCardValueSet(List<int> indexList)
    {
        IndexList = indexList;
    }

    #endregion Class Variables
}