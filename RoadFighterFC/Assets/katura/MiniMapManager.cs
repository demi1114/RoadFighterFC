using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    [Header("ミニマップの背景")]
    [SerializeField] private Image background;

    [Header("スタート旗")]
    [SerializeField] private Image startFlag;

    [Header("ゴール旗")]
    [SerializeField] private Image goalFlag;

    [Header("プレイヤーアイコン")]
    [SerializeField] private Image playerIcon;

    [Header("プレイヤー")]
    [SerializeField] private Transform player;

    [Header("コース設定")]
    [SerializeField] private float startPosition = 0f;
    [SerializeField] private float goalPosition = 10000f;

    private void Update()
    {
        GetPlayerPosition();
    }

    private void GetPlayerPosition()
    {

        Vector3 playerPosition = player.position;

        // 進行度（0～1）
        float progress = Mathf.InverseLerp(
            startPosition,
            goalPosition,
            playerPosition.z
        );

        // 0～100％に変換
        float progressPercent = progress * 100f;

        // 整数に変換
        int progressPercentInt = Mathf.FloorToInt(progressPercent);

        Debug.Log("進行度 : " + progressPercentInt + " %");

        Vector2 startPos = startFlag.rectTransform.anchoredPosition;
        Vector2 goalPos = goalFlag.rectTransform.anchoredPosition;
        Vector2 currentPos = playerIcon.rectTransform.anchoredPosition;

        float newY = Mathf.Lerp(startPos.y, goalPos.y, progress);

        playerIcon.rectTransform.anchoredPosition =
            new Vector2(currentPos.x, newY);

    }
}