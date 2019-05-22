using UnityEngine;

public class BetMax_Button_Behavior : MonoBehaviour
{
    /// <summary>
    /// Sends a message to any registered listeners notifying them that the max bet button has been pressed.
    /// </summary>
    /// <returns></returns>
    public void HandleButtonPressed()
    {
        Messenger.Broadcast(GameEvents.Buttons.Bet_Max_Pressed);
    }
}