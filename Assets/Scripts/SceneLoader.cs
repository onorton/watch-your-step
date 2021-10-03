using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string _nextScene;

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Next()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_nextScene);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
