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
        if(TopBar.gameObject.activeSelf)
            TopBar.UpdateUI();
        if(QuestUI.gameObject.activeSelf)
            QuestUI.UpdateUI();
    }
    #endregion

    #region CELL_SELECTION

    [Header("Cell Selection")]

    public CellSelectionShop cellSelection;
    public GameObject SelectedCellUI;



    public void DisplayCellShop(CellMain originalCell)
    {
        cellSelection.transform.position = originalCell.transform.position + Vector3.up * .5f;
        
        cellSelection.DisplaySections();
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

    public void LoadToolTip(Vector3 pos, CellMain cell, bool displayCost)
    {
        tooltipCount += Time.deltaTime;

        if (tooltipCount > firstTooltipDelay && !firstTooltipDisplayed)
        {
            DisplayTooltip(pos, cell, displayCost);
        }

        if (tooltipCount > secondTooltipDelay && !secondTooltipDisplayed)
        {
            DisplaySecondToolTip();
        }

    }

    public void DisplayTooltip(Vector3 pos, CellMain cell, bool displayCost)
    {
        tooltipUI.UpdateUI(cell, displayCost);
        DisplayUI(tooltipUI.gameObject);
        tooltipUI.transform.position = pos + Vector3.up;
        //tooltipUI.anim.Play("first Display");



        firstTooltipDisplayed = true;
    }

    public void DisplayCostToolTip(Vector3 pos, CellMain cell)
    {
        //costUI.transform.position = pos + Vector3.forward;
    }

    public void HideCostTooltip()
    {

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
        ColonyBtn colonyBtn = ObjectPooler.poolingSystem.GetPooledObject<ColonyBtn>() as ColonyBtn ;
        Debug.Log("Woooo");
        colonyBtn.cost = area.splouchCost;
        colonyBtn.point = area.transform.position;
        colonyBtn.transform.SetParent(colonyCreation);
        colonyBtn.transform.position = area.transform.position + Vector3.up * 2;
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
        UpdateShootingArea(shootingCell.specifiqueStats);
        divineShotArea.transform.position = shootingCell.transform.position;
        divineCellTarget.SetActive(true);
        divineShotArea.SetActive(true);
    }
    public void HideDivineShot()
    {
        divineCellTarget.SetActive(false);
        divineShotArea.SetActive(false);
    }


    public void UpdateShootingArea(float newRange)
    {
        if (InputManager.Instance.shootingCell == null)
            return;
        divineShotArea.transform.position = InputManager.Instance.shootingCell.transform.position;
        divineShotArea.transform.localScale = (Vector3.one * newRange /0.075f);
    }

    public void SetTargetPos(Vector3 pos)
    {
        divineCellTarget.transform.position = pos;
    }

    #endregion

    #region NOT_ENOUGH_NRJ

    [Header("NRJ")]
    public Animator nENRJ;

    public void DisplayNotEnoughNRJ()
    {
        nENRJ.Play("Show");
    }

    #endregion

    #region NOT ENOUGH LINK 
    [Header("NotEnoughLinks")]
    public Animator nELink;
    public void DisplayNotEnoughLink()
    {
        if (nELink.GetCurrentAnimatorStateInfo(0).IsName("invisible"))
        {
            nELink.Play("Show");
        }
    }

    #endregion

    #region ALERT

    [Header("CellExplosionAlert")]
    [Range(0, 1000)] public float alertRadius = 200;
    public Transform alertHolder;
    [Range(0, 20)] public float offsetPercentage;

    public void DisplayCellAlert(Transform transform, CellAlert alert)
    {

        if (alert.transform.parent != alertHolder)
            alert.transform.SetParent(alertHolder);

        RectTransform rect = alert.GetComponent<RectTransform>();
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 screenObjPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 dir = (screenObjPos - screenCenter).normalized;

        rect.anchoredPosition = new Vector2((dir.x * Screen.width / 2) - (Mathf.Sign(dir.x) * Screen.width * offsetPercentage), (dir.y * Screen.height / 2) - (Mathf.Sign(dir.y) * Screen.height * offsetPercentage));
    }


    #endregion

    #region PAUSE MENU 

    [Header ("PauseMenu")]
    public PauseMenu pauseMenu;

    #endregion

}

