using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAlert : PoolableObjects
{
    [HideInInspector]public CellMain associatedCell;
    [SerializeField]private RectTransform rect;
    [SerializeField] private Animator anim;

    private void Update()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        if (associatedCell == null)
            return;

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 screenObjPos = Camera.main.WorldToScreenPoint(associatedCell.transform.position);
        Vector3 dir = (screenObjPos - screenCenter).normalized;
        rect.anchoredPosition = Vector2.zero + new Vector2(dir.x * Screen.width/2.2f, dir.y * Screen.height/2.3f);

        //float angle = Vector2.Angle(Vector2.up, dir);
        //rect.localRotation = Quaternion.Euler(0, 0, angle);
        //rect.eulerAngles = new Vector3(0, 0, angle);
    }

    public void Display(CellMain cell)
    {
        associatedCell = cell;
        
        UpdatePos();
        anim.SetBool("Show", true);
    }

    public void Hide()
    {
        anim.SetBool("Show", false);
    }

    public void BackToPool()
    {
        ClearRefs();
        Inpool();
    }

    public void ClearRefs()
    {
        associatedCell = null;
    }
}
