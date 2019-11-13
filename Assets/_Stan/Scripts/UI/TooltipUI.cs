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
    public GameObject secondDisplay;


    public void UpdateUI(CellMain cell)
    {
        cellName.text = cell.myCellTemplate.name;
        cellDescription.text = "This cell produces " + cell.myCellTemplate.prodPerTick + " blobs per tick";

        effects.text = "Effects are not implemented yet";
    }

    public void HideTooltipUI()
    {
        UIManager.Instance.HideTooltip();
    }
}
