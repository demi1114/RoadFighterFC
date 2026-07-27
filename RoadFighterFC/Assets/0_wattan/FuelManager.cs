using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    [Header("燃料UI")]
    public TMP_Text fuelText;

    [Header("スコアUI")]
    public TMP_Text scoreText;

    [Header("ゴール地点(Z座標)")]
    public float goalZ = 1000f;

    private bool isGameOver = false;
    private bool isGoal = false;

    private int previousFuelInt;
    [Header("クリア画面")]
    public GameObject clearPanel;

    [Header("クリア文字")]
    public TMP_Text clearText;

    [Header("最終スコア")]
    public TMP_Text resultScoreText;

    [Header("リトライするシーン")]
    [SerializeField] private string retrySceneName;

    [Header("タイトルシーン")]
    [SerializeField] private string titleSceneName;

    [Header("ゲームオーバー時のシーン")]
    [SerializeField] private string gameOverSceneName;

    private GameObject player;

    void Start()
    {
        currentFuel = maxFuel;
        previousFuelInt = Mathf.FloorToInt(currentFuel);

        player = GameObject.FindGameObjectWithTag("Player");
        UpdateFuelUI();
        UpdateScoreUI();
        Debug.Log($"開始時 燃料 : {previousFuelInt}");
        Debug.Log($"開始時 スコア : {score}");

        if (clearPanel != null)
            clearPanel.SetActive(false);
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

        UpdateFuelUI();

        int currentFuelInt = Mathf.FloorToInt(currentFuel);

        // 燃料が減った分だけスコア加算
        if (currentFuelInt < previousFuelInt)
        {
            int diff = previousFuelInt - currentFuelInt;

            previousFuelInt = currentFuelInt;

            score += diff * 50;

            UpdateScoreUI();

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

     // 燃料UI更新
    void UpdateFuelUI()
    {
        if (fuelText == null)
        {
            Debug.Log("FuelTextが設定されていません");
            return;
        }

        fuelText.text = Mathf.FloorToInt(currentFuel).ToString();

        Debug.Log("UI更新：" + fuelText.text);
    }
    // スコアUI更新
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString("D6");
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

        UpdateScoreUI();

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

        UpdateFuelUI();

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

        Debug.Log("残り燃料：" + currentFuel);
        Debug.Log("ボーナス：" + bonus);

        score += bonus;

        Debug.Log("最終スコア：" + score);

        UpdateScoreUI();

        Debug.Log("===== GOAL =====");
        Debug.Log($"最終スコア : {score}");

        // 全て停止
        Time.timeScale = 0f;

        // プレイヤー停止
        if (player != null)
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            if (controller != null)
                controller.enabled = false;
        }

        // UI表示
        clearPanel.SetActive(true);

        clearText.text = "GAME CLEAR";

        resultScoreText.text =
            "\n" + score.ToString("D6");
    }
    //ゲームオーバー
    void GameOver()
    {
        isGameOver = true;

        Debug.Log($"ゲームオーバー！最終スコア：{score}");

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameOverSceneName);
    }


    public void Retry()
    {
        Debug.Log("Retry押された");
        Time.timeScale = 1f;
        SceneManager.LoadScene(retrySceneName);
    }

    public void BackTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}


  