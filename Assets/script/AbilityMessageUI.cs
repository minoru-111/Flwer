
using System.Collections;
using UnityEngine;
using TMPro;

public class AbilityMessageUI : MonoBehaviour
{
    public GameObject background;
    public TMP_Text messageText;
    public float displayTime = 3f;

    private Coroutine currentMessage;

    void Start()
    {
        HideMessage();
    }

    public void ShowMessage(string message)
    {
        if (currentMessage != null)
        {
            StopCoroutine(currentMessage);
        }

        currentMessage = StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        messageText.text = message;
        background.SetActive(true);
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        HideMessage();
        currentMessage = null;
    }

    void HideMessage()
    {
        if (background != null)
            background.SetActive(false);

        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }
}
