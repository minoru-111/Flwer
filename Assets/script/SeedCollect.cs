
using UnityEngine;

public class SeedCollect : MonoBehaviour
{
    public GameObject target;
    public AudioClip sound;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.gameObject == target)
        {
            collected = true;

            if (sound != null)
            {
                AudioSource.PlayClipAtPoint(sound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
