using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carriable : MonoBehaviour
{
    public bool carried = false;
    public Collider detectionCollider;
    private Blob carryingBlob;


    private void Update()
    {
        if (carried && Vector3.SqrMagnitude(carryingBlob.transform.position - carryingBlob.originCell.position) < 9)
        {
            Collect(carryingBlob);
        }
    }

    public void GetCarried(Blob blob)
    {
        if (carried)
            return;

        carried = true;
        detectionCollider.enabled = false;
        blob.carriedObject = this;
        carryingBlob = blob;
        transform.parent = blob.carriableSocket;
        transform.localPosition = Vector3.zero;
    }

    public void GetDropped()
    {
        transform.parent = null;
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        detectionCollider.enabled = true;
        carryingBlob = null;
        carried = false;
    }

    public void Collect(Blob blob)
    {
        RessourceTracker.instance.AddRocketPiece();
        blob.carriedObject = null;
        Destroy(gameObject);
    }
}
