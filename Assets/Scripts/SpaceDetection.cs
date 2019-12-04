using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDetection : MonoBehaviour
{
    public CellMain cell;
    private int nbrOfObjetsInArea;

    private void OnTriggerEnter(Collider other)
    {
        nbrOfObjetsInArea++;
        if (nbrOfObjetsInArea > 0)
            cell.canBeBuild = false;
    }

    private void OnTriggerExit(Collider other)
    {
        nbrOfObjetsInArea--;
        if (nbrOfObjetsInArea < 1)
            cell.canBeBuild = true;
    }
}
