using UnityEngine;

public class BlueCarMovement : MonoBehaviour
{
    [Header("左右に移動する距離")]
    [Tooltip("現在位置から右または左へ、どれくらい移動するか")]
    [SerializeField] private float moveDistance = 10f;

    [Header("左右に移動する速さ")]
    [Tooltip("数値を大きくすると、左右移動が速くなる")]
    [SerializeField] private float moveSpeed = 30f;

    // 現在動いている方向
    private int moveDirection;

    // 車が次に向かうX座標
    private float targetX;

    // ゲーム開始時に1回だけ実行される
    private void Start()
    {
        // 最初に移動する方向をランダムで決める
        DecideFirstDirection();

        // 最初の目的地を設定する
        SetNextTarget();

    }

    // ゲーム中、毎フレーム実行される
    private void Update()
    {
        // 設定された目的地へ向かって左右移動する
        MoveSideways();
    }

    // 最初に動く方向をランダムで決める
    private void DecideFirstDirection()
    {

        int randomNumber = Random.Range(0, 2);

        if (randomNumber == 0)
        {
            // -1は左方向
            moveDirection = -1;
        }
        else
        {
            // 1は右方向
            moveDirection = 1;
        }
    }

    // 次に向かうX座標を決める
    private void SetNextTarget()
    {
        // 現在のX座標に移動距離と移動方向を掛けた値を足す
        targetX = transform.position.x
            + moveDistance * moveDirection;
    }

    // 車を左右方向へ移動させる
    private void MoveSideways()
    {
        // 現在の車の位置を取得する
        Vector3 currentPosition = transform.position;

        // X座標だけを目的地へ近づける
        currentPosition.x = Mathf.MoveTowards(
            currentPosition.x,
            targetX,
            moveSpeed * Time.deltaTime
        );

        // 計算した位置を車に反映する
        transform.position = currentPosition;

        // 目的地に到着したか確認する
        if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
        {
            // 移動方向を反対にする
            moveDirection *= -1;

            // 反対方向の次の目的地を設定する
            SetNextTarget();
        }
    }
}