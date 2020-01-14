using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSalle : CellMain
{
    public int myCoachBlobNumber;

    public override void blobAddCheckType(int amount, BlobManager.BlobType _blobType)
    {
        switch (_blobType)
        {
            case BlobManager.BlobType.normal:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                for (int i = 0; i < amount; i++)
                {
                    BlobCoach newCoach = new BlobCoach();
                    newCoach.Init(this, specifiqueStats);
                    newCoach.ChangeCellArrive(this);
                }
                break;
            case BlobManager.BlobType.coach:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                break;
            case BlobManager.BlobType.explorateur:
                explorateurBlobNumber += amount;
                break;
        }
        blobNumber = normalBlobNumber + blobCoaches.Count + explorateurBlobNumber;
        UpdateCaract();
    }

    public override void CheckForCoach(BlobManager.BlobType _blobType)
    {
        if (blobCoaches.Count - myCoachBlobNumber <= 0)
        {
            //coachBlobNumber = 0;
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
    }

    public override void BlobAmountCheck(int amount, BlobManager.BlobType _blobType)
    {
        if (blobNumber > currentBlobStockage)
        {
            Debug.Log("Nexus");
            int blobToRemobe = blobNumber - currentBlobStockage;
            switch (_blobType)
            {
                case BlobManager.BlobType.normal:
                    Debug.Log("Coach à moi");
                    RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.coach, blobToRemobe);
                    for (int i = 0; i < blobToRemobe; i++)
                    {
                        myCoachBlobNumber--;
                        blobCoaches[myCoachBlobNumber].Death();
                    }
                    break;

                case BlobManager.BlobType.coach:
                    RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.coach, blobToRemobe);
                    for (int i = 0; i < blobToRemobe; i++)
                    {
                        blobCoaches[blobCoaches.Count-1].Death();
                    }
                    break;

                case BlobManager.BlobType.explorateur:
                    RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.explorateur, blobToRemobe);
                    explorateurBlobNumber -= blobToRemobe;
                    break;
            }
            blobNumber = normalBlobNumber + explorateurBlobNumber + blobCoaches.Count;
        }
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

        if (blobCoaches.Count > 1)
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
            else if (blobCoaches.Count > 1)
            {
                return BlobManager.BlobType.coach;

            }
            else
            {
                return BlobManager.BlobType.aucun;
            }
        }
    }
}
