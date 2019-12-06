using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : PoolableObjects
{

    #region REFS
    public Rigidbody rb;
    public Renderer rd;
    #endregion

    #region GENERAL
    public int tickCount = 0; 
    public BlobManager.BlobType blobType = BlobManager.BlobType.normal;
    #endregion

    #region ENNEMIES
    public Transform tagetTransform;
    public bool isStuck = false;
    public CellMain infectedCell;
    public int infectionAmount = 1;
    #endregion

    #region SOLDIER
    public float flyTime = 3;
    public float flySpeed = 5;
    public bool canExplode = false;
    #endregion



    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rd = GetComponent<Renderer>();


        UpdateMat();
        //rajoute le blob à la liste des blobs actifs dans la scène

        RessourceTracker.instance.AddBlob(this);
        BlobManager.blobList.Add(this);
    }

    public void ChangeType(BlobManager.BlobType newType)
    {
        RessourceTracker.instance.RemoveBlob(this);
        blobType = newType;
        UpdateMat();
        RessourceTracker.instance.AddBlob(this);

    }

    public void UpdateMat()
    {
        switch (blobType)
        {
            case BlobManager.BlobType.charged:

                rd.material = BlobManager.instance.chargedMat;
                break;

            case BlobManager.BlobType.normal:

                rd.material = BlobManager.instance.normalMat;
                break;

            case BlobManager.BlobType.mad:

                rd.material = BlobManager.instance.angryMat;
                break;

            case BlobManager.BlobType.soldier:

                rd.material = BlobManager.instance.soldierMat;
                break;
        }
    }

    private void Update()
    {
        if (blobType == BlobManager.BlobType.soldier && flyTime > 0)
            Fly();
    }

    private void OnDestroy()
    {
        if(infectedCell != null)
        {
            infectedCell.StockageCapabilityVariation(infectionAmount);
        }
        else
        {
            //retire le blob de la liste des blobs actifs dans la scène
            BlobManager.blobList.Remove(this);
        }
        Inpool();
    }


    public void Jump(Vector3 direction)
    {
        transform.LookAt(transform.position + direction);
        rb.AddForce(direction, ForceMode.Impulse);

    }

    public void Fly()
    {
        rb.velocity = transform.forward * flySpeed;
        flyTime -= Time.deltaTime;
    }

    public void ReduceCapacity()
    {
        infectedCell.StockageCapabilityVariation(-infectionAmount);
        infectionAmount++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(blobType == BlobManager.BlobType.mad && collision.transform.tag == "Cell")
        {
            infectedCell = collision.transform.GetComponent<CellMain>();
            infectedCell.StockageCapabilityVariation( - infectionAmount);
        }

        if (blobType == BlobManager.BlobType.soldier && canExplode)
            BlobManager.instance.Explode(this);
    }


}
