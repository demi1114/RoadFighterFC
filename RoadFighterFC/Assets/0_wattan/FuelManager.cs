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

    private int previousFuelInt;

    void Start()
    {
        currentFuel = maxFuel;
        previousFuelInt = Mathf.FloorToInt(currentFuel);

        Debug.Log($"開始時 燃料 : {previousFuelInt}");
        Debug.Log($"開始時 スコア : {score}");
    }

    void Update()
    {
        if (isGameOver) return;

        // 燃料減少
        currentFuel -= fuelDecreaseRate * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        int currentFuelInt = Mathf.FloorToInt(currentFuel);

        // ★燃料が減った瞬間
        if (currentFuelInt < previousFuelInt)
        {
            int diff = previousFuelInt - currentFuelInt;

            previousFuelInt = currentFuelInt;

            for (int i = 0; i < diff; i++)
            {
                score += 50;
            }

            Debug.Log($"燃料 : {currentFuelInt} / スコア : {score}");
        }

        if (currentFuel <= 0f)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 補給車
    /// </summary>
    public void AddFuel(float amount)
    {
        if (isGameOver) return;

        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        previousFuelInt = Mathf.FloorToInt(currentFuel);

        Debug.Log($"補給！燃料 : {previousFuelInt} / スコア : {score}");
    }

    /// <summary>
    /// クラッシュ
    /// </summary>
    public void CrashPenalty(float amount = 5f)
    {
        if (isGameOver) return;

        currentFuel -= amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        previousFuelInt = Mathf.FloorToInt(currentFuel);

        Debug.Log($"クラッシュ！燃料 : {previousFuelInt} / スコア : {score}");

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