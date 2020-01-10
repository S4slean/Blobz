using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSelectionShop : MonoBehaviour
{
    public GameObject[] sections;
    public Animator[] anims;







    public void CellConstruction(CellMain cellule)
    {
        Debug.Log("build");

        System.Type cellType = cellule.GetType();
        if (cellule.myCellTemplate.energyCost > RessourceTracker.instance.energy)
        {
            UIManager.Instance.DisplayNotEnoughNRJ();
            CellManager.Instance.SetIfNewCell(false);
        }
        else
        {
            HideSections();
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
        Debug.Log("displaySections");
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].GetComponent<RectTransform>().ForceUpdateRectTransforms();
            anims[i].Play("DisplaySection");
        }
    }

    public void HideOtherSubMenus(int i)
    {
        for (int j = 0; j < sections.Length; j++)
        {
            if (j == i)
                continue;
            anims[j].Play("HideSubMenu");
        }
    }

    public void HideSections()
    {
        Debug.Log("close All");
        for (int i = 0; i < sections.Length; i++)
        {
            anims[i].SetTrigger("close");
        }
    }

    public void DisplaySubMenu(int index)
    {
        anims[index].Play("DisplaySubMenu");
        HideOtherSubMenus(index);
    }


}


