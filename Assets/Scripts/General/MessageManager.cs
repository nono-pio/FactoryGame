using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{

    [SerializeField] private GameObject MessagePrefab;
    [SerializeField] private Transform MessagesParent;

    [SerializeField] private int maxMessages = 3;
    [SerializeField] private float timeDispawnMessage = 5f;
    [SerializeField] private float timeDispawnAnimation = 1f;

    private List<Message> waitMessages = new List<Message>();
    private int nbCurMessage = 0;

    [HideInInspector] public static MessageManager instance;
    private void Awake() { instance = this; }

    public void AddMessage(Message message)
    {
        if (nbCurMessage < maxMessages)
            StartCoroutine(AddAndDeleteMessage(message));
        else
            waitMessages.Add(message);
    }

    private IEnumerator AddAndDeleteMessage(Message _message)
    {
        nbCurMessage++;
        GameObject message = Instantiate(MessagePrefab, MessagesParent);
        message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _message.title;
        message.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _message.content;

        yield return new WaitForSeconds(timeDispawnMessage);

        LeanTween.moveX(message, -420, timeDispawnAnimation);

        yield return new WaitForSeconds(timeDispawnAnimation);

        Destroy(message);
        nbCurMessage--;
        if (waitMessages.Count > 0)
            NextMessage();
    }

    private void NextMessage()
    {
        StartCoroutine(AddAndDeleteMessage(waitMessages[0]));
        waitMessages.RemoveAt(0);
    }
}

public class Message
{
    public string title, content;
    public Message(string _title, string _content)
    {
        title = _title;
        content = _content;
    }
}
