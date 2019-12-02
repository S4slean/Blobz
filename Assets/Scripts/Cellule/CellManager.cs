using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public static CellManager Instance;
    public static Camera mainCamera;

    public int Energy;

    public Material allowedBuildingMat;
    public Material refusedBuildingMat;


    [Header("Debug")]
    public CellMain selectedCell;
    [SerializeField]
    private CellMain createdCell;
    [SerializeField]
    private LineRenderer currentLine;
    [SerializeField]
    private LinkClass currentLink;
    [SerializeField]
    private CellMain receivingCell;

    public Vector3 originalPosOfMovingCell;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        mainCamera = Camera.main;
    }


    public void CreatenewLink()
    {
        if (selectedCell.noMoreLink)
        {
            Debug.Log("Trop de lien sur la cellule de base");
            return;
        }
        //Referencing in script
        LinkClass newLink = ObjectPooler.poolingSystem.GetPooledObject<LinkClass>() as LinkClass;
        currentLink = newLink;
        currentLine = newLink.line;
        currentLink.Outpool();
        //Setup 
        InputManager.Instance.DraggingLink = true;
        //selectedCell.AddLink(currentLink, true);
        currentLink.startPos = selectedCell.transform.position;

        ///nouveau systeme de link
       // currentLink.FirstSetup(currentLink.startPos, InputManager.Instance.mousePos, selectedCell.GetCurrentRange());

        currentLine.SetPosition(0, currentLink.startPos);
        currentLine.SetPosition(1, InputManager.Instance.mousePos);
    }
    public void DragNewlink(RaycastHit hit)
    {
        // Permet de draw la line en runtime 
        float distance = Vector3.Distance(currentLink.startPos, hit.point);
        if (distance <= selectedCell.myCellTemplate.rangeBase / 2)
        {
            currentLink.endPos = new Vector3(hit.point.x, currentLink.startPos.y, hit.point.z);
            currentLine.SetPosition(1, currentLink.endPos);



            ///nouveau systeme de link
            //Vector3 lastPos = new Vector3(hit.point.x, currentLink.startPos.y, hit.point.z);
            //currentLink.UpdatePoint(lastPos);

        }
        else
        {
            Vector3 direction = (hit.point - currentLink.startPos);
            direction = new Vector3(direction.x, 0, direction.z);
            direction = direction.normalized;


            currentLink.endPos = currentLink.startPos + direction * selectedCell.myCellTemplate.rangeBase / 2;
            currentLine.SetPosition(1, currentLink.endPos);
        }
    }
    public void DragNewlink(Vector3 pos)
    {
        // Permet de draw la line en runtime 
        float distance = Vector3.Distance(currentLink.startPos, pos);
        if (distance <= selectedCell.myCellTemplate.rangeBase / 2)
        {
            currentLink.endPos = new Vector3(pos.x, currentLink.startPos.y, pos.z);
            currentLine.SetPosition(1, currentLink.endPos);
        }
        else
        {
            Vector3 direction = (pos - currentLink.startPos);
            direction = new Vector3(direction.x, 0, direction.z);
            direction = direction.normalized;
            currentLink.endPos = currentLink.startPos + direction * selectedCell.myCellTemplate.rangeBase / 2;
            currentLine.SetPosition(1, currentLink.endPos);
        }
    }
    public void ValidateNewLink(RaycastHit hit)
    {
        receivingCell = hit.transform.GetComponent<CellMain>();
        //permet de check si c'est bien une cellule et pas la meme cellule
        if (receivingCell != null && receivingCell != selectedCell)
        {
            bool check1 = false;
            bool check2 = false;
            //check si un lien entre les 2 celllules existe déja 
            //si il n'y pas de lien la boucle for ne se lance pas c'est pas grave
            for (int i = 0; i < selectedCell.links.Count; i++)
            {
                if (selectedCell.links[i].originalCell == receivingCell) check1 = true; else check1 = false;
                if (selectedCell.links[i].receivingCell == receivingCell) check2 = true; else check2 = false;
                if (check1 || check2)
                {
                    SupressCurrentLink();
                    return;
                }
            }
            if (receivingCell.noMoreLink)
            {
                SupressCurrentLink();
                return;
            }
            currentLine.startColor = Color.green;
            currentLine.endColor = Color.cyan;
            currentLink.endPos = receivingCell.transform.position;
            currentLine.SetPosition(1, currentLink.endPos);
            selectedCell.AddLink(currentLink, true);
            receivingCell.AddLink(currentLink, false);
            cleanLinkRef();


        }
        else
        {
            SupressCurrentLink();
        }


    }
    public void NewCellCreated(CellMain newCell)
    {
        createdCell = newCell;
        CreatenewLink();

        EnergyVariation(-newCell.myCellTemplate.EnergyCost);

        //selectedCell.AddLink(currentLink, true);
        //createdCell.AddLink(currentLink, false);
        currentLine.startColor = Color.green;
        currentLine.endColor = Color.cyan;

        //cleanLinkRef();
        UIManager.Instance.cellSelection.DesactiveButton();
        InputManager.Instance.StartMovingCell(newCell, false);

    }

    public void cleanLinkRef()
    {
        currentLink = null;
        currentLine = null;
        InputManager.Instance.DraggingLink = false;
    }
    public void SupressCurrentLink()
    {
        currentLink.Inpool();
        cleanLinkRef();
    }


    public void DeselectCell()
    {
        selectedCell = null;
        InputManager.Instance.CellSelected = false;
        UIManager.Instance.CellDeselected();
    }
    public void SelectCell(RaycastHit hit)
    {
        CellMain hitCell = hit.transform.GetComponent<CellMain>();
        if (hitCell != null)
        {
            if (hitCell != selectedCell)
            {
                selectedCell = hitCell;
                InputManager.Instance.CellSelected = true;
                InputManager.Instance.posCell = selectedCell.transform.position;
                UIManager.Instance.CellSelected(selectedCell.transform.position);

            }
        }
    }


    public void InteractWithCell()
    {
        selectedCell.ClickInteraction();
    }
    public void CellDeplacement(Vector3 posToTest, CellMain cellToMove, bool isNewCell)
    {
        bool shouldStop = false;
        //if (posToTest.y > 2)
        //    return;


        for (int i = 0; i < cellToMove.links.Count; i++)
        {
            if (cellToMove.links[i].CheckLength(posToTest) == false)
            {
                shouldStop = true;
            }
        }

        if (shouldStop)
        {

        }
        else
        {
            cellToMove.transform.position = posToTest;

            if (isNewCell)
                DragNewlink(posToTest);
            

            for (int i = 0; i < cellToMove.links.Count; i++)
            {
                cellToMove.links[i].UpdateLinks(cellToMove, posToTest);
            }


        }

    }


    public void EnergyVariation(int Variation)
    {
        RessourceTracker.instance.energy += Variation;
        UIManager.Instance.TopBar.UpdateUI();
    }
}
