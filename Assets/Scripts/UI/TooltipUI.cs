using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    //public Animator anim;
    public TextMeshProUGUI cellName;
    public TextMeshProUGUI cellDescription;
    public TextMeshProUGUI proximityEffects;
    public TextMeshProUGUI clicEffects;
    public TextMeshProUGUI proximityLvl;
    public TextMeshProUGUI cellCost;
    public GameObject secondDisplay;


    public void UpdateUI(CellMain cell, bool displayCost)
    {
        cellCost.gameObject.SetActive(displayCost);
        cellName.text = cell.myCellTemplate.name;
        cellDescription.text = cell.myCellTemplate.description;
        cellCost.text = "Cost: " + cell.myCellTemplate.energyCost;


        if (displayCost)
        {
            //ADD CellTemplates Proximity Text
            //ADD Pro 
        }
        else
        {

        }


        if(cell.myCellTemplate.energyCost > RessourceTracker.instance.energy)
        {
            cellCost.color = Color.red;
        }
        else
        {
            cellCost.color = Color.white;
        }


        proximityEffects.text = "Effects are not implemented yet";
    }

    public void HideTooltipUI()
    {
        UIManager.Instance.HideTooltip();
    }
}
