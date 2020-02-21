using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarData : MonoBehaviour
{

    public Transform MyTransform { get; private set; }
    public float rangeOfSight;
    public bool alreadyAddToFog;

    private void Awake()
    {
        if (MyTransform == null)
        {
            MyTransform = transform;
        }
    }
}
