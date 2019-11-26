using System.Collections;
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
            //DontDestroyOnLoad(gameObject);
        }

        if (cellSelection == null)
        {
            Debug.LogError("Il faut rajouter , CellSelecetion sur Choose a Cell  ", cellSelection.transform);
        }

    }

    private void Start()
    {
        TickManager.doTick += TopBar.UpdateUI;
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



    public void DisplayCellShop(CellMain originalCell)
    {
        cellSelection.transform.position = originalCell.transform.position;
        cellSelection.gameObject.SetActive(true);
        cellSelection.ButtonPositions(originalCell);
        InputManager.Instance.InCellSelection = true;
    }
    public IEnumerator DesactivateCellShop ()
    {
        yield return new WaitForEndOfFrame();
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


    #endregion

    #region CELL_TOOLTIP


    [Header("ToolTip")]
    public TooltipUI tooltipUI;
    public CellOptionsUI cellOptionsUI;
    

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
        DisplayUI(tooltipUI.gameObject);
        tooltipUI.transform.position = pos + Vector3.up;
        //tooltipUI.anim.Play("first Display");



        firstTooltipDisplayed = true;
    }

    public void DisplaySecondToolTip()
    {

        secondTooltipDisplayed = true;

        DisplayUI(tooltipUI.secondDisplay);
        //tooltipUI.anim.Play("second Display");
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

    public void HideTooltip()
    {
        HideUI(tooltipUI.gameObject);
    }

    public void DisplayCellOptions(CellMain cell)
    {
        DisplayUI(cellOptionsUI.gameObject);
        cellOptionsUI.transform.position = cell.transform.position + Vector3.up;
        cellOptionsUI.cell = cell;
        //cellOptionsUI.anim.Play("Display");
    }

    #endregion
}