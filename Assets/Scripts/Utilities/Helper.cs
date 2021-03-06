﻿using UnityEngine;

public class Helper : MonoBehaviour
{
    public static int LoopIndex(int currentIndex, int maxIndex)
    {
        if (currentIndex >= maxIndex)
        {
            if (maxIndex <= 0)
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
    public static RaycastHit ReturnHit(Vector3 pos, Camera cam)
    {
        RaycastHit originhit;
        Ray ray = cam.ScreenPointToRay(pos);
        Physics.Raycast(ray, out originhit);
        return originhit;
    }
    public static RaycastHit ReturnHit(Vector3 pos, Camera cam, int LayerMask)
    {
        RaycastHit originhit;
        Ray ray = cam.ScreenPointToRay(pos);
        Physics.Raycast(ray, out originhit, 1000, LayerMask);
        return originhit;
    }

    public static bool ReturnHit(Vector3 startPos, Vector3 endPos)
    {
        RaycastHit hit;
        Vector3 dir = endPos - startPos;
        dir = new Vector3(dir.x, 0, dir.z).normalized;

        float distance = Vector3.Distance(startPos, endPos);


        Debug.DrawLine(startPos, endPos);
        return Physics.Raycast(startPos, dir, out hit, distance, 1 << 11 | 1 << 15 | 1<<16);

    }

    public static bool CheckAvailableSpace(Vector3 pos, float radius, Collider colliderToIgnore)
    {

        Collider[] objects = Physics.OverlapSphere(pos, radius, 1 << 12 | 1 << 11 | 1 << 15 | 1 << 16);

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != colliderToIgnore)
            {

                return false;
            }
        }

        return true;

    }
}
