﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObjects : MonoBehaviour
{
    public bool canBePool;
    public GameObject initialPool;


    public virtual void Outpool()
    {
        canBePool = false;
        gameObject.SetActive(true);
    }

    public virtual void InpoolEditor()
    {
        canBePool = true;
        gameObject.SetActive(false);
    }

    public virtual void Inpool()
    {

        transform.position = initialPool.transform.position;
        transform.SetParent(initialPool.transform);
        // gameObject.SetActive(false);
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DesactiveGameObject());
        }

;
    }
    protected IEnumerator DesactiveGameObject()
    {
        //A changé mais c'est pour test
        yield return new WaitForFixedUpdate();
        gameObject.SetActive(false);
        canBePool = true;
    }
}
