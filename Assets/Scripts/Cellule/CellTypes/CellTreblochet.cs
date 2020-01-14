using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTreblochet : CellMain
{
    public treblochetChargeur myChargeur;
    public bool chargerIsFull;


    public override void ProximityLevelModification()
    {
        base.ProximityLevelModification();
        myChargeur.UpdateSpecificity(specifiqueStats);
    }

    public override void BlobsTick()
    {
        if (blobNumber - blobCoaches.Count > 0 & !chargerIsFull)
        {
            BlobManager.BlobType blType = ChargerCustomBlobCheck();
            BlobNumberVariation(-1, blType);
            myChargeur.AddBlob(blType);
        }

        base.BlobsTick();
    }

    public override void CellInitialisation()
    {
        base.CellInitialisation();
        myChargeur.Init();
    }


    private BlobManager.BlobType ChargerCustomBlobCheck()
    {
        if (explorateurBlobNumber > 0)
        {
            if ((int)Random.Range(1, 101) <= 50)
            {
                return BlobManager.BlobType.explorateur;
            }
        }

        if (blobNumber - (blobCoaches.Count + explorateurBlobNumber) > 0)
        {
            return BlobManager.BlobType.soldier;
        }
        return BlobManager.BlobType.explorateur;
    }

    public override void blobAddCheckType(int amount, BlobManager.BlobType _blobType)
    {
        switch (_blobType)
        {
            case BlobManager.BlobType.normal:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.normal, amount);
                normalBlobNumber += amount;
                break;

            case BlobManager.BlobType.soldier:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.normal, amount);
                normalBlobNumber += amount;
                break;

            case BlobManager.BlobType.coach:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                break;

            case BlobManager.BlobType.explorateur:
                explorateurBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
                break;

        }
        blobNumber = normalBlobNumber + blobCoaches.Count + explorateurBlobNumber;

    }


}

