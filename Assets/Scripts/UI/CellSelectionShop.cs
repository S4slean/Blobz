using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSelectionShop : MonoBehaviour
{
    public CellButtonsShop[] sections;
    public Animator[] anims;

    public static CellSelectionShop instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

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
            anims[i].SetInteger("opening", 1);
        }
    }

    public void HideOtherSubMenus(int i)
    {
        for (int j = 0; j < sections.Length; j++)
        {
            if (j == i)
                continue;
            anims[j].SetInteger("opening", 1);
        }
    }

    public void HideSections()
    {
        
        Debug.Log("close All");
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].UndetectMouse();
            anims[i].SetInteger("opening", 0);
        }
    }

    public void DisplaySubMenu(int index)
    {
        if (sections[index].detectMouse)
        {

            Debug.Log("display SubMenu " + index);
            anims[index].SetInteger("opening", 2) ;
            HideOtherSubMenus(index);
        }
    }


}


