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
    }

    public void UnlockNewCell(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Passage:
                UIManager.Instance.cellSelection.exploSubMenus[0].gameObject.SetActive(true);
                CellPassage = true;
                break;

            case CellType.BlipBlop:
                UIManager.Instance.cellSelection.gestionSubMenus[0].gameObject.SetActive(true);
                CellBlipBlop = true;
                break;

            case CellType.Broyeur:
                UIManager.Instance.cellSelection.energySubMenus[2].gameObject.SetActive(true);
                CellBroyeur = true;
                break;

            case CellType.Decharge:
                UIManager.Instance.cellSelection.energySubMenus[1].gameObject.SetActive(true);
                CellDecharge = true;
                break;

            case CellType.Divine:
                UIManager.Instance.cellSelection.combatSubMenus[2].gameObject.SetActive(true);
                CellDivine = true;
                break;

            case CellType.Exploration:
                UIManager.Instance.cellSelection.exploSubMenus[1].gameObject.SetActive(true);
                CellExplo = true;
                break;

            case CellType.LaSalle:
                UIManager.Instance.cellSelection.gestionSubMenus[2].gameObject.SetActive(true);
                CellSalle = true;
                break;

            case CellType.Merveille:
                UIManager.Instance.cellSelection.exploSubMenus[2].gameObject.SetActive(true);
                CellFusee = true;
                break;

            case CellType.Pilone:
                UIManager.Instance.cellSelection.gestionSubMenus[1].gameObject.SetActive(true);
                CellPilone = true;
                break;

            case CellType.Stockage:
                UIManager.Instance.cellSelection.energySubMenus[0].gameObject.SetActive(true);
                CellStockage = true;
                break;

            case CellType.Tourelle:
                UIManager.Instance.cellSelection.combatSubMenus[1].gameObject.SetActive(true);
                CellTourelle = true;
                break;

            case CellType.Treblochet:
                UIManager.Instance.cellSelection.combatSubMenus[0].gameObject.SetActive(true);
                CellTreblobchet = true;
                break;
        }
    }

    public void SetupUnlockedFeatures()
    {
        if (CellStockage)
            UnlockNewCell(CellType.Stockage);

        if (CellTreblobchet)
            UnlockNewCell(CellType.Treblochet);

        if (CellBroyeur)
            UnlockNewCell(CellType.Broyeur);

        if (CellPassage)
            UnlockNewCell(CellType.Passage);

        if (CellBlipBlop)
            UnlockNewCell(CellType.BlipBlop);

        if (CellDecharge)
            UnlockNewCell(CellType.Decharge);

        if (CellDivine)
            UnlockNewCell(CellType.Divine);

        if (CellExplo)
            UnlockNewCell(CellType.Exploration);

        if (CellSalle)
            UnlockNewCell(CellType.LaSalle);

        if (CellFusee)
            UnlockNewCell(CellType.Merveille);

        if (CellPilone)
            UnlockNewCell(CellType.Pilone);

        if (CellTourelle)
            UnlockNewCell(CellType.Tourelle);
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
