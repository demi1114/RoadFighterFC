using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    [SerializeField] private string retrySceneName = "Saka";
    [SerializeField] private string titleSceneName = "TitleScene";

    void Start()
    {
        int score = PlayerPrefs.GetInt("ResultScore", 0);
        float time = PlayerPrefs.GetFloat("ResultTime", 0f);

        scoreText.text = "SCORE\n" + score.ToString("D6");

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);

        timeText.text = $"TIME\n{minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    public void Retry()
    {
        SceneManager.LoadScene(retrySceneName);
    }

    public void BackTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}