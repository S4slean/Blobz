using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carriable : MonoBehaviour
{
    public bool carried = false;
    public Collider detectionCollider;

    public void GetCarried(Blob blob)
    {
        if (carried)
            return;

        carried = true;
        detectionCollider.enabled = false;
        transform.parent = blob.carriableSocket;
        transform.localPosition = Vector3.zero;
    }

    public void GetDropped()
    {
        transform.parent = null;
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        detectionCollider.enabled = true;
        carried = false;
    }
}
