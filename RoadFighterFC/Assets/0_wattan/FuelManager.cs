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

    [Header("ゲームオーバー時に再読み込みするシーン名")]
    public string sceneName = "GameScene";

    private bool isGameOver = false;

    void Start()
    {
        currentFuel = maxFuel;
    }

    void Update()
    {
        if (isGameOver) return;

        // 燃料を時間経過で減少
        currentFuel -= fuelDecreaseRate * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        // 燃料切れ
        if (currentFuel <= 0f)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 燃料を追加
    /// </summary>
    public void AddFuel(float amount)
    {
        if (isGameOver) return;

        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
    }

    /// <summary>
    /// クラッシュ時の燃料ペナルティ
    /// </summary>
    public void CrashPenalty(float amount = 5f)
    {
        if (isGameOver) return;

        currentFuel -= amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        Debug.Log($"クラッシュ！燃料 -{amount}");

        if (currentFuel <= 0f)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("燃料切れ！ゲームオーバー");
        SceneManager.LoadScene(sceneName);
    }
}