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
    [HideInInspector] public bool dragging;

    //Ui
    [HideInInspector] public bool holdingLeftClick;
    [HideInInspector] public bool InPauseMenu;
    [HideInInspector] public bool InQuestEvent;

    //LayerMask
    int layer_Mask_Cell;

    [HideInInspector] public Vector3 posCell;
    [HideInInspector] public Vector3 mouseWorldPos;
    [HideInInspector] public Vector3 mouseScreenPos;

    private float clickTime;
    public enum InputMode {normal, movingCell, divineShot };
    private InputMode inputMode = InputMode.normal;


    //permet d'éviter un getComponent à chaque frame lors du raycast de mouse Over
    private bool isOverInteractiveElement;
    private bool rightClickedOnCell;
    private PlayerAction elementOver;
    public PlayerAction selectedElement;
    public CellMain selectedCell;


    [HideInInspector] public CellMain objectMoved;
    [HideInInspector] public CellDivine shootingCell;

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

        switch (inputMode)
        {
            #region STANDARD_STATE
            case InputMode.normal:

                #region LINKS, INTERACTIONS AND CELL_CREATIONS

                //En train de drag le lien 
                if (dragging)
                {

                    selectedElement.OnLeftDrag(CurrentHit);


                    if (Input.GetMouseButtonUp(0))
                    {
                        selectedElement.OnDragEnd(CurrentHit);
                        dragging = false;
                    }

                }


                //Click Gauche In
                if (Input.GetMouseButtonDown(0))
                {
                    if (CurrentHit.transform != null)
                    {

                        currentPlayerAction = CurrentHit.transform.GetComponent<PlayerAction>(); //------------------------
                    }

                    if (currentPlayerAction != null)
                    {
                        SelectElement();
                        selectedElement.OnLeftClickDown(CurrentHit);
                        selectedElement.OnSelect();
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

                    if (selectedElement != null && clickTime > clickCooldown && !holdingLeftClick && !dragging)
                    {

                        selectedElement.OnLeftClickHolding(CurrentHit);
                        holdingLeftClick = true;

                    }
                    if (selectedElement != null)
                    {
                        

                        float distanceFromElement = (CurrentHit.point - ((Component)selectedElement).transform.position).magnitude;

                        if (clickTime > clickCooldown && selectedElement != null && !dragging && distanceFromElement > distanceBeforeDrag)
                        {
                            selectedElement.OnDragStart(CurrentHit);
                            dragging = true;
                        }
                    }
                }


                //Click Gauche Out
                if (Input.GetMouseButtonUp(0))
                {
                    //pour interagir avec la cellule
                    if (clickTime <= clickCooldown && isOverInteractiveElement && elementOver == selectedElement)
                    {
                        selectedElement.OnShortLeftClickUp(CurrentHit);
                    }

                    if (holdingLeftClick)
                    {

                        selectedElement.OnLongLeftClickUp(CurrentHit);
                        holdingLeftClick = false;
                    }

                    DeselectElement();
                    clickTime = 0;
                }

                //Click Droit In
                if (Input.GetMouseButtonDown(1))
                {
                    if (holdingLeftClick)
                    {
                        selectedElement.OnRightClickWhileHolding(CurrentHit);
                        holdingLeftClick = false;
                    }
                    else if (dragging)
                    {
                        selectedElement.OnRightClickWhileDragging(CurrentHit);
                        dragging = false;
                    }
                    else if (selectedElement != null)
                    {
                        CellManager.Instance.DeselectElement();
                    }
                }

                #endregion

                #region MOUSEOVER_CELLS - TOOLTIP

                //PERMET DE NE PAS AFFICHE LE TOOLTIP DANS CES DEUX CONDITIONS
                if (!dragging && !holdingLeftClick)
                {

                    if (CurrentHit.transform != null && !isOverInteractiveElement)
                    {
                        if (CurrentHit.transform.GetComponent<PlayerAction>() != null)
                        {
                            
                            isOverInteractiveElement = true;
                            elementOver = CurrentHit.transform.GetComponent<PlayerAction>();
                            elementOver.OnmouseIn(CurrentHit);

                        }

                    }
                }

                if (CurrentHit.transform != null && isOverInteractiveElement)
                {
                    if (CurrentHit.transform.GetComponent<PlayerAction>() != elementOver)
                    {
                        isOverInteractiveElement = false;
                        elementOver.OnMouseOut(CurrentHit);
                        elementOver = null;
                    }
                }

                else if (CurrentHit.transform == null && isOverInteractiveElement)
                {
                    isOverInteractiveElement = false;
                    elementOver = null;
                    selectedElement.OnMouseOut(CurrentHit);
                }


                #endregion

                #region CELL_OPTIONS

                if (isOverInteractiveElement && Input.GetMouseButtonDown(1))
                {
                    rightClickedOnCell = true;
                }

                if (rightClickedOnCell && Input.GetMouseButtonUp(1))
                {
                    //elementOver.OnShortRightClick(CurrentHit);
                }

                if (!isOverInteractiveElement && (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)))
                {
                    if (selectedElement != null)
                        selectedElement.StopAction();
                }

                #endregion

                break;
            #endregion

            #region MOVING_STATE
            case InputMode.movingCell:

                
                if (CurrentHit.transform != null && CurrentHit.transform.tag == "Ground")
                {
                    CellManager.Instance.CellDeplacement(CurrentHit.point, objectMoved);
                }

                //si clic gauche, replacer la cell et update tous ses liens
                if (Input.GetMouseButtonDown(0))
                {
                    if (CellManager.Instance.terrainIsBuildable)
                    {

                        if (CellManager.Instance.newCell)
                            CellManager.Instance.ValidateNewLink(CurrentHit);

                        objectMoved.CellInitialisation();
                        SwitchInputMode(InputMode.normal);
                        objectMoved = null;
                        //dragging = false;

                        ResetInputs();
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
                        CellManager.Instance.CellDeplacement(CellManager.Instance.originalPosOfMovingCell, objectMoved);
                        objectMoved.TickInscription();
                        CellManager.Instance.ValidateNewLink(CurrentHit);
                    }

                    SwitchInputMode(InputMode.normal);
                    objectMoved = null;
                    //dragging = false;

                }

                break;
            #endregion

            #region SHOOTING_STATE
            case InputMode.divineShot:

                UpdateTargetPos();

                if (Input.GetMouseButtonDown(0))
                {
                    Collider[] hitColliders = Physics.OverlapSphere(UIManager.Instance.divineCellTarget.transform.position, 1, 1<<12 | 1<< 16);
                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        if(hitColliders[i].TryGetComponent<Destructible>(out Destructible destrucible))
                        {
                            destrucible.ReceiveDamage(3);
                        }
                    }

                    shootingCell.Decharge();
                    SwitchInputMode(InputMode.normal);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    SwitchInputMode(InputMode.normal);
                }


                break;
                #endregion
        }




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
        selectedCell = CurrentHit.transform.GetComponent<CellMain>();
        UIManager.Instance.CellSelected(selectedCell.transform.position);
        CellManager.Instance.selectedCell = selectedCell;
    }

    public void ResetInputs()
    {
        dragging = false;
        holdingLeftClick = false;
        clickTime = 0;
    }

    public void StartMovingCell(CellMain cell)
    {
        cell.TickDesinscription();
        Instance.objectMoved = cell;
        SwitchInputMode(InputMode.movingCell);
        Instance.dragging = false;
        //Instance.holdingLeftClick = false;
    }


    public void SelectElement()
    {
        selectedElement = currentPlayerAction;
    }

    public void DeselectElement()
    {
        UIManager.Instance.DeselectElement();

        selectedElement = null;
    }

    public void StopCurrentAction()
    {

        UIManager.Instance.DeselectElement();


        if (inputMode == InputMode.movingCell)
        {
            CellManager.Instance.SupressCurrentLink();
            objectMoved.Inpool();
            SwitchInputMode(InputMode.normal);
        }

        ResetInputs();
    }

    public void SetShootingCell(CellDivine newShootingCell)
    {
        shootingCell = newShootingCell;
    }

    public static void SwitchInputMode(InputMode newInputMode)
    {
        Instance.inputMode = newInputMode;

        if(newInputMode == InputMode.divineShot)
        {
            UIManager.Instance.DisplayDivineShot(Instance.shootingCell);
        }
        else
        {
            Instance.shootingCell = null;
            UIManager.Instance.HideDivineShot();
        }
    }

    public void UpdateTargetPos()
    {
        float dist = (shootingCell.transform.position - mouseWorldPos).sqrMagnitude;
        if(dist < Mathf.Pow(shootingCell.specifiqueStats/2, 2)/*range au carré*/)
        {
            UIManager.Instance.SetTargetPos(mouseWorldPos);
        }
        else
        {
            UIManager.Instance.SetTargetPos(shootingCell.transform.position + (mouseWorldPos - shootingCell.transform.position).normalized * (shootingCell.specifiqueStats/2));
        }

    }
}
