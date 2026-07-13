using UnityEngine;

public class RedCarMovement : MonoBehaviour
{
    [Header("プレイヤーに反応する距離")]
    [SerializeField] private float activationDistance = 30f;

    [Header("左右に移動する距離")]
    [SerializeField] private float moveDistance = 10f;

    [Header("左右に移動する速さ")]
    [SerializeField] private float moveSpeed = 20f;

    // 次の赤い車が動く方向を保存する
    // -1は左、1は右
    private static int nextDirection = -1;

    // プレイヤーのTransformを保存する
    private Transform player;

    // 赤い車が向かうX座標
    private float targetX;

    // プレイヤーに反応したか
    private bool hasActivated = false;

    // 現在、左右へ移動しているか
    private bool isMoving = false;

    // ゲーム開始時に1回だけ実行される
    private void Start()
    {
        // シーン内にあるPlayerControllerを探す
        PlayerController playerController =
            FindFirstObjectByType<PlayerController>();

        // プレイヤーが見つかった場合
        if (playerController != null)
        {
            // プレイヤーの位置情報を保存する
            player = playerController.transform;
        }
        else
        {
            // プレイヤーが見つからなかった場合はConsoleに警告を出す
            Debug.LogWarning(
                "RedCarMovement：PlayerControllerが見つかりません"
            );
        }
    }

    // ゲーム中、毎フレーム実行される
    private void Update()
    {
        // プレイヤーが見つかっていなければ何もしない
        if (player == null)
        {
            return;
        }

        // まだ一度も反応していない場合
        if (!hasActivated)
        {
            // プレイヤーとの距離を確認する
            CheckPlayerDistance();
        }

        // 左右移動中の場合
        if (isMoving)
        {
            // 目的地へ移動する
            MoveSideways();
        }
    }

    // プレイヤーが近づいたか確認する
    private void CheckPlayerDistance()
    {
        // 赤い車とプレイヤーの距離を計算する
        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        // プレイヤーが設定した距離まで近づいた場合
        if (distance <= activationDistance)
        {
            // 左右交互の移動を開始する
            StartAlternatingMove();
        }
    }

    // 左右交互に動く処理を開始する
    private void StartAlternatingMove()
    {
        // この赤い車は反応済みにする
        hasActivated = true;

        // 移動中にする
        isMoving = true;

        // 今回の赤い車が動く方向を保存する
        int direction = nextDirection;

        // 次の赤い車は反対方向にする
        nextDirection *= -1;

        // 現在位置から左または右の目的地を計算する
        targetX = transform.position.x
            + moveDistance * direction;
    }

    // 赤い車を左右へ移動させる
    private void MoveSideways()
    {
        // 現在位置を取得する
        Vector3 currentPosition = transform.position;

        // X座標だけを目的地へ近づける
        currentPosition.x = Mathf.MoveTowards(
            currentPosition.x,
            targetX,
            moveSpeed * Time.deltaTime
        );

        // 計算した位置を赤い車に反映する
        transform.position = currentPosition;

        // 目的地に到着したか確認する
        if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
        {
            // 移動を終了する
            isMoving = false;
        }
    }
}