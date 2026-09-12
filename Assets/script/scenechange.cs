using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public GameObject targetObject; 
    public string tagName; 
    public string sceneName; 

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject == targetObject || 
            collision.gameObject.tag == tagName)
        {
            SceneManager.LoadScene(sceneName); 
        }
    }
}
