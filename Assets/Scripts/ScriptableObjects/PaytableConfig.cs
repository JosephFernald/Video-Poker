using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PaytableConfig", menuName = "Poker/Math/PatytableConfig", order = 1)]
public class PaytableConfig : ScriptableObject
{
    #region Inspector Variables

    [SerializeField]
    private List<double> availableDenoms = null;

    [SerializeField]
    private List<int> betAmounts = null;

    [SerializeField]
    private List<Paytable> paytables = null;

    #endregion Inspector Variables

    #region Class Functions

    /// <summary>
    /// Unity can't serialize properties and I want to restrict set access to only the editor.
    /// Returns the available denoms in the scriptable object.
    /// </summary>
    public List<double> AvailableDenoms
    {
        get { return availableDenoms; }
        private set { value = availableDenoms; }
    }

    /// <summary>
    /// Unity can't serialize properties and I want to restrict set access to only the editor.
    /// Returns the bet betAmounts set in the scriptable object.
    /// </summary>
    public List<int> BetAmounts
    {
        get { return betAmounts; }
        private set { value = betAmounts; }
    }

    /// <summary>
    /// Unity can't serialize properties and I want to restrict set access to only the editor.
    /// Returns the paytables set in the scriptable object.
    /// </summary>
    public List<Paytable> Paytables
    {
        get { return paytables; }
        private set { value = paytables; }
    }

    #endregion Class Functions
}