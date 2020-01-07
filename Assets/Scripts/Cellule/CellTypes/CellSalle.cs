using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSalle : CellMain
{
    private int myCoachBlobNumber;
    private List<BlobCoach> blobCoaches = new List<BlobCoach>();

    public override void BlobNumberVariation(int amount, BlobManager.BlobType _blobType)
    {
        switch (_blobType)
        {
            case BlobManager.BlobType.normal:
                myCoachBlobNumber += amount;
                if (myCoachBlobNumber >= myCellTemplate.maxBlobCoach)
                {
                    myCoachBlobNumber = myCellTemplate.maxBlobCoach;
                    return;
                }
                if (myCoachBlobNumber < 0)
                {
                    myCoachBlobNumber = 0;
                }
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                BlobCoach newCoach = new BlobCoach();
                newCoach.origianlSalle = this;
                newCoach.inThisCell = this;
                newCoach.currentLife = specifiqueStats;
                blobCoaches.Add(newCoach);
                BlobManager.instance.blobCoaches.Add(newCoach);
                break;
            case BlobManager.BlobType.coach:
                coachBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);

                if (coachBlobNumber < 0)
                {
                    coachBlobNumber = 0;
                    hasBlobCoach = false;
                    coachIcon.SetActive(false);
                    ProximityLevelModification();
                }
                else
                {
                    hasBlobCoach = true;
                    coachIcon.SetActive(true);
                    ProximityLevelModification();
                }
                break;
            case BlobManager.BlobType.explorateur:
                explorateurBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
                break;
        }
        blobNumber = myCoachBlobNumber + coachBlobNumber + explorateurBlobNumber;


        float ratio = (float)blobNumber / (float)currentBlobStockage;
        int pourcentage = Mathf.FloorToInt(ratio * 100f);
        if (pourcentage >= 80)
        {
            inDanger = true;
            if (!isVisible)
            {
                CellAlert alert = ObjectPooler.poolingSystem.GetPooledObject<CellAlert>() as CellAlert;
                UIManager.Instance.DisplayCellAlert(transform, alert);
            }
            //ANIM DANGER CELL 
        }
        else
        {
            inDanger = false;
        }

        if (blobNumber > currentBlobStockage && !isDead && !isNexus)
        {
            Died(false);
        }

        //Nexus 
        if (blobNumber > currentBlobStockage)
        {
            int blobToRemobe = blobNumber - currentBlobStockage;
            RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.normal, blobToRemobe);
            blobNumber = currentBlobStockage;
        }
        UpdateCaract();
    }
    public override void SetupVariable()
    {
        base.SetupVariable();
        myCoachBlobNumber = 0;
        foreach (BlobCoach blob in blobCoaches)
        {
            blob.SalleDeath();
        }

        blobCoaches.Clear();
    }

}


[System.Serializable]
public struct BlobCoach
{
    public CellSalle origianlSalle;
    public int currentLife;
    public CellMain inThisCell;

    private bool salleDead;

     public void Init()
    {
        TickManager.doTick += Tick;
    }
    private void LooseLife()
    {
        currentLife -= 1;
        if (currentLife <=0)
        {
            Death();
        }
    }

    public void ChangeCell(CellMain newCell)
    {
        inThisCell = newCell;
    }

    private void Tick()
    {
        if (origianlSalle != inThisCell || salleDead == true )
        {
            LooseLife();
        }
    }

    public void SalleDeath()
    {
        origianlSalle = null;
        salleDead = true;
    }

    private void Death()
    {
        inThisCell.BlobNumberVariation(-1, BlobManager.BlobType.coach);
        BlobManager.instance.blobCoaches.Remove(this);
        
    }
}
