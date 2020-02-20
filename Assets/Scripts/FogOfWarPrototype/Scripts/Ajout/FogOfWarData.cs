using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarData : MonoBehaviour
{

    public Transform MyTransform { get; private set; }
    public float rangeOfSight;

    private void Awake()
    {
        if (MyTransform == null)
        {
            MyTransform = transform;
        }

    }


}
