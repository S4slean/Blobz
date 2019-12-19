using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    #region VARIABLES
    public static CellManager Instance;
    public static Camera mainCamera;

    public int Energy;

    [Header("Placement Materials")]
    public Material allowedBuildingMat;
    public Material allowedBuildingSpriteMat;
    public Material refusedBuildingMat;
    public Material refusedBuldingSpriteMask;

    bool shouldStop;
    [HideInInspector] public bool terrainIsBuildable = false;
    bool terrainWasBuildable = false;


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
    #endregion

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

    #region LINK GESTION
    public bool CreatenewLink()
    {
        selectedCell = InputManager.Instance.selectedCell;
        LinkJointClass joint = selectedCell.CheckForAvailableJointOfType(linkJointType.output);

        if (joint == null)
        {
            Debug.Log("Plus assez d'output");
            return false;
        }
        else
        {
            joint.disponible = false;
            joint.typeOfJoint = linkJointType.output;
            joint.GraphUpdate();

            LinkClass _newLink = ObjectPooler.poolingSystem.GetPooledObject<LinkClass>() as LinkClass;
            currentLink = _newLink;
            currentLine = _newLink.line;
            currentLink.Outpool();

            Vector3 _dir = (InputManager.Instance.mouseWorldPos - selectedCell.transform.position).normalized;
            Vector3 firstPos = selectedCell.transform.position + _dir * selectedCell.myCellTemplate.slotDistance;

            currentLink.FirstSetupWithSlot(firstPos, InputManager.Instance.mouseWorldPos, selectedCell.GetCurrentRange(), joint/*, joint.isOutput*/);
            return true;
        }
    }
    //Sans Cell
    public void DragNewlink(RaycastHit hit)
    {
        Vector3 direction = (hit.point - selectedCell.transform.position);
        direction = new Vector3(direction.x, 0, direction.z);
        direction = direction.normalized;

        // Permet de draw la line en runtime 
        float distance = Vector3.Distance(selectedCell.transform.position, hit.point);
        if (distance <= (selectedCell.GetCurrentRange() + selectedCell.myCellTemplate.slotDistance))
        {
            Vector3 lastPos = new Vector3(hit.point.x, currentLink.extremityPos[0].y, hit.point.z);
            Vector3 firstPos = selectedCell.transform.position + direction * selectedCell.myCellTemplate.slotDistance;
            currentLink.UpdatePoint(firstPos, lastPos);

        }
        else
        {

            Vector3 firstPos = selectedCell.transform.position + direction * selectedCell.myCellTemplate.slotDistance;
            Vector3 lastPos = selectedCell.transform.position + (direction * selectedCell.GetCurrentRange() + direction * selectedCell.myCellTemplate.slotDistance);
            currentLink.UpdatePoint(firstPos, lastPos);
        }
    }

    //Avec Cell
    public void DragNewlink(Vector3 pos, CellMain cellMoved)
    {
        Vector3 direction = (pos - selectedCell.transform.position);
        direction = new Vector3(direction.x, 0, direction.z);
        direction = direction.normalized;

        // Permet de draw la line en runtime 
        float distance = Vector3.Distance(selectedCell.transform.position, pos);
        if (distance <= (selectedCell.GetCurrentRange() + selectedCell.myCellTemplate.slotDistance + cellMoved.myCellTemplate.slotDistance))
        {
            Vector3 firstPos = selectedCell.transform.position + direction * selectedCell.myCellTemplate.slotDistance;

            Vector3 lastPos = new Vector3(pos.x, currentLink.extremityPos[0].y, pos.z) - (direction * cellMoved.myCellTemplate.slotDistance);
            currentLink.UpdatePoint(firstPos, lastPos);
        }
        else
        {
            Vector3 lastPos = selectedCell.transform.position + (direction * selectedCell.GetCurrentRange() + direction * selectedCell.myCellTemplate.slotDistance);
            Vector3 firstPos = selectedCell.transform.position + direction * selectedCell.myCellTemplate.slotDistance;
            currentLink.UpdatePoint(firstPos, lastPos);
        }
    }

    public void ValidateNewLink(RaycastHit hit)
    {

        //Nouveau Cellules en cours de placement 
        if (InputManager.Instance.newCell)
            receivingCell = InputManager.Instance.objectMoved;
        else
            receivingCell = hit.transform.GetComponent<CellMain>();



        //permet de check si c'est bien une cellule et pas la meme cellule
        selectedCell = InputManager.Instance.selectedCell;


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
                //if (originalPosOfMovingCell != new Vector3(0, 100, 0))
                //{
                //}
                if (check1 || check2)
                {
                    SupressCurrentLink();
                    return;
                }
            }
            if (!currentLink.CheckNewLinkLength(receivingCell.transform.position, selectedCell, receivingCell))
            {
                Debug.Log("Coucou c moi ki fé chié Old " + receivingCell.name);
                SupressCurrentLink();
                return;
            }


            LinkJointClass cellJoint = receivingCell.CheckForAvailableJointOfType(linkJointType.input);
            if (cellJoint == null)
            {
                Debug.Log("Plus assez de lien input");
                SupressCurrentLink();
                return;
            }
            else
            {
                currentLink.joints[1].Inpool();
                currentLink.joints[1] = cellJoint;

            }

            currentLink.GetInputSlot(receivingCell);

            currentLine.startColor = Color.green;
            currentLine.endColor = Color.cyan;


            Vector3 cellPos = receivingCell.transform.position;
            Vector3 _dir = (cellPos - selectedCell.transform.position).normalized;
            Vector3 lastPos = cellPos - _dir * receivingCell.myCellTemplate.slotDistance;
            Vector3 firstPos = selectedCell.transform.position + _dir * selectedCell.myCellTemplate.slotDistance;
            currentLink.UpdatePoint(firstPos, lastPos);


            selectedCell.AddLinkReferenceToCell(currentLink, true);
            receivingCell.AddLinkReferenceToCell(currentLink, false);
            cleanLinkRef();

        }
        else
        {
            SupressCurrentLink();
        }



    }



    public void cleanLinkRef()
    {
        currentLink = null;
        currentLine = null;
        InputManager.Instance.DraggingLink = false;
    }
    public void SupressCurrentLink()
    {
        currentLink.joints[0].disponible = true;
        selectedCell.jointReset(currentLink.joints[0]);
        currentLink.joints[1].Inpool();
        currentLink.Inpool();
        cleanLinkRef();
    }
    #endregion

    #region CELL GESTION

    public void NewCellCreated(CellMain newCell)
    {
        createdCell = newCell;

        newCell.ChangeDeplacementMat(false);
        terrainIsBuildable = false;
        terrainWasBuildable = true;

        CreatenewLink();

        EnergyVariation(-newCell.myCellTemplate.energyCost);

        currentLine.startColor = Color.green;
        currentLine.endColor = Color.cyan;

        originalPosOfMovingCell = new Vector3(0, 100, 0);
        UIManager.Instance.cellSelection.DesactiveButton();
        InputManager.Instance.StartMovingCell(newCell, false);

    }
    public void DeselectCell()
    {
        selectedCell = null;
        InputManager.Instance.CellSelected = false;
        UIManager.Instance.CellDeselected();
    }
    //public void SelectCell(RaycastHit hit)
    //{
    //    CellMain hitCell = hit.transform.GetComponent<CellMain>();
    //    if (hitCell != null)
    //    {
    //        if (hitCell != selectedCell)
    //        {
    //            selectedCell = hitCell;
    //            InputManager.Instance.CellSelected = true;
    //            InputManager.Instance.posCell = selectedCell.transform.position;
    //            UIManager.Instance.CellSelected(selectedCell.transform.position);

    //        }
    //    }
    //}


    public void InteractWithCell()
    {
        selectedCell.OnShortLeftClickUp();
    }

    public void CellDeplacement(Vector3 posToTest, CellMain cellToMove, bool isNewCell)
    {
        shouldStop = false;

        if (currentLink.CheckNewLinkLength(posToTest, selectedCell, cellToMove) == false)
        {
            shouldStop = true;
        }

        terrainIsBuildable = Helper.CheckAvailableSpace(cellToMove.transform.position, cellToMove.myCellTemplate.slotDistance, cellToMove.ownCollider);

        if (terrainIsBuildable && !terrainWasBuildable)
        {
            cellToMove.ChangeDeplacementMat(true);
        }
        else if (!terrainIsBuildable && terrainWasBuildable)
        {
            cellToMove.ChangeDeplacementMat(false);
        }


        for (int i = 0; i < cellToMove.links.Count; i++)
        {
            shouldStop = !cellToMove.links[i].CheckLength(posToTest);
        }

        if (shouldStop)
        {
            Vector3 refPoint;
            if (originalPosOfMovingCell == new Vector3(0, 100, 0))
            {
                refPoint = selectedCell.transform.position;
                cellToMove.transform.position = refPoint + (InputManager.Instance.mouseWorldPos - refPoint).normalized * (selectedCell.GetCurrentRange() + selectedCell.myCellTemplate.slotDistance + cellToMove.myCellTemplate.slotDistance);
                DragNewlink(cellToMove.transform.position /*+ Vector3.up * .1f */, cellToMove);
            }
            else
            {
                refPoint = originalPosOfMovingCell;
            }


        }
        else
        {
            cellToMove.transform.position = posToTest;

            if (isNewCell)
                DragNewlink(posToTest, cellToMove);

        }

        for (int i = 0; i < cellToMove.links.Count; i++)
        {
            //A REMPLACER PAR updatePoints
            cellToMove.links[i].UpdateLinks(cellToMove, posToTest);
        }

        terrainWasBuildable = terrainIsBuildable;
    }

    #endregion

    #region UTILITAIRE

    public void EnergyVariation(int Variation)
    {
        RessourceTracker.instance.energy += Variation;
        UIManager.Instance.TopBar.UpdateUI();
    }
    #endregion

}
