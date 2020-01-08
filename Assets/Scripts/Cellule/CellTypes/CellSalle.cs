using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSalle : CellMain
{
    public int myCoachBlobNumber;
    //public List<BlobCoach> createdCoach = new List<BlobCoach>();

    public override void BlobsTick()
    {
        base.BlobsTick();
 
    }

    public override void BlobNumberVariation(int amount, BlobManager.BlobType _blobType)
    {
        switch (_blobType)
        {
            case BlobManager.BlobType.normal:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                for (int i = 0; i < amount; i++)
                {
                    if (blobNumber < currentBlobStockage && !isDead)
                    {
                        BlobCoach newCoach = new BlobCoach();
                        newCoach.Init(this, specifiqueStats);
                        newCoach.ChangeCell(this);
                    }
                }
                break;
            case BlobManager.BlobType.coach:
                coachBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                break;
            case BlobManager.BlobType.explorateur:
                explorateurBlobNumber += amount;
                break;
        }
        blobNumber = normalBlobNumber + coachBlobNumber + explorateurBlobNumber + myCoachBlobNumber;

        if (blobCoaches.Count - myCoachBlobNumber <= 0)
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
    public override BlobManager.BlobType BlobCheck()
    {
        if (explorateurBlobNumber > 0)
        {
            if ((int)Random.Range(1, 101) <= 40)
            {
                return BlobManager.BlobType.explorateur;
            }
        }

        if (coachBlobNumber > 0)
        {
            if ((int)Random.Range(1, 101) <= 40)
            {
                return BlobManager.BlobType.coach;
            }
        }

        if (myCoachBlobNumber > 0)
        {
            return BlobManager.BlobType.coach;
        }

        else
        {
            if (explorateurBlobNumber > 0)
            {
                return BlobManager.BlobType.explorateur;
            }
            else
            {
                return BlobManager.BlobType.coach;
            }
        }
    }
}
