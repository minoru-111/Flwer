
using UnityEngine;

public class SeedCollectible : MonoBehaviour
{
    public int amount = 1;

    private bool collected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        if (other.CompareTag("Player"))
        {
            if (SeedManager.Instance != null)
            {
                collected = true;

                SeedManager.Instance.AddSeed(amount);

                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("SeedManagerが見つからない！");
            }
        }
    }
}
