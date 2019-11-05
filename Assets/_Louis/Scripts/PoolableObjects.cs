using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObjects : MonoBehaviour
{
    public bool canBePool;


    public void Outpool()
    {
        canBePool = false;
        gameObject.SetActive(true);
    }

    public void Inpool()
    {
        canBePool = true;
        gameObject.SetActive(false);
    }
}
