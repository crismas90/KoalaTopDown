using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{    
    public string[] textToSay;
    int dialogeNumber;
    public bool textDone;


    public void Speak()
    {
        if (!textDone)
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
                dialogeNumber = 1;
            }
        }
        else
        {
            ChatBubble.Clear(gameObject);
            ChatBubble.Create(transform, new Vector3(0f, 0f), "Всё, молодец, иди дальше");
        }  
    }

    public void TextDone()
    {
        textDone = true;
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
