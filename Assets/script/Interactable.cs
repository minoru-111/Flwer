
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject interactMark;

    private bool playerNearby = false;

    void Start()
    {
        interactMark.SetActive(false);
    }

    void Update()
    {
        if (playerNearby)
        {
            interactMark.SetActive(true);
        }
        else
        {
            interactMark.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
