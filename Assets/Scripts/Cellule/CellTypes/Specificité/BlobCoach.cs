using System.Collections;
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
    public void ChangeCell(CellMain newCell)
    {
        previousCell = inThisCell;

        inThisCell = newCell;

        inThisCell.blobCoaches.Add(this);

        if (previousCell != null)
        {
            if (previousCell.blobCoaches.Contains(this))
            {
                previousCell.blobCoaches.Remove(this);
            }
        }
        if (previousCell == origianlSalle)
        {
            origianlSalle.myCoachBlobNumber--;
        }
        if (inThisCell == origianlSalle )
        {
            origianlSalle.myCoachBlobNumber++;
        }
    }
    private void Tick()
    {
        if (origianlSalle != inThisCell)
        {
            LooseLife();
        }
    }
    private void Death()
    {
        TickManager.doTick -= Tick;
        inThisCell.blobCoaches.Remove(this);
        inThisCell.BlobNumberVariation(-1, BlobManager.BlobType.coach);

    }

}
