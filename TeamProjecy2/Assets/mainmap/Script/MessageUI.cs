using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public static MessageUI instance;
    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;

    private void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public void ShowMessage (string title,string message)
    {
        panel.SetActive (true);
        nameText.text = title;
        messageText.text = message;
    }

    public void HideMessage()
    {
        panel.SetActive (false);
    }
}
