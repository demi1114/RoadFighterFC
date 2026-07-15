using UnityEngine;
using UnityEngine.SceneManagement;

public class FuelManager : MonoBehaviour
{
    [Header("初期燃料")]
    public float maxFuel = 100f;

    [Header("現在燃料")]
    public float currentFuel;

    [Header("プレイヤー現在Z座標（確認用）")]
    public float currentPlayerZ;

    [Header("減少速度（1秒あたり）")]
    public float fuelDecreaseRate = 5f;

    [Header("現在スコア")]
    public int score = 0;

    [Header("ゲームオーバー時に再読み込みするシーン名")]
    public string sceneName = "GameScene";

    [Header("ゴール地点(Z座標)")]
    public float goalZ = 1000f;

    private bool isGameOver = false;
    private bool isGoal = false;

    private int previousFuelInt;

    private GameObject player;

    void Start()
    {
        currentFuel = maxFuel;
        previousFuelInt = Mathf.FloorToInt(currentFuel);

        player = GameObject.FindGameObjectWithTag("Player");

        Debug.Log($"開始時 燃料 : {previousFuelInt}");
        Debug.Log($"開始時 スコア : {score}");
    }

    void Update()
    {
        if (isGameOver || isGoal) return;

        if (player != null)
        {
            currentPlayerZ = player.transform.position.z;
        }

        // 燃料減少
        currentFuel -= fuelDecreaseRate * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        int currentFuelInt = Mathf.FloorToInt(currentFuel);

        // 燃料が減った分だけスコア加算
        if (currentFuelInt < previousFuelInt)
        {
            int diff = previousFuelInt - currentFuelInt;

            previousFuelInt = currentFuelInt;

            score += diff * 50;

            Debug.Log($"燃料 : {currentFuelInt} / スコア : {score}");
        }

        // ゴール判定
        if (player != null && player.transform.position.z >= goalZ)
        {
            Goal();
            return;
        }

        // ゲームオーバー判定
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
        if (isGameOver || isGoal) return;

        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        previousFuelInt = Mathf.FloorToInt(currentFuel);

        Debug.Log($"補給！燃料 : {previousFuelInt}");
    }

    /// <summary>
    /// スコア加算
    /// </summary>
    public void AddScore(int amount)
    {
        if (isGameOver || isGoal) return;

        score += amount;

        Debug.Log($"スコア +{amount}　現在スコア : {score}");
    }

    /// <summary>
    /// クラッシュ
    /// </summary>
    public void CrashPenalty(float amount = 5f)
    {
        if (isGameOver || isGoal) return;

        currentFuel -= amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        previousFuelInt = Mathf.FloorToInt(currentFuel);

        Debug.Log($"クラッシュ！燃料 : {previousFuelInt} / スコア : {score}");

        if (currentFuel <= 0f)
        {
            GameOver();
        }
    }

    /// <summary>
    /// ゴール
    /// </summary>
    void Goal()
    {
        isGoal = true;

        int bonus = Mathf.FloorToInt(currentFuel) * 30;
        score += bonus;

        Debug.Log("===== GOAL =====");
        Debug.Log($"残り燃料 : {Mathf.FloorToInt(currentFuel)}");
        Debug.Log($"燃料ボーナス : +{bonus}");
        Debug.Log($"最終スコア : {score}");

        // リザルト画面へ行く場合はここ
        // SceneManager.LoadScene("ResultScene");
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    void GameOver()
    {
        isGameOver = true;

        Debug.Log($"ゲームオーバー！最終スコア：{score}");

        SceneManager.LoadScene(sceneName);
    }
}