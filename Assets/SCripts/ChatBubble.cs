using UnityEngine;
using UnityEngine.UI;

public class ChatBubble : MonoBehaviour
{
    public Text messageText;

    public void SetMessage(string message)
    {
        messageText.text = message;
    }
}
