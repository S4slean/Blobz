using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string levelSelectionSceneName;

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        SceneManager.LoadScene(levelSelectionSceneName);
    }

}
