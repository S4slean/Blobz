using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSelectionShop : MonoBehaviour
{
    public Button[] buttonTypes;
    [TextArea]
    public string Important;
    [Space(10f)]
    [Header("TWEAKING")]
    [Range(0f, 5f)]
    public float buttonDistance = 2f;

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
        foreach (Button button in buttonTypes)
        {
            button.gameObject.SetActive(false);
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
            Vector3 dir = new Vector3(Mathf.Sin(angle), 1, Mathf.Cos(angle));
            Vector3 pos = dir * buttonDistance;
            CellType actualType = select[i].GetComponent<CellMain>().myCellTemplate.type;

            ButtonChoosen(pos, actualType);

        }
    }

    private void ButtonChoosen(Vector3 pos, CellType cellType)
    {
        Button currentButton = buttonTypes[(int)cellType];
        butTrans[(int)cellType].transform.localPosition = pos;
        currentButton.gameObject.SetActive(true);
    }

    public void CellConstruction(CellMain cellule)
    {
        System.Type cellType = cellule.GetType();
        CellMain newCell = ObjectPooler.poolingSystem.GetPooledObject(cellType) as CellMain;
        if (newCell.myCellTemplate.EnergyCost > RessourceTracker.instance.energy)
        {
            Debug.Log("pas assez d'énergie");
            newCell = null;
        }
        else
        {
            newCell.transform.position = transform.position;           
            newCell.Outpool();
            CellManager.Instance.NewCellCreated(newCell);
        }
    }
}
