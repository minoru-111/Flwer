
using UnityEngine;

public class destroy : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == target)
        {
            Destroy(gameObject);
        }
    }
}
