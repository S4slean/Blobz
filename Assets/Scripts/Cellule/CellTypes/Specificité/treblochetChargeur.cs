using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treblochetChargeur : MonoBehaviour, PlayerAction
{
    private Vector3 initialPos; 



    public List<BlobManager.BlobType> blobInChargeur = new List<BlobManager.BlobType>();
    public int maxStockage;
    public float maxPower;
    public float yOffset;
    private CellTreblochet parent;


    public void AddBlob(BlobManager.BlobType blobToAdd)
    {
        if (blobInChargeur.Count < maxStockage)
        {
            blobInChargeur.Add(blobToAdd);
            //Update UI ; 

        }
    }

    public void Fire(float ratio)
    {
        if (blobInChargeur.Count > 0)
        {
            Vector3 dir = (parent.transform.position - transform.position).normalized;
            dir = new Vector3(dir.x, 0, dir.z);


            Blob blobToThrow = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;

            blobToThrow.ChangeType(blobInChargeur[0]);

            blobToThrow.rb.AddForce(((dir * ratio * maxPower) + (Vector3.up * yOffset)), ForceMode.Impulse);
            blobToThrow.Outpool();
            BlolbFIre();

        }
        else
        {
            // Display error
        }
    }

    public void UpdateChargeurCapacity(int capacity)
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

    public void BlolbFIre()
    {
        blobInChargeur.RemoveAt(0);
        if (blobInChargeur.Count < maxStockage)
        {
            parent.chargerIsFull = false;
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
        initialPos = transform.position;
    }

    public void OnLeftDrag(RaycastHit hit)
    {
    }

    public void OnDragEnd(RaycastHit hit)
    {
    }

    public void OnShortRightClick(RaycastHit hit)
    {
    }

    public void OnRightClickWhileHolding(RaycastHit hit)
    {
    }

    public void OnRightClickWhileDragging(RaycastHit hit)
    {
    }

    public void OnmouseIn(RaycastHit hit)
    {
    }

    public void OnMouseOut(RaycastHit hit)
    {
    }

    public void OnSelect()
    {
    }

    public void OnDeselect()
    {
    }

    public void StopAction()
    {
    }
    #endregion
}
