using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneHandler.instance.ReplayLevel();
    }

    public void BackToLevelSelection()
    {
        SceneHandler.instance.BackToLevelSelection();
    }
}
