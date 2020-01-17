using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    //public Animator anim;
    public TextMeshProUGUI cellName;
    public TextMeshProUGUI cellDescription;
    public TextMeshProUGUI effects;
    public TextMeshProUGUI cellCost;
    public GameObject secondDisplay;


    public void UpdateUI(CellMain cell, bool displayCost)
    {
        cellCost.gameObject.SetActive(displayCost);
        cellName.text = cell.myCellTemplate.name;
        cellDescription.text = "This cell produces " + cell.myCellTemplate.prodPerTickBase + " blobs per tick";
        cellCost.text = "Cost: " + cell.myCellTemplate.energyCost;


        effects.text = "Effects are not implemented yet";
    }

    public void HideTooltipUI()
    {
        UIManager.Instance.HideTooltip();
    }
}
