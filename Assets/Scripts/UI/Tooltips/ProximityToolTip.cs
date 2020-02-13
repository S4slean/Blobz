using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProximityToolTip : MonoBehaviour
{
    public TextMeshProUGUI description;
    public Animator anim;


    


    public void UpdateInfo(CellMain cell)
    {
        switch (cell.myCellTemplate.type)
        {
            case CellType.Academy:
                break;

            case CellType.Accelerator:
                break;

            case CellType.AerialStrike:
                break;

            case CellType.Battery:
                break;

            case CellType.BlipBlop:
                break;

            case CellType.Crusher:
                break;

            case CellType.Dump:
                break;

            case CellType.Stock:
                break;

            case CellType.Treblobchet:
                break;

            case CellType.Turret:
                break;

                
        }
    }

}
