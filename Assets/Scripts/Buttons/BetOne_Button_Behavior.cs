using UnityEngine;

public class BetOne_Button_Behavior : MonoBehaviour
{
    /// <summary>
    /// Sends a message to any registered listeners notifying them that the bet one button has been pressed.
    /// </summary>
    /// <returns></returns>
    public void HandleButtonPressed()
    {
        Messenger.Broadcast(GameEvents.Buttons.Bet_One_Pressed);
    }
}