using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceTracker : MonoBehaviour
{
    public static RessourceTracker instance;

    public int blobPop;
    public int cellNbr;

    public int hatchNbr;
    public int stockNbr;
    public int armoryNbr;

    public int energy;
    public int blobProduced;

    private void Awake()
    {
        instance = this;
    }
}
