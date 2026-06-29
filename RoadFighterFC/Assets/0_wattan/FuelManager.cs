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

    // ★追加：クラッシュ中フラグ
    public bool isCrashed = false;

    void Start()
    {
        currentFuel = maxFuel;
    }

    void Update()
    {
        if (isGameOver) return;

        // ★クラッシュ中は燃料減らさない
        if (!isCrashed)
        {
            currentFuel -= fuelDecreaseRate * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        }

        if (currentFuel <= 0f)
        {
            GameOver();
        }
    }

    public void AddFuel(float amount)
    {
        if (isGameOver) return;

        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("燃料切れ！ゲームオーバー");
        SceneManager.LoadScene(sceneName);
    }
}