using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSelectionShop : MonoBehaviour
{
    public GameObject[] buttonTypes;
    [TextArea]
    public string Important;
    [Space(10f)]
    [Header("TWEAKING")]
    [Range(0f, 5f)]
    public float buttonDistance = 2f;
    public float yOffset = 1;

    [SerializeField]
    private RectTransform[] butTrans;

    private void Awake()
    {

        butTrans = new RectTransform[buttonTypes.Length];
        DesactiveButton();
        for (int i = 0; i < buttonTypes.Length; i++)
        {
            butTrans[i] = buttonTypes[i].GetComponent<RectTransform>();
        }
    }

    public void DesactiveButton()
    {
        foreach (GameObject button in buttonTypes)
        {
            button.SetActive(false);
        }
    }

    public void ButtonPositions(CellMain inputCell)
    {
        GameObject[] select = inputCell.myCellTemplate.cellsEnableToBuild;
        if (select.Length == 0)
        {
            Debug.Log("IL FAUT RAJOUTER LE CHECK POUR LE POSSIBILITE DE BUILD");
            return;
        }
        float anglefrac = 2 * Mathf.PI / select.Length;
        for (int i = 0; i < select.Length; i++)
        {
            //calcule de l'angle en foncttion du nombre de point
            float angle = anglefrac * i;
            Vector3 dir = new Vector3(Mathf.Sin(angle), yOffset, Mathf.Cos(angle));
           // Vector3 dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) - transform.position;
           // Vector3 pos = transform.position + (dir * buttonDistance) + new Vector3 (0 , yOffset , 0 );
            Vector3 pos = dir * buttonDistance;
            CellType actualType = select[i].GetComponent<CellMain>().myCellTemplate.type;

            ButtonChoosen(pos, actualType);

        }
    }

    private void ButtonChoosen(Vector3 pos, CellType cellType)
    {
        GameObject currentButton;

        for (int i = 0; i < buttonTypes.Length; i++)
        {
            if (cellType.ToString() == buttonTypes[i].name.Replace("Button", ""))
            {
                currentButton = buttonTypes[i];
                butTrans[i].transform.localPosition = pos;
                currentButton.SetActive(true);
                break;
            }
        }

    }

    public void CellConstruction(CellMain cellule)
    {


        System.Type cellType = cellule.GetType();
        CellMain newCell = ObjectPooler.poolingSystem.GetPooledObject(cellType) as CellMain;
        if (newCell.myCellTemplate.EnergyCost > RessourceTracker.instance.energy)
        {
            Debug.Log("pas assez d'énergie");
            UIManager.Instance.DisplayNotEnoughNRJ();
            newCell = null;
        }
        else
        {
            newCell.transform.position = InputManager.Instance.mousePos; ;           
            newCell.Outpool();
            CellManager.Instance.NewCellCreated(newCell);

        }
    }
}
