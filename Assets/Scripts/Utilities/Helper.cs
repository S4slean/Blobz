using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static int LoopIndex(int currentIndex, int maxIndex)
    {
        if (currentIndex >= maxIndex)
        {
            if (maxIndex <= 0 )
            {
                return 0;
            }
            currentIndex = currentIndex % maxIndex;
        }
        else if (currentIndex < 0)
        {
            currentIndex += maxIndex;
        }
        return currentIndex;
    }
    public static Vector3 RandomVectorInUpSphere()
    {
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)).normalized;
        return dir;
    }
    public static RaycastHit ReturnHit(Vector3 pos , Camera cam)
    {
        RaycastHit originhit;
        Ray ray = cam.ScreenPointToRay(pos);
        Physics.Raycast(ray, out originhit);
        return originhit;
    }
    public static RaycastHit ReturnHit(Vector3 pos, Camera cam , int LayerMask)
    {
        RaycastHit originhit;
        Ray ray = cam.ScreenPointToRay(pos);
        Physics.Raycast(ray, out originhit ,1000 , LayerMask);
        return originhit;
    }

    public static bool CheckAvailableSpace(Vector3 pos, float radius)
    {
        return !Physics.CheckSphere(pos, radius, 1<<12 | 1<<11 | 1<<15);
        
    }
}
