using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class NotificationMessagePanel : MonoBehaviour
{
    [SerializeField] GameObject LogMessagePrefab;
    public bool Silent { get; private set; } = false;

    static NotificationMessagePanel _Instance;
    public static NotificationMessagePanel Instance
    {
        get
        {
            if (_Instance == null)
            { _Instance = FindObjectOfType<NotificationMessagePanel>(); }
            return _Instance;
        }
        private set { _Instance = value; }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void InstatiateMessage(string message)
    {
        if (Silent)
        { return; }

        message = UpperToSpaces(message); // rewrites the message based on previous example 'HelloWorld_r' -> 'hello world'
        var go = Instantiate(LogMessagePrefab, parent: transform);
        go.GetComponent<Message>().Initialise(message);
    }

    string UpperToSpaces(string input)
    {
        StringBuilder sb = new StringBuilder();

        bool doDelete = false;
        foreach (char c in input)
        {
            if (char.IsWhiteSpace(c))
            { sb.Append(' '); doDelete = false; }
            else if (doDelete)
            { }
            else if (c == '_')
            { doDelete = true; }
            else if (char.IsUpper(c))
            {
                sb.Append(' ');
                sb.Append(char.ToLower(c));
            }
            else
            { sb.Append(c); }
        }
        return sb.ToString();
    }

    public void Mute(bool setSilent)
    {
        Silent = setSilent;

        if (Silent)
            Clear();
    }

    public void Clear()
    {
        foreach (var t in GetComponents<Transform>())
        {
            if (t != transform)
                Destroy(t);
        }
    }
}
