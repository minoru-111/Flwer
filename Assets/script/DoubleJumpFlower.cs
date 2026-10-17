
using UnityEngine;

public class DoubleJumpFlower : MonoBehaviour
{
    private bool collected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            walk playerWalk = other.GetComponent<walk>();

            if (playerWalk != null)
            {
                // ダブルジャンプを解放
                playerWalk.UnlockDoubleJump();

                // 能力を保存
                if (AbilityManager.Instance != null)
                {
                    AbilityManager.Instance.UnlockDoubleJump();
                }

                collected = true;
                Debug.Log("ダブルジャンプ解放！");
                Destroy(gameObject);
                AbilityMessageUI ui =
    FindFirstObjectByType<AbilityMessageUI>();

if (ui != null)
{
    ui.ShowMessage(
        "能力解放！\nダブルジャンプ"
    );
}
            }
        }
    }
}
