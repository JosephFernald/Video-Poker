using UnityEngine;

public class HoldButtonController : MonoBehaviour
{
    /// <summary>
    /// Sends a message to any registered listeners notifying them that the add money button has been pressed.
    /// </summary>
    /// <returns></returns>
    public void HandleHoldButtonPressed(int buttonIndex)
    {
        Messenger.Broadcast<int>(GameEvents.Buttons.Hold_Pressed, buttonIndex);
    }
}