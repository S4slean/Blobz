using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellOptionsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public Animator anim;
    public CellMain cell;
    Image img;
    public bool mouseIsOver; 
    

    private void Start()
    {
        //anim = GetComponent<Animator>();
        img = GetComponent<Image>();
    }

    

    public void MoveCell()
    {
        Debug.Log("Move Cell: " + cell);
        InputManager.Instance.objectMoved = cell;
        InputManager.Instance.movingObject = true;

        //store original POsition
        CellManager.Instance.originalPosOfMovingCell = cell.transform.position;

        mouseIsOver = false;
        UIManager.Instance.HideUI(gameObject);

        //anim.Play("Hide");
    }

    public void DeleteCell()
    {
        Debug.Log(cell);
        cell.Died(true);

        mouseIsOver = false;
        UIManager.Instance.HideUI(gameObject);
        //anim.Play("Hide");
    }

    public void HideCellOptionUI()
    {
        mouseIsOver = false;
        UIManager.Instance.HideUI(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
    }
}
