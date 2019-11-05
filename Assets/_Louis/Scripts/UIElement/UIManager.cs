using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    [Header("REF A FAIRE")]

    public CellSelectionShop cellSelection;
    public GameObject SelectedCellUI;
    public QuestUI QuestUI;
    public TopBarUI TopBar;

    public TextMeshProUGUI Energy;


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

    #region CELL_SELCTION
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

    public void UpdateEnergy()
    {
        Energy.text = CellManager.Instance.Energy.ToString();
    }

}