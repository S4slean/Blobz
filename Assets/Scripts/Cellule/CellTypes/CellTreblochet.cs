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
        if (blobNumber - blobCoaches.Count > 0 & !chargerIsFull && ! overLoad)
        {
            BlobManager.BlobType blType = ChargerCustomBlobCheck();
            BlobNumberVariation(-1, blType , false);
            myChargeur.AddBlob(blType);
        }

        base.BlobsTick();
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

    public override void blobAddCheckType(int amount, BlobManager.BlobType _blobType , bool transmission)
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
                if (!transmission)
                {
                    if (amount < 0)
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            blobCoaches.Remove(blobCoaches[blobCoaches.Count - 1]);
                        }
                    }
                }
                break;

            case BlobManager.BlobType.explorateur:
                explorateurBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
                break;

        }
        blobNumber = normalBlobNumber + blobCoaches.Count + explorateurBlobNumber;

    }

    public override void SetupVariable()
    {

        myChargeur.Init();
        chargerIsFull = false;
        base.SetupVariable();


    }

    public override void Died(bool intentionnalDeath)
    {
        base.Died(intentionnalDeath);
        myChargeur.blobInChargeur.Clear();
    }


}

