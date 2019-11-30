using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : PoolableObjects
{
    public Rigidbody rb;
    public int tickCount = 0; 
    public BlobManager.BlobType blobType = BlobManager.BlobType.normal;
    public Renderer rd;
    public Transform tagetTransform;
    public bool isStuck = false;
    public CellMain infectedCell;
    public int infectionAmount = 1;
    public float flyTime = 3;
    public float flySpeed = 5;



    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rd = GetComponent<Renderer>();
        //rajoute le blob à la liste des blobs actifs dans la scène
        BlobManager.blobList.Add(this);
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

    private void OnCollisionEnter(Collision collision)
    {
        if(blobType == BlobManager.BlobType.mad && collision.transform.tag == "Cell")
        {
            infectedCell = collision.transform.GetComponent<CellMain>();
            infectedCell.StockageCapabilityVariation( - infectionAmount);
            BlobManager.blobList.Remove(this);
        }
    }


}
