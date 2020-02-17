using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopBarUI : MonoBehaviour
{

    public TextMeshProUGUI energy;
    public energieTopBar energieTopBar;
    public TextMeshProUGUI rocketShards;

    public void UpdateUI()
    {
        energy.text = "Sploosh : " + RessourceTracker.instance.energy + "/ " + RessourceTracker.instance.energyCap;
        energieTopBar.CheckEnergieAmount();

        rocketShards.text = "Rocket Shards: " + RessourceTracker.instance.rocketPiece + "/3";

    }

    public void ReplayLevel()
    {
        SceneHandler.instance.ReplayLevel();
    }

    public void NormalSpeed()
    {
        LevelManager.instance.normalGameSpeed();
    }

    public void FastSpeed()
    {
        LevelManager.instance.SpeedGame();
    }

}
