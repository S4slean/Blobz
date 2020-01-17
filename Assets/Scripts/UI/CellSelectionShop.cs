using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSelectionShop : MonoBehaviour
{
    public CellButtonsShop[] sections;
    public Animator[] anims;

    public Animator[] energySubMenus;
    public Animator[] exploSubMenus;
    public Animator[] gestionSubMenus;
    public Animator[] combatSubMenus;

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

        System.Type cellType = cellule.GetType();
        if (cellule.myCellTemplate.energyCost > RessourceTracker.instance.energy)
        {
            UIManager.Instance.DisplayNotEnoughNRJ();
            //CellManager.Instance.SetIfNewCell(false);
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
        for (int i = 0; i < sections.Length; i++)
        {
            anims[i].SetBool("Show", true);
        }
    }



    public void HideOtherSubMenus(int i)
    {
        for (int j = 0; j < sections.Length; j++)
        {
            if (j == i)
            {

                continue;
            }

            switch (j)
            {
                case 0:
                    for (int l = 0; l < energySubMenus.Length; l++)
                    {
                        if (energySubMenus[l].gameObject.activeSelf)
                            energySubMenus[l].SetBool("Open", false);
                    }

                    break;
                case 1:
                    for (int l = 0; l < exploSubMenus.Length; l++)
                    {
                        if (exploSubMenus[l].gameObject.activeSelf)
                            exploSubMenus[l].SetBool("Open", false);
                    }

                    break;
                case 2:
                    for (int l = 0; l < gestionSubMenus.Length; l++)
                    {
                        if (gestionSubMenus[l].gameObject.activeSelf)
                            gestionSubMenus[l].SetBool("Open", false);
                    }

                    break;
                case 3:
                    for (int l = 0; l < combatSubMenus.Length; l++)
                    {
                        if (combatSubMenus[l].gameObject.activeSelf)
                            combatSubMenus[l].SetBool("Open", false);
                    }

                    break;
            }
        }
    }

    public void HideSections()
    {


        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].UndetectMouse();
            HideOtherSubMenus(5);
            anims[i].SetBool("Show", false);
        }
    }

    public void DisplaySubMenu(int index)
    {

        switch (index)
        {
            case 0:
                for (int i = 0; i < energySubMenus.Length; i++)
                {
                    if (energySubMenus[i].gameObject.activeSelf)
                        energySubMenus[i].SetBool("Open", true);
                }
                break;
            case 1:
                for (int i = 0; i < energySubMenus.Length; i++)
                {
                    if (exploSubMenus[i].gameObject.activeSelf)
                        exploSubMenus[i].SetBool("Open", true);
                }
                break;
            case 2:
                for (int i = 0; i < energySubMenus.Length; i++)
                {
                    if (gestionSubMenus[i].gameObject.activeSelf)
                        gestionSubMenus[i].SetBool("Open", true);
                }
                break;
            case 3:
                for (int i = 0; i < energySubMenus.Length; i++)
                {
                    if (combatSubMenus[i].gameObject.activeSelf)
                        combatSubMenus[i].SetBool("Open", true);
                }
                break;
        }

        HideOtherSubMenus(index);
    }
}





