using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // スコア
    private int score;

    // ゲーム開始時に一度だけ実行
    private void Start()
    {
        ResetScore();
    }

    // スコアを0に初期化する
    private void ResetScore()
    {
        score = 0;
        Debug.Log("スコアを0に初期化しました");
    }
}