using Unity.VisualScripting;
using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject player;
    public Vector2 offset;
    public float xlimit = 0;
    public bool followHOnly = true;
    public bool smoothFollow = false;
    public float smoothSpeed = 4f;

    void LateUpdate()
    {
        if (player == null) return;
        Vector3 playerPos = player.transform.position;
        Vector3 desiredPos;
        float vx = playerPos.x + offset.x;
        float vz = transform.position.z;
        if (vx < xlimit) vx = xlimit;
        if (followHOnly)
        {
            desiredPos = new Vector3(vx, transform.position.y, vz);
        }
        else
        {
            desiredPos = new Vector3(vx, playerPos.y + offset.y, vz);
        }
        if (smoothFollow)
        {
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = desiredPos;
        }
    }
}
