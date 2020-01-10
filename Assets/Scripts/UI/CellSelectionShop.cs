using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSelectionShop : MonoBehaviour
{
    public GameObject[] sections;
    public GameObject[] cells;

    [SerializeField]
    private RectTransform[] butTrans;




    public void CellConstruction(CellMain cellule)
    {
        

        System.Type cellType = cellule.GetType();
        if (cellule.myCellTemplate.energyCost > RessourceTracker.instance.energy)
        {
            UIManager.Instance.DisplayNotEnoughNRJ();
            CellManager.Instance.SetIfNewCell(false);
        }
        else
        {
            CellManager.Instance.SetIfNewCell(true);

            CellMain newCell = ObjectPooler.poolingSystem.GetPooledObject(cellType) as CellMain;
            newCell.transform.position = InputManager.Instance.mouseWorldPos;         
            newCell.Outpool();

            CellManager.Instance.NewCellCreated(newCell);
            newCell.GenerateLinkSlot();

        }
    }


    public void DisplaySections()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].SetActive(true);
        }
    }

    public void HideSections()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].SetActive(false);
        }
    }

    public void DisplaySubMenu(int index)
    {
        sections[index].GetComponent<Animator>().Play("DisplaySubMenu");
    }
}


