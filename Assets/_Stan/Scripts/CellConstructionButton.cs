using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.Events;

[System.Serializable]
public class CellConstructionEvent : UnityEvent<CellMain>
{

}

public class CellConstructionButton : MonoBehaviour
{
    public int i;
    public CellConstructionEvent constructionEvent;


    //void Construct(CellMain cell)
    //{

    //    constructionEvent = new CellConstructionEvent();

    //    UnityEventTools.AddPersistentListener<CellMain>(constructionEvent, Action, cell);
    //    constructionEvent.AddListener(Action);
    //}

    //public void Action(CellMain cell)
    //{

    //}
}
