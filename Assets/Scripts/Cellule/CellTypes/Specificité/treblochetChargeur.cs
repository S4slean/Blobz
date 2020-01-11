using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treblochetChargeur : MonoBehaviour, PlayerAction
{
    public List<BlobManager.BlobType> blobInChargeur = new List<BlobManager.BlobType>();

    public int maxStockage;
    public float maxPower;
    public float yOffset;
    public float dragRange;

    //private Vector3 initialPos; 
    public CellTreblochet parent;
    private float disTanceFromParent;

    private float distance;


    private void Awake()
    {
        transform.position = new Vector3(transform.position.x, parent.transform.position.y, transform.position.z);
        dragRange = parent.myCellTemplate.magazineDragRange;
        disTanceFromParent = Vector3.Distance(transform.position, parent.transform.position);
    }
    public void Fire(float ratio)
    {
        Vector3 dir = (parent.transform.position - transform.position).normalized;
        dir = new Vector3(dir.x, 0, dir.z);
        if (blobInChargeur.Count > 0)
        {


            Blob blobToThrow = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;

            blobToThrow.ChangeType(blobInChargeur[0]);
            blobToThrow.transform.position = transform.position + Vector3.up * 1.2f;


            blobToThrow.rb.AddForce(((dir * ratio * maxPower) + (Vector3.up * yOffset)), ForceMode.Impulse);
            blobToThrow.Outpool();
            BlolbFIre();

        }
        else
        {
            // Display error
        }
        transform.position = parent.transform.position - dir * disTanceFromParent; 

    }
    public void UpdateSpecificity(int capacity)
    {
        //UI mise à jour
        maxStockage = capacity;
        if (blobInChargeur.Count >= maxStockage)
        {
            parent.chargerIsFull = true;

            int diff = blobInChargeur.Count - maxStockage;
            for (int i = 0; i < diff; i++)
            {
                parent.BlobNumberVariation(1, blobInChargeur[blobInChargeur.Count - 1]);
                blobInChargeur.RemoveAt(blobInChargeur.Count - 1);
            }
        }

    }

    private void DragAction(RaycastHit hit)
    {
        Vector3 direction = hit.point - parent.transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        direction = direction.normalized;

        distance = Vector3.Distance(parent.transform.position, hit.point);
        if (distance <= disTanceFromParent)
        {
            transform.position = parent.transform.position + direction * disTanceFromParent;
        }
        else if (distance < dragRange)
        {
            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);

        }
        else
        {
            transform.position = parent.transform.position + direction * dragRange;
        }

        transform.LookAt(parent.transform);
        transform.rotation = Quaternion.Euler(90, transform.rotation.y, transform.rotation.z);
    }

    private void DragEnd()
    {
        if (disTanceFromParent + parent.myCellTemplate.minDistanceDrag < distance)
        {
            float ratio = distance / dragRange;
            Fire(ratio);
        }
        else
        {
            Debug.Log("pas assez de force");
        }
    }

    private void DragCancel(RaycastHit hit)
    {
        Vector3 direction = hit.point - parent.transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        direction = direction.normalized;

        transform.position = parent.transform.position + direction * disTanceFromParent;
    }

    #region BlobGestion
    public void BlolbFIre()
    {
        blobInChargeur.RemoveAt(0);
        if (blobInChargeur.Count < maxStockage)
        {
            parent.chargerIsFull = false;
        }
    }

    public void AddBlob(BlobManager.BlobType blobToAdd)
    {
        if (blobInChargeur.Count < maxStockage)
        {
            blobInChargeur.Add(blobToAdd);
            //Update UI ; 

        }
    }

    public void BlobAdd(BlobManager.BlobType blobType)
    {
        blobInChargeur.Add(blobType);

        if (blobInChargeur.Count >= maxStockage)
        {
            parent.chargerIsFull = true;
        }
    }
    #endregion

    #region PLAYERINTERACTION

    public void OnLeftClickDown(RaycastHit hit)
    {
    }

    public void OnShortLeftClickUp(RaycastHit hit)
    {
    }

    public void OnLeftClickHolding(RaycastHit hit)
    {
    }

    public void OnLongLeftClickUp(RaycastHit hit)
    {

    }

    public void OnDragStart(RaycastHit hit)
    {
        //InputManager.Instance.selectedElement = this;
        //je sais pas si c'est utiles
        //initialPos = transform.position;
    }

    public void OnLeftDrag(RaycastHit hit)
    {
        DragAction(hit);
    }

    public void OnDragEnd(RaycastHit hit)
    {
        DragEnd();
    }

    public void OnShortRightClick(RaycastHit hit)
    {
    }

    public void OnRightClickWhileHolding(RaycastHit hit)
    {
    }

    public void OnRightClickWhileDragging(RaycastHit hit)
    {
        //
        InputManager.Instance.DeselectElement();
        DragCancel(hit);

    }

    public void OnmouseIn(RaycastHit hit)
    {
        //

    }

    public void OnMouseOut(RaycastHit hit)
    {
        //

    }

    public void OnSelect()
    {
        //

    }

    public void OnDeselect()
    {
        //
    }

    public void StopAction()
    {
        //

    }
    #endregion
}
