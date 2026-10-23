
using UnityEngine;

public class AirDashFlower : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (AbilityManager.Instance == null)
        {
            Debug.LogError("AbilityManagerが見つかりません！");
            return;
        }

        // すでに解放済みなら、もう一度表示しない
        if (AbilityManager.Instance.hasAirDash)
            return;

        // 空中ダッシュを解放
        AbilityManager.Instance.UnlockAirDash();

        // 画面いっぱいの能力解放メッセージを表示
        AbilityMessageUI messageUI =
            FindObjectOfType<AbilityMessageUI>();

        if (messageUI != null)
        {
            messageUI.ShowMessage("能力解放！\n空中ダッシュ");
        }
        else
        {
            Debug.LogWarning(
                "AbilityMessageUIが見つかりません！"
            );
        }

        // 花を消す
        Destroy(gameObject);
    }
}
