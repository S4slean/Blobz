using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string levelSelectionSceneName;

    public void RestartLevel()
    {
        SceneHandler.instance.ChangeScene(SceneManager.GetActiveScene().name);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        UIManager.Instance.HideUI(gameObject);
    }

    public void DisplayQuitButton()
    {

    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void LevelSelectionMenu()
    {
        SceneHandler.instance.ChangeScene(levelSelectionSceneName);
    }

}
