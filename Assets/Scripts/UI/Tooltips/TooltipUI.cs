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
        clicEffects.text = cell.myCellTemplate.descriptionClick;
        proximityLvl.text = "Tier: " + cell.GetProximityTier().ToString();
        proximityEffects.text = cell.myCellTemplate.descriptionProximity;



        


        if(cell.myCellTemplate.energyCost > RessourceTracker.instance.energy)
        {
            cellCost.color = Color.red;
        }
        else
        {
            cellCost.color = Color.white;
        }
    }

    public void HideTooltipUI()
    {
        UIManager.Instance.HideTooltip();
    }
}
