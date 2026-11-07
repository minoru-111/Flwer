using UnityEngine;

public class Wind : MonoBehaviour
{
    public enum WindDirection
    {
        Left,
        Right
    }

    [Header("風の向き")]
    public WindDirection direction = WindDirection.Left;

    [Header("風の強さ")]
    public float windPower = 2f;

    [Header("移動速度への影響")]
    [Range(0f, 1f)]
    public float speedMultiplier = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        walk player = other.GetComponent<walk>();

        if (player == null)
            return;

        ApplyWind(player);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        walk player = other.GetComponent<walk>();

        if (player == null)
            return;

        ApplyWind(player);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        walk player = other.GetComponent<walk>();

        if (player == null)
            return;

        player.ClearWind();
    }

    private void ApplyWind(walk player)
    {
        float directionValue = 0f;

        if (direction == WindDirection.Left)
            directionValue = -1f;

        if (direction == WindDirection.Right)
            directionValue = 1f;

        Vector2 force =
            new Vector2(
                directionValue * windPower,
                0f
            );

        player.SetWind(
            force,
            speedMultiplier
        );
    }
}