﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (cellSelection == null)
        {
            Debug.LogError("Il faut rajouter , CellSelecetion sur Choose a Cell  ", cellSelection.transform);
        }

    }

    private void Start()
    {
        UpdateEnergy();
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

    #endregion

    #region CELL_SELECTION

    [Header("Cell Selection")]

    public CellSelectionShop cellSelection;
    public GameObject SelectedCellUI;



    public void InUICellSelection(Vector3 pos , CellMain originalCell , LineRenderer currentLine)
    {
        //c'est du debug
        currentLine.startColor = Color.red;
        currentLine.endColor = Color.red;

        cellSelection.transform.position = pos;
        cellSelection.gameObject.SetActive(true);
        cellSelection.ButtonPositions(originalCell);
        InputManager.Instance.InCellSelection = true;
    }
    public void DesactivateCellShop ()
    {
        cellSelection.DesactiveButton();
        InputManager.Instance.InCellSelection = false;
        ///CellManager2.Instance.SupressCurrentLink();
    }
    public void CellSelected( Vector3 pos)
    {
        SelectedCellUI.transform.position = pos;
        SelectedCellUI.SetActive(true);
    }
    public void CellDeselected()
    {
        SelectedCellUI.SetActive(false);
    }

    #endregion

    #region QUEST_UI

    [Header("Quests")]
    public QuestUI QuestUI;

    #endregion

    #region TOP_BAR
    [Header("Top Bar")]
    public TopBarUI TopBar;

    public void UpdateEnergy()
    {

    }
    #endregion

    #region CELL_TOOLTIP


    [Header("ToolTip")]
    public TooltipUI tooltipUI;

    private float tooltipCount = 0;
    public float firstTooltipDelay = .4f;
    private bool firstTooltipDisplayed = false;
    public float secondTooltipDelay = 1.5f;
    private bool secondTooltipDisplayed = false;

    public void LoadToolTip(Vector3 pos, CellMain cell)
    {
        tooltipCount += Time.deltaTime;

        if(tooltipCount > firstTooltipDelay && !firstTooltipDisplayed)
        {
            DisplayTooltip(pos, cell) ;
        }

        if(tooltipCount > secondTooltipDelay && !secondTooltipDisplayed)
        {
            DisplaySecondToolTip();
        }

    }

    public void DisplayTooltip(Vector3 pos, CellMain cell)
    {
        tooltipUI.UpdateUI(cell);
        tooltipUI.anim.Play("first Display");
        tooltipUI.transform.position = pos + Vector3.up;



        DisplayUI(tooltipUI.gameObject);
        firstTooltipDisplayed = true;
    }

    public void DisplaySecondToolTip()
    {

        secondTooltipDisplayed = true;

        tooltipUI.anim.Play("second Display");
    }

    public void UnloadToolTip()
    {

        if (secondTooltipDisplayed)
            tooltipUI.anim.Play("hide both");

        else if (firstTooltipDisplayed)
            tooltipUI.anim.Play("hide first");

    
        tooltipCount = 0;


        firstTooltipDisplayed = false;
        secondTooltipDisplayed = false;
    }

    public void HideTooltip()
    {
        HideUI(tooltipUI.gameObject);


    }

    #endregion
}