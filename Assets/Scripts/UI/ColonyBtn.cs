﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColonyBtn : PoolableObjects
{
    [HideInInspector]public int cost = 100;
    [HideInInspector] public Vector3 point;

    public TextMeshProUGUI txt;
    public void BuildNexus()
    {
        if(RessourceTracker.instance.energy > cost)
        {
            CellProductrice newProd = ObjectPooler.poolingSystem.GetPooledObject<CellProductrice>() as CellProductrice;
            newProd.Outpool();
            newProd.transform.position = point;
            newProd.CellInitialisation();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough Energy");
        }
    }

    public void UpdateText()
    {
        txt.SetText("New Nexus Cost: " + cost);
    }
}
