using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameUtils
{
    public static class Constants
    {
        public const int INVALID_AWARD_INDEX = -1;
    }

    /// <summary>
    /// There doesn't seem to be an int clamp function in C#'s Math library even though it's
    /// documented on MSDN.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int Clamp(int value, int min, int max)
    {
        return Math.Min(Math.Max(value, min), max);
    }

    /// <summary>
    /// There doesn't seem to be an double clamp function in C#'s Math library even though it's
    /// documented on MSDN.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double Clamp(double value, double min, double max)
    {
        return Math.Min(Math.Max(value, min), max);
    }

    /// <summary>
    /// Utility function used to get the last element in a list safely.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static int GetLastElementInList<T>(List<T> list)
    {
        Debug.Assert(list.Count > 0, "Can't get the last element in a list with a size of 0.");
        return list.Count - 1;
    }
}


