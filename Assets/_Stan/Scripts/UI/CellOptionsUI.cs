using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellOptionsUI : MonoBehaviour
{
    public Animator anim;
    public CellMain cell;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void MoveCell()
    {
        InputManager.Instance.objectMoved = cell;
        InputManager.Instance.movingObject = true;

        //store original POsition
        CellManager.Instance.originalPosOfMovingCell = cell.transform.position;

        anim.Play("Hide");
    }

    public void DeleteCell()
    {
        Debug.Log(cell);
        cell.Died(true);

        anim.Play("Hide");
    }

    public void HideCellOptionUI()
    {
        UIManager.Instance.HideUI(gameObject);
    }

}
