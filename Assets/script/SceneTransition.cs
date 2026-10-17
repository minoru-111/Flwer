
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName;

    private bool transitioned = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (transitioned) return;

        if (other.CompareTag("Player"))
        {
            transitioned = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
