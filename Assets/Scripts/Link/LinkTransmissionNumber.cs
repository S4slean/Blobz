using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTransmissionNumber : MonoBehaviour
{
    private Transform myTransform;

    private void Awake()
    {
        if (myTransform == null)
        {
            myTransform = transform;
        }
        
    }

    public void UpdatePosAndScale(Vector3 pos , float angle)
    {
        myTransform.position = pos + new Vector3(0, 0.2f, 0);
        myTransform.rotation = Quaternion.Euler(90, angle%90 - 90, 0);
    }

}
