using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InputManager : MonoBehaviour
{
    #region Variables
    public static InputManager Instance;

    public LayerMask maskLeftCLick;
    public LayerMask UIMask;

    //défini la distance avant d'activé le drag 
    public float distanceBeforeDrag;

    //variables pour l'interaction avec la cellule 
    public float DelayBetweenClick;
    public float clickCooldown;

    //bools concernant la pahse de dragging d'un lien 
    [HideInInspector] public bool CellSelected;
    [HideInInspector] public bool DraggingLink;

    //Ui
    [HideInInspector] public bool InCellSelection;
    [HideInInspector] public bool InPauseMenu;
    [HideInInspector] public bool InQuestEvent;

    //LayerMask
    int layer_Mask_Cell;

    [HideInInspector] public Vector3 posCell;
    [HideInInspector] public Vector3 mousePos;

    private float clickTime;
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
        layer_Mask_Cell = LayerMask.GetMask("Cell");
    }

    //permet d'éviter un getComponent à chaque frame lors du raycast de mouse Over
    private bool isOverCell;
    private bool rightClickedOnCell;
    private bool leftClickedOnCell;
    private CellMain cellOver;
    public CellMain selectedCell;

    public bool newCell = false;
    [HideInInspector] public bool movingObject = false;
    [HideInInspector] public CellMain objectMoved;

    private void Update()
    {
        RaycastHit hit = Helper.ReturnHit(Input.mousePosition, CellManager.mainCamera, maskLeftCLick);
        mousePos = hit.point;


        if (!movingObject)
        {
            #region LINKS, INTERACTIONS AND CELL_CREATIONS

            //En train de drag le lien 
            if (DraggingLink)
            {
                CellManager.Instance.DragNewlink(hit);

                if (Input.GetMouseButtonUp(0))
                {
                    CellManager.Instance.ValidateNewLink(hit);
                }

            }


            //Click Gauche In
            if (Input.GetMouseButtonDown(0))
            {

                if (hit.transform != null && hit.transform.tag == "Cell")
                {
                    leftClickedOnCell = true;
                    selectedCell = hit.transform.GetComponent<CellMain>();
                }



                if (!DraggingLink && !InCellSelection && isOverCell && !movingObject)
                {
                    CellManager.Instance.SelectCell(hit);
                }
            }

            //Click Gauche Maintient
            if (Input.GetMouseButton(0))
            {
                clickTime += Time.deltaTime;

                if (CellSelected && clickTime > clickCooldown && leftClickedOnCell && !InCellSelection && !DraggingLink)
                {

                    UIManager.Instance.DisplayCellShop(selectedCell);

                }
                if (selectedCell != null)
                {

                    float distanceFromCell = (hit.point - selectedCell.transform.position).magnitude;
                    if (clickTime > clickCooldown && leftClickedOnCell && !DraggingLink && distanceFromCell > distanceBeforeDrag)
                    {
                        UIManager.Instance.StartCoroutine(UIManager.Instance.DesactivateCellShop());
                        CellManager.Instance.CreatenewLink();
                        DraggingLink = true;
                    }
                }

            }


            //Click Gauche Out
            if (Input.GetMouseButtonUp(0))
            {
                //pour interagir avec la cellule
                if (clickTime <= clickCooldown && isOverCell && cellOver == CellManager.Instance.selectedCell)
                {
                    CellManager.Instance.InteractWithCell();

                }

                if (InCellSelection)
                {
                    UIManager.Instance.StartCoroutine(UIManager.Instance.DesactivateCellShop());
                }

                leftClickedOnCell = false;
                clickTime = 0;

            }

            //Click Droit In
            if (Input.GetMouseButtonDown(1))
            {
                if (InCellSelection)
                {
                    UIManager.Instance.DesactivateCellShop();

                }
                //Deselectionne la cellule si sélectionné 
                else if (CellSelected && DraggingLink)
                {
                    DesactivateLinkWhileDragging();
                }
                else if (CellSelected)
                {
                    CellManager.Instance.DeselectCell();
                }
            }

            #endregion

            #region MOUSEOVER_CELLS - TOOLTIP


            if (!DraggingLink && !InCellSelection)
            {

                if (hit.transform != null && hit.transform.tag == "Cell" && !isOverCell)
                {
                    isOverCell = true;
                    cellOver = hit.transform.GetComponent<CellMain>();
                }
            }

            if (hit.transform == null || hit.transform.tag != "Cell")
            {
                isOverCell = false;
                cellOver = null;
            }

            if (isOverCell)
            {
                UIManager.Instance.LoadToolTip(cellOver.transform.position, cellOver);
            }
            else
            {
                UIManager.Instance.UnloadToolTip();
                rightClickedOnCell = false;
            }


            #endregion

            #region CELL_OPTIONS

            if (isOverCell && Input.GetMouseButtonDown(1))
            {
                rightClickedOnCell = true;
            }

            if (rightClickedOnCell && Input.GetMouseButtonUp(1))
            {
                UIManager.Instance.DisplayCellOptions(cellOver);
            }

            if (!isOverCell && Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            {




                if (!UIManager.Instance.cellOptionsUI.mouseIsOver)
                {

                    if (UIManager.Instance.cellOptionsUI.gameObject.activeSelf)
                        UIManager.Instance.HideUI(UIManager.Instance.cellOptionsUI.gameObject);
                }
            }

            #endregion
        }

        #region MOVING_CELL
        else
        {




            if (hit.transform != null && hit.transform.tag == "Ground")
            {
                CellManager.Instance.CellDeplacement(hit.point, objectMoved, newCell);
            }

            //si clic gauche, replacer la cell et update tous ses liens
            if (Input.GetMouseButtonDown(0))
            {
                if (newCell)
                    CellManager.Instance.ValidateNewLink(hit);
                objectMoved.CellInitialisation();
                movingObject = false;
                objectMoved = null;
                DraggingLink = false;
            }

            else if (Input.GetMouseButtonDown(1))
            {
                if (CellManager.Instance.originalPosOfMovingCell == new Vector3(0, 100, 0))
                {
                    RessourceTracker.instance.energy += objectMoved.myCellTemplate.EnergyCost;
                    objectMoved.Died(true);
                    CellManager.Instance.SupressCurrentLink();
                }
                else
                {
                    CellManager.Instance.CellDeplacement(CellManager.Instance.originalPosOfMovingCell, objectMoved, false);
                    objectMoved.TickInscription();
                    CellManager.Instance.ValidateNewLink(hit);
                }

                movingObject = false;
                objectMoved = null;
                DraggingLink = false;

            }
        }
        #endregion

        #region CAMERA

        CameraController.instance.MoveCamera();

        if (Input.GetKey(KeyCode.LeftAlt))
            CameraController.instance.TiltCamera();
        else
            CameraController.instance.DecreaseTiltCount();

        #endregion

    }

    private void Start()
    {
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();
        p_Raycaster = FindObjectOfType<PhysicsRaycaster>();
    }

    PhysicsRaycaster p_Raycaster;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    public void DesactivateLinkWhileDragging()
    {
        DraggingLink = false;
        CellManager.Instance.SupressCurrentLink();
    }

    public void CleanBoolsRelatedToCell()
    {
        CellSelected = false;
        DraggingLink = false;
        InCellSelection = false;
    }

    public void StartMovingCell(CellMain cell, bool alreadyExistingCell)
    {

        if (!alreadyExistingCell)
            CellManager.Instance.originalPosOfMovingCell = new Vector3(0, 100, 0);
        else
            CellManager.Instance.originalPosOfMovingCell = cell.transform.position;

        if (CellManager.Instance.originalPosOfMovingCell == new Vector3(0, 100, 0))
        {
            newCell = true;
        }
        else
        {
            newCell = false;
        }


        cell.TickDesinscription();
        Instance.objectMoved = cell;
        Instance.movingObject = true;
        Instance.DraggingLink = true;
        Instance.InCellSelection = false;
    }
}
