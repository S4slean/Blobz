using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Variables
    public static InputManager Instance;

    public LayerMask maskLeftCLick;

    //défini la distance avant d'activé le drag 
    public float distanceBeforeDrag;

    //variables pour l'interaction avec la cellule 
    public float DelayBetweenClick;
    public float clickCooldown;

    //bools concernant la pahse de dragging d'un lien 
    public bool CellSelected;
    public bool DraggingLink;

    //Ui
    public bool InCellSelection;
    public bool InPauseMenu;

    //LayerMask
    int layer_Mask_Cell; 

    [HideInInspector]
    public Vector3 posCell;

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
            DontDestroyOnLoad(gameObject);
        }
        layer_Mask_Cell = LayerMask.GetMask("Cell");
    }

    //permet d'éviter un getComponent à chaque frame lors du raycast de mouse Over
    private bool isOverCell;
    private bool rightClickedOnCell;
    private bool leftClickedOnCell;
    private CellMain cellOver;

    public bool movingObject = false;
    public CellMain objectMoved;

    private void Update()
    {
        RaycastHit hit = Helper.ReturnHit(Input.mousePosition, CellManager.mainCamera, maskLeftCLick);

        if (!movingObject)
        {
            #region LINKS, INTERACTIONS AND CELL_CREATIONS

            //En train de drag le lien 
            if (DraggingLink && !InCellSelection)
            {

                CellManager.Instance.DragNewlink(hit);
            }


            //Click Gauche Maintient
            if (Input.GetMouseButton(0))
            {
                clickTime += Time.deltaTime;

                if (CellSelected && clickTime > clickCooldown && leftClickedOnCell)
                {
       
                    //drag pour crée un lien
                    if (Vector3.Distance(posCell, hit.point) >= distanceBeforeDrag && !DraggingLink)
                    {
                        CellManager.Instance.CreatenewLink();

                    }
                }
            }


            //Click Gauche In
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform != null && hit.transform.tag == "Cell")
                    leftClickedOnCell = true;


                if (!DraggingLink && !InCellSelection && isOverCell)
                {

                    //Debug.Log(hit.transform, hit.transform);
                    CellManager.Instance.SelectCell(hit);
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



                if (DraggingLink)
                {

                    CellManager.Instance.ValidateNewLink(hit);

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
                if(UIManager.Instance.cellOptionsUI.gameObject.activeSelf)
                    UIManager.Instance.cellOptionsUI.anim.Play("Hide");
            }

            #endregion
        }
        else
        {

            if(hit.transform.tag == "Ground")
            {
                CellManager.Instance.CellDeplacement(hit.point, objectMoved);

            }

            //si clic gauche, replacer la cell et update tous ses liens
            if (Input.GetMouseButtonDown(0))
            {
                movingObject = false;
                objectMoved = null;
            }

            else if (Input.GetMouseButtonDown(1))
            {
                CellManager.Instance.CellDeplacement(CellManager.Instance.originalPosOfMovingCell, objectMoved);
                movingObject = false;
                objectMoved = null;
            }
        }

        #region CAMERA

        CameraController.instance.MoveCamera();

        if (Input.GetKey(KeyCode.LeftAlt))
            CameraController.instance.TiltCamera();
        else
            CameraController.instance.DecreaseTiltCount();

        #endregion

    }

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
}
