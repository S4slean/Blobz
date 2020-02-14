using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public CellMain nexus;

    public GameObject[] availablesCells;






    [Header("Features Unlocked")]
    public bool Quests;
    public bool TopBar;

    [Header("Cell Invincible")]
    public bool cellInvicible;


    [Header("Cells Unlocked")]
    public bool allUnlocked = true;
    [Space]

    public bool CellStockage;
    public bool CellTreblobchet;
    public bool CellBroyeur;
    public bool CellPassage;
    public bool CellBlipBlop;
    public bool CellDecharge;
    public bool CellDivine;
    public bool CellExplo;
    public bool CellSalle;
    public bool CellFusee;
    public bool CellPilone;
    public bool CellTourelle;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        if (allUnlocked)
            UnlockAllCells();
        else
            SetupUnlockedFeatures();


    }

    #region VICTORY_CONDITIONS
    public void LevelSuccessed()
    {
       // TickManager.instance.PauseTick();
        UIManager.Instance.DisplayVictoryScreen();
    }

    public void LevelFailed()
    {
        TickManager.instance.PauseTick();
        CameraController.instance.enabled = false;
        UIManager.Instance.DisplayGameOver();
    }

    #endregion

    #region GAME_SPEED
    public void SpeedGame()
    {
        Debug.Log("fasSpeed");
        Time.timeScale = 2;
    }

    public void normalGameSpeed()
    {
        Debug.Log("normalSpeed");
        CameraController.instance.enabled = true;
        InputManager.Instance.enabled = true;
        Time.timeScale = 1;
    }
    #endregion

    #region ENEMIES
    public void SpawnEnemyWave(int nbrOfEnemy, Vector3 pos)
    {

    }

    public void SpawnEnemyWave(int nbrOfEnemy, Transform transform)
    {

    }
    #endregion

    #region PLAYER_PROGRESS & SAVE
    public void UnlockAllCells()
    {
        UIManager.Instance.cellSelection.exploSubMenus[0].gameObject.SetActive(true);
        CellPassage = true;

        UIManager.Instance.cellSelection.gestionSubMenus[0].gameObject.SetActive(true);
        CellBlipBlop = true;

        UIManager.Instance.cellSelection.energySubMenus[2].gameObject.SetActive(true);
        CellBroyeur = true;

        UIManager.Instance.cellSelection.energySubMenus[1].gameObject.SetActive(true);
        CellDecharge = true;

        UIManager.Instance.cellSelection.combatSubMenus[2].gameObject.SetActive(true);
        CellDivine = true;

        UIManager.Instance.cellSelection.exploSubMenus[1].gameObject.SetActive(true);
        CellExplo = true;

        UIManager.Instance.cellSelection.gestionSubMenus[2].gameObject.SetActive(true);
        CellSalle = true;

        UIManager.Instance.cellSelection.exploSubMenus[2].gameObject.SetActive(true);
        CellFusee = true;

        UIManager.Instance.cellSelection.gestionSubMenus[1].gameObject.SetActive(true);
        CellPilone = true;

        UIManager.Instance.cellSelection.energySubMenus[0].gameObject.SetActive(true);
        CellStockage = true;

        UIManager.Instance.cellSelection.combatSubMenus[1].gameObject.SetActive(true);
        CellTourelle = true;

        UIManager.Instance.cellSelection.combatSubMenus[0].gameObject.SetActive(true);
        CellTreblobchet = true;

        UnlockSectionCheck();
    }

    public void UnlockSectionCheck()
    {
        if (CellStockage || CellDecharge || CellBroyeur)
            UIManager.Instance.cellSelection.sections[0].gameObject.SetActive(true);

        if (CellExplo || CellPassage || CellFusee)
            UIManager.Instance.cellSelection.sections[1].gameObject.SetActive(true);

        if (CellBlipBlop || CellSalle || CellPilone)
            UIManager.Instance.cellSelection.sections[2].gameObject.SetActive(true);

        if (CellTreblobchet || CellDivine || CellTourelle)
            UIManager.Instance.cellSelection.sections[3].gameObject.SetActive(true);

    }

    public void UnlockNewCell(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Accelerator:
                UIManager.Instance.cellSelection.exploSubMenus[0].gameObject.SetActive(true);
                CellPassage = true;
                break;

            case CellType.BlipBlop:
                UIManager.Instance.cellSelection.gestionSubMenus[0].gameObject.SetActive(true);
                CellBlipBlop = true;
                break;

            case CellType.Crusher:
                UIManager.Instance.cellSelection.energySubMenus[2].gameObject.SetActive(true);
                CellBroyeur = true;
                break;

            case CellType.Dump:
                UIManager.Instance.cellSelection.energySubMenus[1].gameObject.SetActive(true);
                CellDecharge = true;
                break;

            case CellType.AerialStrike:
                UIManager.Instance.cellSelection.combatSubMenus[2].gameObject.SetActive(true);
                CellDivine = true;
                break;

            case CellType.Academy:
                UIManager.Instance.cellSelection.exploSubMenus[1].gameObject.SetActive(true);
                CellExplo = true;
                break;

            case CellType.Gym:
                UIManager.Instance.cellSelection.gestionSubMenus[2].gameObject.SetActive(true);
                CellSalle = true;
                break;

            case CellType.Rocket:
                UIManager.Instance.cellSelection.exploSubMenus[2].gameObject.SetActive(true);
                CellFusee = true;
                break;

            case CellType.Battery:
                UIManager.Instance.cellSelection.gestionSubMenus[1].gameObject.SetActive(true);
                CellPilone = true;
                break;

            case CellType.Stock:
                UIManager.Instance.cellSelection.energySubMenus[0].gameObject.SetActive(true);
                CellStockage = true;
                break;

            case CellType.Turret:
                UIManager.Instance.cellSelection.combatSubMenus[1].gameObject.SetActive(true);
                CellTourelle = true;
                break;

            case CellType.Treblobchet:
                UIManager.Instance.cellSelection.combatSubMenus[0].gameObject.SetActive(true);
                CellTreblobchet = true;
                break;

        }
        UnlockSectionCheck();
    }

    public void LockCell(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Accelerator:
                UIManager.Instance.cellSelection.exploSubMenus[0].gameObject.SetActive(false);
                CellPassage = false;
                break;

            case CellType.BlipBlop:
                UIManager.Instance.cellSelection.gestionSubMenus[0].gameObject.SetActive(false);
                CellBlipBlop = false;
                break;

            case CellType.Crusher:
                UIManager.Instance.cellSelection.energySubMenus[2].gameObject.SetActive(false);
                CellBroyeur = false;
                break;

            case CellType.Dump:
                UIManager.Instance.cellSelection.energySubMenus[1].gameObject.SetActive(false);
                CellDecharge = false;
                break;

            case CellType.AerialStrike:
                UIManager.Instance.cellSelection.combatSubMenus[2].gameObject.SetActive(false);
                CellDivine = false;
                break;

            case CellType.Academy:
                UIManager.Instance.cellSelection.exploSubMenus[1].gameObject.SetActive(false);
                CellExplo = false;
                break;

            case CellType.Gym:
                UIManager.Instance.cellSelection.gestionSubMenus[2].gameObject.SetActive(false);
                CellSalle = false;
                break;

            case CellType.Rocket:
                UIManager.Instance.cellSelection.exploSubMenus[2].gameObject.SetActive(false);
                CellFusee = false;
                break;

            case CellType.Battery:
                UIManager.Instance.cellSelection.gestionSubMenus[1].gameObject.SetActive(false);
                CellPilone = false;
                break;

            case CellType.Stock:
                UIManager.Instance.cellSelection.energySubMenus[0].gameObject.SetActive(false);
                CellStockage = false;
                break;

            case CellType.Turret:
                UIManager.Instance.cellSelection.combatSubMenus[1].gameObject.SetActive(false);
                CellTourelle = false;
                break;

            case CellType.Treblobchet:
                UIManager.Instance.cellSelection.combatSubMenus[0].gameObject.SetActive(false);
                CellTreblobchet = false;
                break;

        }
        UnlockSectionCheck();
    }


    public void SetupUnlockedFeatures()
    {
        if (CellStockage)
            UnlockNewCell(CellType.Stock);

        if (CellTreblobchet)
            UnlockNewCell(CellType.Treblobchet);

        if (CellBroyeur)
            UnlockNewCell(CellType.Crusher);

        if (CellPassage)
            UnlockNewCell(CellType.Accelerator);

        if (CellBlipBlop)
            UnlockNewCell(CellType.BlipBlop);

        if (CellDecharge)
            UnlockNewCell(CellType.Dump);

        if (CellDivine)
            UnlockNewCell(CellType.AerialStrike);

        if (CellExplo)
            UnlockNewCell(CellType.Academy);

        if (CellSalle)
            UnlockNewCell(CellType.Gym);

        if (CellFusee)
            UnlockNewCell(CellType.Rocket);

        if (CellPilone)
            UnlockNewCell(CellType.Battery);

        if (CellTourelle)
            UnlockNewCell(CellType.Turret);
    }
    public void GameOver()
    {
        //GameOver
        Debug.Log("GameOver");
    }
    public void SaveUnlockedFeatures()
    {

    }
    public void LoadUnlockedFeatures()
    {

    }
    #endregion
}
