﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlobCoach
{
    public CellSalle origianlSalle;
    public int currentLife;
    public CellMain inThisCell;
    public CellMain previousCell;

    public void Init(CellSalle cell, int life)
    {
        TickManager.doTick += Tick;
        origianlSalle = cell;
        currentLife = life;
    }
    public void LooseLife()
    {
        currentLife -= 1;
        if (currentLife <= 0)
        {
            Death();
        }
    }
    public void ChangeCellArrive()
    {
        if (inThisCell == null)
        {
            return;
        }

        inThisCell.blobCoaches.Add(this);
        if (inThisCell == origianlSalle)
        {
            origianlSalle.myCoachBlobNumber++;
        }
        inThisCell.BlobNumberVariation(1, BlobManager.BlobType.coach, true);

    }
    public void ChangeCellArrive(CellMain cell)
    {
        inThisCell = cell;

        inThisCell.blobCoaches.Add(this);
        if (inThisCell == origianlSalle)
        {
            origianlSalle.myCoachBlobNumber++;
        }
        inThisCell.BlobNumberVariation(1, BlobManager.BlobType.coach, true);
    }

    public void ChangeCellOut(CellMain newCell)
    {
        previousCell = inThisCell;
        inThisCell = newCell;

        if (previousCell != null)
        {
            if (previousCell.blobCoaches.Contains(this))
            {
                previousCell.blobCoaches.Remove(this);
                if (previousCell == origianlSalle)
                {
                    origianlSalle.myCoachBlobNumber--;
                }
                previousCell.BlobNumberVariation(-1, BlobManager.BlobType.coach, true);
            }
        }
    }

    private void Tick()
    {
        if (origianlSalle != inThisCell)
        {
            LooseLife();
        }
    }
    public void Death()
    {
        TickManager.doTick -= Tick;
        if (inThisCell != null)
        {
            inThisCell.blobCoaches.Remove(this);
            inThisCell.BlobNumberVariation(-1, BlobManager.BlobType.coach, true);
        }


        origianlSalle = null;
        currentLife = 0;
        inThisCell = null;
        previousCell = null;

        //inThisCell.BlobNumberVariation(-1, BlobManager.BlobType.coach);
    }

}
