
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class SeedDoor : MonoBehaviour
{
    [Header("設定")]
    public int requiredSeeds = 3;

    [Header("アニメーション")]
    public float moveDistance = 3f;
    public float moveDuration = 1f;

    [Header("UI")]
    public GameObject interactMark;
    public TMP_Text messageText;

    [Header("メッセージ")]
    public string openMessage = "扉が開いた！";
    public string notEnoughMessage = "Seedが足りない！";

    private bool playerNearby = false;
    private bool isOpen = false;

    private Collider2D[] doorColliders;
    private SpriteRenderer doorSprite;

    void Start()
    {
        doorColliders = GetComponents<Collider2D>();
        doorSprite = GetComponent<SpriteRenderer>();

        if (interactMark != null)
            interactMark.SetActive(false);

        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (playerNearby && !isOpen &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenDoor();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HideMessage();
        }
    }

    void OpenDoor()
    {
        if (SeedManager.Instance == null)
        {
            ShowMessage("Seedを使用できない！");
            return;
        }

        if (SeedManager.Instance.SpendSeed(requiredSeeds))
        {
            isOpen = true;

            ShowMessage(openMessage);

            if (interactMark != null)
                interactMark.SetActive(false);

            // 扉の当たり判定を無効にする
            foreach (Collider2D col in doorColliders)
            {
                col.enabled = false;
            }

            // 上に移動してから消す
            StartCoroutine(OpenAnimation());
        }
        else
        {
            ShowMessage(notEnoughMessage);
        }
    }

    IEnumerator OpenAnimation()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition =
            startPosition + Vector3.up * moveDistance;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / moveDuration);

            // 最初はゆっくり、最後はスムーズに移動
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.position =
                Vector3.Lerp(startPosition, endPosition, smoothT);

            yield return null;
        }

        transform.position = endPosition;

        // アニメーション終了後に扉を消す
        Destroy(gameObject);
    }

    void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
        }
    }

    void HideMessage()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (!isOpen && interactMark != null)
                interactMark.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (!isOpen && interactMark != null)
                interactMark.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (interactMark != null)
                interactMark.SetActive(false);

            HideMessage();
        }
    }
}
