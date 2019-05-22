using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDenomBehavior : MonoBehaviour
{
    /// <summary>
    /// Sends a message to any registered listeners notifying them that the bet one button has been pressed.
    /// </summary>
    /// <returns></returns>
    public void HandleChangeDenomButtonPressed(int valueToIncrement)
    {
        Messenger.Broadcast<int>(GameEvents.Buttons.Change_Denom_Pressed, valueToIncrement);
    }
}
