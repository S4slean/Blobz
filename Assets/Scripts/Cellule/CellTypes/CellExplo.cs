﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellExplo : CellMain
{

    public override void blobAddCheckType(int amount, BlobManager.BlobType _blobType)
    {
        switch (_blobType)
        {
            case BlobManager.BlobType.normal:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
                explorateurBlobNumber += amount;
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
        UpdateCaract();
    }

    public override void CheckForExplo(BlobManager.BlobType _blobType)
    {
        if (explorateurBlobNumber <= 0)
        {
            //explorateurBlobNumber = 0;
            hasExplo = false;
            exploIcon.SetActive(false);
        }
        else
        {
            hasExplo = true;
            exploIcon.SetActive(true);
        }
    }

}
