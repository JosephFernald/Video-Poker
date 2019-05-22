using UnityEngine;

public class AddMoneyButtonBehavior : MonoBehaviour
{
    /// <summary>
    /// Sends a message to any registered listeners notifying them that the add money button has been pressed.
    /// </summary>
    /// <returns></returns>
    public void HandleButtonPressed()
    {
        Messenger.Broadcast(GameEvents.Buttons.AddMoney_Pressed);
    }
}