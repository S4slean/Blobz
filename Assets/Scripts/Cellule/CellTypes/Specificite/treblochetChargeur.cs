﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treblochetChargeur : MonoBehaviour /*, PlayerAction*/
{
//    public List<BlobManager.BlobType> blobInChargeur = new List<BlobManager.BlobType>();

//    public int maxStockage;
//    //  public float maxPower;
//    // public float yOffset;
//    private float dragRange;
//    // public float slotRange;
//    public SphereCollider myCollider;

//    //private Vector3 initialPos; 
//    public CellTreblochet parent;
//    private float disTanceFromParent;

//    private float finalDistance;

//    public TreblobchetUISlot[] treblobchetUISlots;
//    private Transform myTransform;
//    private bool gotABlob;

//    private Blob currentBlob;


//    private void Awake()
//    {
//        transform.position = new Vector3(transform.position.x, parent.graphTransform.position.y, transform.position.z);


//        disTanceFromParent = Vector3.Distance(transform.position, parent.transform.position);

//        if (myTransform == null)
//        {
//            myTransform = transform;
//        }
//    }


//    public void Init()
//    {
//        dragRange = parent.myCellTemplate.magazineDragRange;
//        UIGestion();
//        myCollider.enabled = true;
//        myTransform.position = new Vector3(myTransform.position.x, parent.graphTransform.position.y, myTransform.position.z);

//    }

//    //private void DragAction(RaycastHit hit)
//    //{
//    //    if (gotABlob)
//    //    {
//    //        Vector3 direction = hit.point - parent.myTransform.position;
//    //        direction = new Vector3(direction.x, 0, direction.z);
//    //        direction = direction.normalized;


//    //        //Calcul à revoir pour le ratio 
//    //        float distance = Vector3.Distance(parent.myTransform.position, hit.point);
//    //        if (distance <= disTanceFromParent)
//    //        {
//    //            transform.position = parent.graphTransform.position + direction * disTanceFromParent;
//    //            finalDistance = 0;
//    //        }
//    //        else if (distance < dragRange)
//    //        {
//    //            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
//    //            finalDistance = distance - disTanceFromParent;

//    //        }
//    //        else
//    //        {
//    //            transform.position = parent.graphTransform.position + direction * dragRange;
//    //            finalDistance = dragRange - disTanceFromParent;
//    //        }
//    //        transform.LookAt(parent.transform);

//    //        if (gotABlob)
//    //        {
//    //            currentBlob.transform.position = myTransform.position + new Vector3(0, 0.3f, 0);
//    //            currentBlob.transform.LookAt(parent.transform);
//    //        }
//    //    }
//    //    // transform.rotation = Quaternion.Euler(90, 0, transform.rotation.z);
//    //}

//    //private void DragEnd(RaycastHit hit)
//    //{

//    //    float ratio = finalDistance / (dragRange - disTanceFromParent);

//    //    if (parent.myCellTemplate.minDragRatio < ratio)
//    //    {

//    //        // float ratio = finalDistance / (dragRange - disTanceFromParent);
//    //        Fire(ratio);
//    //    }
//    //    else
//    //    {
//    //        Debug.Log("pas assez de force");
//    //        DragCancel(hit);
//    //    }
//    //}

//    //private void DragCancel(RaycastHit hit)
//    //{
//    //    Vector3 direction = hit.point - parent.myTransform.position;
//    //    direction = new Vector3(direction.x, 0, direction.z);
//    //    direction = direction.normalized;

//    //    transform.position = parent.graphTransform.position + direction * disTanceFromParent;
//    //    transform.LookAt(parent.transform);
//    //}

//    //public void Fire(float ratio)
//    //{
//    //    Vector3 dir = (parent.transform.position - transform.position);
//    //    dir = new Vector3(dir.x, 0, dir.z).normalized; ;
//    //    if (blobInChargeur.Count > 0 && gotABlob)
//    //    {


//    //        // blobToThrow.transform.LookAt(transform);

//    //        BlobManager.blobList.Add(currentBlob);
//    //        Vector3 powerVec = ((currentBlob.transform.forward * (1 + (ratio * ratio)) * parent.myCellTemplate.shotPower) + (Vector3.up * parent.myCellTemplate.verticalConstantPower));
//    //        Debug.DrawRay(transform.position, powerVec, Color.blue, 2f);

//    //        currentBlob.rb.AddForce(powerVec, ForceMode.Impulse);
//    //        currentBlob = null;
//    //        BlolbFIre();
//    //        gotABlob = false;
//    //    }

//    //    transform.position = parent.graphTransform.position - dir * disTanceFromParent;
//    //    transform.LookAt(parent.transform);

//    //}
//    //public void UpdateSpecificity(int capacity)
//    //{
//    //    //UI mise à jour
//    //    maxStockage = capacity;
//    //    if (blobInChargeur.Count >= maxStockage)
//    //    {
//    //        parent.chargerIsFull = true;

//    //        BloBRedistribution();
//    //    }
//    //    UIGestion();
//    //}

//    private void BloBRedistribution()
//    {
//        int diff = blobInChargeur.Count - maxStockage;
//        for (int i = 0; i < diff; i++)
//        {
//            parent.BlobNumberVariation(1, blobInChargeur[blobInChargeur.Count - 1], true);
//            blobInChargeur.RemoveAt(blobInChargeur.Count - 1);
//        }
//    }


//    #region BlobGestion
//    //public void BlolbFIre()
//    //{
//    //    blobInChargeur.RemoveAt(0);
//    //    if (blobInChargeur.Count < maxStockage)
//    //    {
//    //        parent.chargerIsFull = false;
//    //    }
//    //    UIGestion();
//    //}

//    //public void AddBlob(BlobManager.BlobType blobToAdd)
//    //{
//    //    if (blobInChargeur.Count < maxStockage)
//    //    {
//    //        blobInChargeur.Add(blobToAdd);
//    //        //Update UI ; 
//    //        if (blobInChargeur.Count >= maxStockage)
//    //        {
//    //            parent.chargerIsFull = true;
//    //            BloBRedistribution();
//    //        }

//    //    }
//    //    UIGestion();
//    //}

//    #endregion

//    #region PLAYERINTERACTION

//    public void OnLeftClickDown(RaycastHit hit)
//    {
//        InputManager.Instance.SetDraggingDistance(.1f);
//    }

//    public void OnShortLeftClickUp(RaycastHit hit)
//    {
//    }

//    public void OnLeftClickHolding(RaycastHit hit)
//    {
//    }

//    public void OnLongLeftClickUp(RaycastHit hit)
//    {

//    }

//    public void OnDragStart(RaycastHit hit)
//    {
//        if (!gotABlob && blobInChargeur.Count > 0)
//        {

//            gotABlob = true;
//            currentBlob = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
//            currentBlob.Outpool();

//            currentBlob.transform.position = transform.position + new Vector3(0, 0.3f, 0);
//            currentBlob.ChangeType(blobInChargeur[0]);
//            currentBlob.transform.LookAt(parent.transform);


//            BlobManager.blobList.Remove(currentBlob);

//        }
//    }

//    //public void OnLeftDrag(RaycastHit hit)
//    //{
//    //    DragAction(hit);
//    //}

//    //public void OnDragEnd(RaycastHit hit)
//    //{
//    //    DragEnd(hit);
//    //}

//    //public void OnShortRightClick(RaycastHit hit)
//    //{
//    //}

//    //public void OnRightClickWhileHolding(RaycastHit hit)
//    //{
//    //}

//    //public void OnRightClickWhileDragging(RaycastHit hit)
//    //{
//    //    //
//    //    InputManager.Instance.DeselectElement();
//    //    DragCancel(hit);

//    //}

//    //public void OnmouseIn(RaycastHit hit)
//    //{
//    //    //

//    //}

//    //public void OnMouseOut(RaycastHit hit)
//    //{
//    //    //

//    //}

//    //public void OnSelect()
//    //{
//    //    //

//    //}

//    //public void OnDeselect()
//    //{
//    //    Debug.Log("drag restored");
//    //    InputManager.Instance.BackToStandardDraggingDistance();
//    //}

//    //public void StopAction()
//    //{
//    //    //

//    //}
//    //#endregion

//    //private void UIGestion()
//    //{
//    //    for (int i = 0; i < treblobchetUISlots.Length; i++)
//    //    {
//    //        treblobchetUISlots[i].gameObject.SetActive(false);
//    //    }

//    //    for (int i = 0; i < maxStockage - 1; i++)
//    //    {

//    //        float angle = i * Mathf.PI * 3 / 2 / maxStockage;
//    //        angle += Mathf.PI;
//    //        Vector3 pos = new Vector3(Mathf.Cos(angle) * parent.myCellTemplate.magazinSlotDistance, Mathf.Sin(angle) * parent.myCellTemplate.magazinSlotDistance, 0);

//    //        treblobchetUISlots[i].gameObject.SetActive(true);
//    //        treblobchetUISlots[i].transform.localPosition = pos;
//    //        treblobchetUISlots[i].transform.LookAt(transform);
//    //        treblobchetUISlots[i].transform.rotation = Quaternion.Euler(90, 0, transform.rotation.z);


//    //        if (i > blobInChargeur.Count - 1)
//    //        {
//    //            treblobchetUISlots[i].UpdateType(BlobManager.BlobType.aucun);
//    //        }
//    //        else
//    //        {
//    //            treblobchetUISlots[i].UpdateType(blobInChargeur[i]);
//    //        }
//    //    }
//    //}


}
