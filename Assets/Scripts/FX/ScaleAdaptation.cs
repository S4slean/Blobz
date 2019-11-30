using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAdaptation : MonoBehaviour
{
    public Transform mainParent;

    private void OnEnable()
    {
        transform.localScale = mainParent.localScale;
    }


}
