using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("追従するプレイヤー")]
    public Transform target;

    [Header("プレイヤーからの距離")]
    public Vector3 offset = new Vector3(0f, 20f, -5f);

    void LateUpdate()
    {
        if (target == null) return;

        // 位置だけ追従
        transform.position = target.position + offset;

        // 回転は固定
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
