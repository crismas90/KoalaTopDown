using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{    
    public string[] textToSay;      // текст для диалога
    int dialogeNumber;              // номер диалога
    bool isTextDone;                // проговорили весь текст


    public void Speak()
    {
        if (!isTextDone)
        {
            ChatBubble.Clear(gameObject);
            if (dialogeNumber == 0)
            {
                ChatBubble.Create(transform, new Vector3(0f, 0f), textToSay[0]);
            }
            else
            {
                ChatBubble.Create(transform, new Vector3(0f, 0f), textToSay[dialogeNumber]);
            }

            dialogeNumber++;

            if (dialogeNumber >= textToSay.Length)
            {
                isTextDone = true;
            }
        }
    }

    public void SpeakText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0f, 0f), text);        
    }

/*    void ClearDialoge()
    {
        ChatBubble[] chats = GetComponentsInChildren<ChatBubble>();
        foreach (ChatBubble chat in chats)
        {
            chat.gameObject.SetActive(false);
        }
    }*/
}
