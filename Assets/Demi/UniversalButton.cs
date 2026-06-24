using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalButton : MonoBehaviour
{
    [SerializeField] private string targetSceneName;

    public void TransitionToScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}