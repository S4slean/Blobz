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
    [HideInInspector] public bool InCellShop;
    [HideInInspector] public bool InPauseMenu;
    [HideInInspector] public bool InQuestEvent;

    //LayerMask
    int layer_Mask_Cell;

    [HideInInspector] public Vector3 posCell;
    [HideInInspector] public Vector3 mouseWorldPos;
    [HideInInspector] public Vector3 mouseScreenPos;

    private float clickTime;


    //permet d'éviter un getComponent à chaque frame lors du raycast de mouse Over
    private bool isOverInteractiveElement;
    private bool rightClickedOnCell;
    private bool leftClickedOnCell;
    private PlayerAction elementOver;
    private PlayerAction selectedElement;
    public CellMain selectedCell;

    public bool newCell = false;
    [HideInInspector] public bool movingObject = false;
    [HideInInspector] public CellMain objectMoved;

    private RaycastHit CurrentHit;
    private PlayerAction currentPlayerAction;
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

    private void Start()
    {
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();
        p_Raycaster = FindObjectOfType<PhysicsRaycaster>();
    }

    private void Update()
    {
        CurrentHit = Helper.ReturnHit(Input.mousePosition, CellManager.mainCamera, maskLeftCLick);
        mouseWorldPos = CurrentHit.point;
        mouseScreenPos = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);

        #region STANDARD_STATE
        if (!movingObject)
        {
            #region LINKS, INTERACTIONS AND CELL_CREATIONS

            //En train de drag le lien 
            if (DraggingLink)
            {
                CellManager.Instance.DragNewlink(CurrentHit);

                if (Input.GetMouseButtonUp(0))
                {
                    CellManager.Instance.ValidateNewLink(CurrentHit);
                }

            }


            //Click Gauche In
            if (Input.GetMouseButtonDown(0))
            {
                currentPlayerAction = CurrentHit.transform.GetComponent<PlayerAction>(); //------------------------
                if (CurrentHit.transform != null && currentPlayerAction != null)
                {
                    currentPlayerAction.OnLeftClickDown();
                    SelectElement();
                }



                //if (!DraggingLink && !InCellShop && isOverCell && !movingObject)
                //{
                //    CellManager.Instance.SelectCell(CurrentHit);
                //}
            }

            //Click Gauche Maintient
            if (Input.GetMouseButton(0))
            {
                clickTime += Time.deltaTime;

                if (CellSelected && clickTime > clickCooldown && leftClickedOnCell && !InCellShop && !DraggingLink)
                {
                    UIManager.Instance.DisplayCellShop(selectedCell);
                }
                if (selectedCell != null)
                {

                    float distanceFromCell = (CurrentHit.point - selectedCell.transform.position).magnitude;
                    if (clickTime > clickCooldown && leftClickedOnCell && !DraggingLink && distanceFromCell > distanceBeforeDrag)
                    {
                        UIManager.Instance.StartCoroutine(UIManager.Instance.DesactivateCellShop());

                        DraggingLink = CellManager.Instance.CreatenewLink();
                        if (DraggingLink)
                        {
                            newCell = false;
                        }
                    }
                }

            }


            //Click Gauche Out
            if (Input.GetMouseButtonUp(0))
            {
                currentPlayerAction = CurrentHit.transform.GetComponent<PlayerAction>();

                //pour interagir avec la cellule
                if (clickTime <= clickCooldown && isOverInteractiveElement && elementOver == selectedElement)
                {
                    currentPlayerAction.OnShortLeftClickUp();
                }

                if (InCellShop)
                {
                    UIManager.Instance.StartCoroutine(UIManager.Instance.DesactivateCellShop());
                }

                leftClickedOnCell = false;
                clickTime = 0;
            }

            //Click Droit In
            if (Input.GetMouseButtonDown(1))
            {
                if (InCellShop)
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


            if (!DraggingLink && !InCellShop)
            {

                if (CurrentHit.transform != null && !isOverInteractiveElement && CurrentHit.transform.GetComponent<PlayerAction>() != null )
                {
                    isOverInteractiveElement = true;
                    elementOver = CurrentHit.transform.GetComponent<PlayerAction>();
                }
            }

            if (CurrentHit.transform == null || CurrentHit.transform.tag != "Cell")
            {
                isOverInteractiveElement = false;
                elementOver = null;
            }

            if (isOverInteractiveElement)
            {
                currentPlayerAction.OnmouseOver();
            }
            else
            {
                UIManager.Instance.UnloadToolTip();
                rightClickedOnCell = false;
            }


            #endregion

            #region CELL_OPTIONS

            if (isOverInteractiveElement && Input.GetMouseButtonDown(1))
            {
                rightClickedOnCell = true;
            }

            if (rightClickedOnCell && Input.GetMouseButtonUp(1))
            {
               // UIManager.Instance.DisplayCellOptions(elementOver);
            }

            if (!isOverInteractiveElement && Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            {




                if (!UIManager.Instance.cellOptionsUI.mouseIsOver)
                {

                    if (UIManager.Instance.cellOptionsUI.gameObject.activeSelf)
                        UIManager.Instance.HideUI(UIManager.Instance.cellOptionsUI.gameObject);
                }
            }

            #endregion
        }
        #endregion

        #region MOVING_CELL
        else
        {
            if (CurrentHit.transform != null && CurrentHit.transform.tag == "Ground")
            {
                CellManager.Instance.CellDeplacement(CurrentHit.point, objectMoved, newCell);
            }

            //si clic gauche, replacer la cell et update tous ses liens
            if (Input.GetMouseButtonDown(0))
            {
                if (CellManager.Instance.terrainIsBuildable)
                {

                    if (newCell)
                        CellManager.Instance.ValidateNewLink(CurrentHit);

                    objectMoved.CellInitialisation();
                    movingObject = false;
                    objectMoved = null;
                    DraggingLink = false;
                }
                else
                {
                    Debug.Log("You can't build there");
                }
            }

            else if (Input.GetMouseButtonDown(1))
            {
                if (CellManager.Instance.originalPosOfMovingCell == new Vector3(0, 100, 0))
                {
                    RessourceTracker.instance.energy += objectMoved.myCellTemplate.energyCost;
                    objectMoved.Inpool();
                    CellManager.Instance.SupressCurrentLink();
                }
                else
                {
                    CellManager.Instance.CellDeplacement(CellManager.Instance.originalPosOfMovingCell, objectMoved, false);
                    objectMoved.TickInscription();
                    CellManager.Instance.ValidateNewLink(CurrentHit);
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


    PhysicsRaycaster p_Raycaster;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;


    public void SelectCell()
    {
        leftClickedOnCell = true;
        selectedCell = CurrentHit.transform.GetComponent<CellMain>();
        UIManager.Instance.CellSelected(selectedCell.transform.position);
        CellManager.Instance.selectedCell = selectedCell;
    }

    public void DesactivateLinkWhileDragging()
    {
        DraggingLink = false;
        leftClickedOnCell = false;
        CellManager.Instance.SupressCurrentLink();
        CellManager.Instance.DeselectCell();
    }
    public void CleanBoolsRelatedToCell()
    {
        CellSelected = false;
        DraggingLink = false;
        InCellShop = false;
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
        Instance.InCellShop = false;
    }


    public void SelectElement()
    {
        selectedElement = currentPlayerAction;
    }

    public void StopCellActions()
    {
        UIManager.Instance.DesactivateCellShop();
        UIManager.Instance.CellDeselected();

        if (DraggingLink)
        {
            CellManager.Instance.SupressCurrentLink();
            DraggingLink = false;
        }
        if (movingObject)
        {
            CellManager.Instance.SupressCurrentLink();
            objectMoved.Inpool();
            movingObject = false;
        }

        CleanBoolsRelatedToCell();
    }
}
