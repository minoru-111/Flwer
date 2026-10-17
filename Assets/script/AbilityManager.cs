
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    public bool hasDoubleJump = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void UnlockDoubleJump()
    {
        hasDoubleJump = true;
        Debug.Log("ダブルジャンプの能力を保存！");
    }
}
