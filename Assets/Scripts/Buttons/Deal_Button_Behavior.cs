using UnityEngine;

public class Deal_Button_Behavior : MonoBehaviour
{
    /// <summary>
    /// Sends a message to any registered listeners notifying them that the deal/draw button has been pressed
    /// </summary>
    /// <returns></returns>
    public void HandleButtonPressed()
    {
        Messenger.Broadcast(GameEvents.Buttons.Deal_Pressed);
    }
}