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
    public bool CellStockage;
    public bool CellArmory;
    public bool CellBroyeur;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);



    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(1);
        }
    }


    public void SpawnEnemyWave(int nbrOfEnemy, Vector3 pos)
    {
        
    }

    public void SpawnEnemyWave(int nbrOfEnemy, Transform transform)
    {

    }


    #region PLAYER_PROGRESS & SAVE
    public void UnlockNewCell(CelluleTemplate newCellTemplate)
    {
        //Créer nouveau bouton, lui associer le template, sprite, la bonne fonction;

        //Ajouter à l'ui le boutton

        //le référencer dans availableCells

        //cocher le bon booléens
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
