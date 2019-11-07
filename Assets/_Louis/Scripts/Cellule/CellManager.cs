using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public static CellManager Instance;
    public static Camera mainCamera;

    public int Energy;

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


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        currentLine.SetPosition(0, currentLink.startPos);
    }
    public void DragNewlink(RaycastHit hit)
    {
        // Permet de draw la line en runtime 
        float distance = Vector3.Distance(currentLink.startPos, hit.point);
        if (distance <= selectedCell.myCellTemplate.range / 2)
        {
            currentLink.endPos = new Vector3(hit.point.x, currentLink.startPos.y, hit.point.z);
            currentLine.SetPosition(1, currentLink.endPos);
        }
        else
        {
            Vector3 direction = (hit.point - currentLink.startPos);
            direction = new Vector3(direction.x, 0, direction.z);
            direction = direction.normalized;
            currentLink.endPos = currentLink.startPos + direction * selectedCell.myCellTemplate.range / 2;
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
                if (selectedCell.links[i].receveingCell == receivingCell) check2 = true; else check2 = false;
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
            UIManager.Instance.InUICellSelection(currentLink.endPos, selectedCell, currentLine);
        }


    }
    public void NewCellCreated(CellMain newCell)
    {
        createdCell = newCell;

        EnergyVariation(-newCell.myCellTemplate.EnergyCost);

        selectedCell.AddLink(currentLink, true);
        createdCell.AddLink(currentLink, false);
        currentLine.startColor = Color.green;
        currentLine.endColor = Color.cyan;

        cleanLinkRef();
        UIManager.Instance.cellSelection.DesactiveButton();
        InputManager.Instance.DraggingLink = false;
        InputManager.Instance.InCellSelection = false;

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
        Debug.Log(hit.transform.name);
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
            else
            {
                InputManager.Instance.clickCooldown = Time.time + InputManager.Instance.DelayBetweenClick;
            }
        }
    }
    public void InteractWithCell()
    {
        selectedCell.ClickInteraction();
    }

    public void EnergyVariation(int Variation)
    {
        RessourceTracker.instance.energy += Variation;
        UIManager.Instance.topBar.UpdateUI();
    }
}
