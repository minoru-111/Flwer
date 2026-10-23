
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    public bool hasDoubleJump = false;
    public bool hasAirDash = false;

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

    public void UnlockAirDash()
    {
        hasAirDash = true;
        Debug.Log("空中ダッシュの能力を保存！");
    }
}
