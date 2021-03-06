﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    private void Awake()
    {

        Instance = this;


        if (cellSelection == null)
        {
            Debug.LogError("Il faut rajouter , CellSelecetion sur Choose a Cell  ", cellSelection.transform);
        }

        QuestUI.gameObject.SetActive(true);

    }


    private void Update()
    {
        UpdateAllUI();
    }


    #region GENERICS

    public void DisplayUI(GameObject go)
    {
        go.SetActive(true);
    }

    public void HideUI(GameObject go)
    {
        go.SetActive(false);
    }

    public void UpdateAllUI()
    {
        if (TopBar.gameObject.activeSelf)
            TopBar.UpdateUI();
        if (QuestUI.gameObject.activeSelf)
            QuestUI.UpdateUI();
    }
    #endregion

    #region CELL_SELECTION

    [Header("Cell Selection")]

    public CellSelectionShop cellSelection;
    public GameObject SelectedCellUI;



    public void DisplayCellShop(CellMain originalCell)
    {
        cellSelection.transform.position = originalCell.transform.position + Vector3.up * 2;

        cellSelection.DisplaySections();
        cellSelection.DisplayAllSubMenus();
    }

    public void DesactivateCellShop()
    {
        cellSelection.HideSections();
    }



    public void CellSelected(Vector3 pos)
    {
        SelectedCellUI.transform.position = pos;
        SelectedCellUI.SetActive(true);
    }
    public void DeselectElement()
    {
        SelectedCellUI.SetActive(false);
    }

    #endregion

    #region QUEST_UI

    [Header("Quests")]
    public QuestUI QuestUI;
    public QuestPopUp questEventPopUpWorld;
    public QuestPopUp questEventPopUpOverlay;
    public QuestPopUp persistentMsg;


    public void HidePersistentMsg()
    {
        persistentMsg.anim.SetBool("Show", false);
    }

    public void DisplayPopUp(QuestPopUp pop)
    {
        pop.anim.SetBool("Show", true);
        pop.anim.Play("Show");
    }

    public void HidePopUp(QuestPopUp pop)
    {
        pop.anim.SetBool("Show", false);
    }

    #endregion

    #region TOP_BAR
    [Header("Top Bar")]
    public TopBarUI TopBar;


    #endregion

    #region CELL_TOOLTIP


    [Header("ToolTip")]
    public TooltipUI tooltipUI;
    public CellOptionsUI cellOptionsUI;
    public ProximityToolTip proximityToolTipUI;
    public PropsTooltip propsTooltip;


    private float tooltipCount = 0;
    public float firstTooltipDelay = .4f;
    private bool firstTooltipDisplayed = false;
    public float secondTooltipDelay = 1.5f;
    private bool secondTooltipDisplayed = false;

    public void LoadToolTip(Vector3 pos, CellMain cell, bool displayCost, bool displaySecondToolTip)
    {
        tooltipCount += Time.deltaTime;

        if (tooltipCount > firstTooltipDelay && !firstTooltipDisplayed)
        {
            DisplayTooltip(pos, cell, displayCost);
        }

        if (displaySecondToolTip)
        {
            DisplaySecondToolTip();
        }

    }
    public void UnloadToolTip()
    {

        if (secondTooltipDisplayed)
            HideUI(tooltipUI.secondDisplay);

        if (firstTooltipDisplayed)
            HideUI(tooltipUI.gameObject);


        tooltipCount = 0;


        firstTooltipDisplayed = false;
        secondTooltipDisplayed = false;
    }
    public void DisplayTooltip(Vector3 pos, CellMain cell, bool displayCost)
    {
        DisplayUI(tooltipUI.gameObject);
        tooltipUI.UpdateUI(cell, displayCost);
        tooltipUI.transform.position = pos + Vector3.up;
        //tooltipUI.anim.Play("first Display");



        firstTooltipDisplayed = true;
    }
    public void HideTooltip()
    {
        HideUI(tooltipUI.gameObject);
    }
    public void DisplaySecondToolTip()
    {

        secondTooltipDisplayed = true;

        DisplayUI(tooltipUI.secondDisplay);
        //tooltipUI.anim.Play("second Display");
    }

    public void DisplayProximityToolTip(Vector3 pos, CellMain cell)
    {
        proximityToolTipUI.gameObject.SetActive(true);
        proximityToolTipUI.UpdateInfo(cell);
        proximityToolTipUI.transform.position = pos + Vector3.up;
    }
    public void UpdateProximityToolTipPos(Vector3 pos)
    {
        proximityToolTipUI.transform.position = pos + Vector3.up;
    }
    public void HideProximityToolTip()
    {
        proximityToolTipUI.gameObject.SetActive(false);
    }

    public void DisplayPropsTooltip(Vector3 pos, TooltipScriptable tooltipData)
    {
        propsTooltip.gameObject.SetActive(true);
        propsTooltip.UpdateTooltipInfos(tooltipData);
        propsTooltip.transform.position = pos + Vector3.up;
    }
    public void HidePropsTooltip()
    {
        propsTooltip.gameObject.SetActive(false);
    }

    public void DisplayCellOptions(CellMain cell)
    {
        DisplayUI(cellOptionsUI.gameObject);
        cellOptionsUI.transform.position = cell.transform.position + Vector3.up;
        cellOptionsUI.cell = cell;
        //cellOptionsUI.anim.Play("Display");
    }
    public void HideCellOptions()
    {
        HideUI(UIManager.Instance.cellOptionsUI.gameObject);
    }

    #endregion

    #region VILLAGE_SELECTION
    [Header("Village Destruction")]
    public VillageSelectionBtn villageSelection;

    public void DisplayVillageSelection(EnemyVillage village)
    {
        villageSelection.transform.position = village.transform.position + Vector3.up * 5 + Vector3.forward * 3;
        villageSelection.SplouchAmount = village.splouchOnDestruction;
        villageSelection.village = village;

        villageSelection.gameObject.SetActive(true);
        villageSelection.UpdateText();
    }

    public void HideVillageSelection()
    {
        villageSelection.gameObject.SetActive(false);
    }

    #endregion

    #region COLONY
    [Header("Colony")]
    public Transform colonyCreation;

    public void DisplayColonyBtn(NexusAera area)
    {
        ColonyBtn colonyBtn = ObjectPooler.poolingSystem.GetPooledObject<ColonyBtn>() as ColonyBtn;
        Debug.Log("Woooo");
        colonyBtn.cost = area.splouchCost;
        colonyBtn.point = area.transform.position;
        colonyBtn.transform.SetParent(colonyCreation);
        colonyBtn.transform.position = area.transform.position + Vector3.up * 2;
        colonyBtn.nexus = area;
        colonyBtn.Outpool();
        colonyBtn.anim.SetBool("Show", true);
        area.btn = colonyBtn;
        colonyBtn.UpdateText();
    }

    public void HideColonyBtn(NexusAera area)
    {
        area.btn.Inpool(); ;
    }
    #endregion

    #region DIVINE_CELL
    [Header("Divine Cell")]
    public GameObject divineCellTarget;
    public GameObject divineShotArea;

    public void DisplayDivineShot(CellDivine shootingCell)
    {
        InputManager.Instance.UpdateTargetPos();
        UpdateShootingArea(shootingCell.specifiqueStats , shootingCell.myCellTemplate.explosionRadius);
        divineShotArea.transform.position = shootingCell.graphTransform.position + new Vector3(0, 0.1f, 0);
        divineCellTarget.SetActive(true);
        divineShotArea.SetActive(true);
    }
    public void HideDivineShot()
    {
        divineCellTarget.SetActive(false);
        divineShotArea.SetActive(false);
    }


    public void UpdateShootingArea(float newRange , float areaRadius)
    {
        if (InputManager.Instance.shootingCell == null)
            return;
        divineShotArea.transform.position = InputManager.Instance.shootingCell.graphTransform.position + new Vector3(0, 0.3f, 0);
        divineShotArea.transform.localScale = (Vector3.one * newRange / 0.075f);
        divineCellTarget.transform.localScale = (Vector3.one * areaRadius / 0.075f);
        //divineShotArea.transform.localScale = (Vector3.one );
    }

    public void SetTargetPos(Vector3 pos)
    {
        divineCellTarget.transform.position = pos + new Vector3(0, 0.05f, 0);
    }

    #endregion

    #region WARNING_MESSAGE
    public WarningMessage warning;

    public void WarningMessage(string text)
    {
        warning.Display(text);
    }

    #endregion

    #region ALERT

    [Header("CellExplosionAlert")]
    [Range(0, 1000)] public float alertRadius = 200;
    public Transform alertHolder;
    [Range(0, 20)] public float offsetPercentage;

    public void DisplayCellAlert(CellMain cell, CellAlert alert)
    {


        if (alert.transform.parent != alertHolder)
            alert.transform.SetParent(alertHolder);

        alert.Outpool();
        alert.Display(cell);

    }


    public void HideCellAlert(CellAlert alert)
    {
        alert.Hide();
    }


    #endregion

    #region PAUSE MENU 

    [Header("PauseMenu")]
    public PauseMenu pauseMenu;

    #endregion

    #region VICTORY_SCREEN
    public Animator victoryScreen;

    public void DisplayVictoryScreen()
    {
        victoryScreen.Play("Appear");
    }


    #endregion

    #region GAMEOVER
    public GameObject gameOverScreen;

    public void DisplayGameOver()
    {
        gameOverScreen.SetActive(true);
    }

    #endregion

}

