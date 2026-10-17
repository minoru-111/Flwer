
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    public GameObject interactMark;
    public TMP_Text messageText;

    [TextArea(2, 5)]
    public string message = "調べた！";

    private HashSet<Collider2D> playerColliders =
        new HashSet<Collider2D>();

    private bool playerNearby = false;

    void Start()
    {
        if (interactMark != null)
            interactMark.SetActive(false);
        else
            Debug.LogWarning("Interact Markが設定されていない ! ");

        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        playerColliders.RemoveWhere(c => c == null);

        bool isNearby = playerColliders.Count > 0;

        if (playerNearby != isNearby)
        {
            playerNearby = isNearby;

            if (interactMark != null)
                interactMark.SetActive(playerNearby);
        }

        if (Keyboard.current == null)
            return;

        // 近くにいるとき、Eキーで調べる
        if (playerNearby &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (messageText != null)
            {
                messageText.text = message;
                messageText.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerColliders.Add(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerColliders.Add(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerColliders.Remove(other);

            if (playerColliders.Count == 0)
            {
                playerNearby = false;

                if (interactMark != null)
                    interactMark.SetActive(false);

                if (messageText != null)
                    messageText.gameObject.SetActive(false);
            }
        }
    }
}
