using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleCheck : MonoBehaviour
{

    private bool isVisible;

    private void OnBecameInvisible()
    {
        isVisible = false;

    }
    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void Update()
    {
        if (isVisible)
        {
            Debug.Log("Is Visible ,object :" , gameObject);
        }
        else
        {
            Debug.Log("Invisible ,object :" , gameObject);

        }
    }
}
