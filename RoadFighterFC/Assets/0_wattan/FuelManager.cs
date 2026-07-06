using UnityEngine;
using UnityEngine.SceneManagement;

public class FuelManager : MonoBehaviour
{
    [Header("初期燃料")]
    public float maxFuel = 100f;

    [Header("現在燃料")]
    public float currentFuel;

    [Header("減少速度（1秒あたり）")]
    public float fuelDecreaseRate = 5f;

    [Header("現在スコア")]
    public int score = 0;

    [Header("ゲームオーバー時に再読み込みするシーン名")]
    public string sceneName = "GameScene";

    private bool isGameOver = false;

    // 燃料の整数値を記録（スコア加算用）
    private int previousFuelInt;

    void Start()
    {
        currentFuel = maxFuel;
        previousFuelInt = Mathf.FloorToInt(currentFuel);
    }

    void Update()
    {
        if (isGameOver) return;

        // 時間経過で燃料減少
        currentFuel -= fuelDecreaseRate * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        // 燃料が1減るごとに50点加算
        int currentFuelInt = Mathf.FloorToInt(currentFuel);

        if (currentFuelInt < previousFuelInt)
        {
            int diff = previousFuelInt - currentFuelInt;
            score += diff * 50;
            previousFuelInt = currentFuelInt;
        }

        // 燃料切れ
        if (currentFuel <= 0f)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 燃料を追加（補給車）
    /// </summary>
    public void AddFuel(float amount)
    {
        if (isGameOver) return;

        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        // スコアは増やさず基準だけ更新
        previousFuelInt = Mathf.FloorToInt(currentFuel);
    }

    /// <summary>
    /// クラッシュ時の燃料ペナルティ
    /// </summary>
    public void CrashPenalty(float amount = 5f)
    {
        if (isGameOver) return;

        currentFuel -= amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        // クラッシュではスコアは加算しない
        previousFuelInt = Mathf.FloorToInt(currentFuel);

        Debug.Log($"クラッシュ！燃料 -{amount}");

        if (currentFuel <= 0f)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;

        Debug.Log($"ゲームオーバー！最終スコア：{score}");

        SceneManager.LoadScene(sceneName);
    }
}