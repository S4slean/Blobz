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
                        energySubMenus[l].SetBool("Open", false);
                    }

                    break;
                case 1:
                    for (int l = 0; l < exploSubMenus.Length; l++)
                    {
                        exploSubMenus[l].SetBool("Open", false);
                    }

                    break;
                case 2:
                    for (int l = 0; l < gestionSubMenus.Length; l++)
                    {
                        gestionSubMenus[l].SetBool("Open", false);
                    }

                    break;
                case 3:
                    for (int l = 0; l < combatSubMenus.Length; l++)
                    {
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
        if (sections[index].detectMouse)
        {


            
            switch (index)
            {
                case 0:
                    for (int i = 0; i < energySubMenus.Length; i++)
                    {
                        energySubMenus[i].SetBool("Open", true);
                    }
                    break;
                case 1:
                    for (int i = 0; i < energySubMenus.Length; i++)
                    {
                        exploSubMenus[i].SetBool("Open", true);
                    }
                    break;
                case 2:
                    for (int i = 0; i < energySubMenus.Length; i++)
                    {
                        gestionSubMenus[i].SetBool("Open", true);
                    }
                    break;
                case 3:
                    for (int i = 0; i < energySubMenus.Length; i++)
                    {
                        combatSubMenus[i].SetBool("Open", true);
                    }
                    break;
            }

            HideOtherSubMenus(index);
        }
    }


}


