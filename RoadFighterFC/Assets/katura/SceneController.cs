using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionButton : MonoBehaviour
{
    // インスペクターで文字を入力するための箱
    [SerializeField] private string targetSceneName;

    // ボタンを押したときに実行する中身
    public void TransitionToScene()
    {
        // 箱に入っているシーン名に切り替える
        SceneManager.LoadScene(targetSceneName);
    }
}