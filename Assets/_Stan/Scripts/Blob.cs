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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rd = GetComponent<Renderer>();
        //rajoute le blob à la liste des blobs actifs dans la scène
        BlobManager.blobList.Add(this);

    }

    private void OnDestroy()
    {
        //retire le blob de la liste des blobs actifs dans la scène
        BlobManager.blobList.Remove(this);
        Inpool();
    }


    public void Jump(Vector3 direction)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }

    
}
