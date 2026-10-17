
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SeedManager : MonoBehaviour
{
    public static SeedManager Instance;

    public int seedCount = 0;
    public TMP_Text seedText;

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
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        seedText = null;

        TMP_Text[] texts = FindObjectsByType<TMP_Text>(
            FindObjectsSortMode.None
        );

        foreach (TMP_Text text in texts)
        {
            if (text.name == "SeedText")
            {
                seedText = text;
                break;
            }
        }

        UpdateSeedText();
    }

    void Start()
    {
        UpdateSeedText();
    }

    public void AddSeed(int amount)
    {
        seedCount += amount;
        UpdateSeedText();

        Debug.Log("Seed獲得 !  現在：" + seedCount);
    }

    public bool SpendSeed(int amount)
    {
        if (seedCount < amount)
        {
            Debug.Log("Seedが足りない ! ");
            return false;
        }

        seedCount -= amount;
        UpdateSeedText();

        Debug.Log("Seed使用 !  残り：" + seedCount);
        return true;
    }

    void UpdateSeedText()
    {
        if (seedText != null)
        {
            seedText.text = "Seed : " + seedCount;
        }
    }
}
