using System.Collections;
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
                for (int i = 0; i < amount; i++)
                {
                    explorateurBlobNumber += amount;
                }
                break;
            case BlobManager.BlobType.coach:
                //coachBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                break;
            case BlobManager.BlobType.explorateur:
                explorateurBlobNumber += amount;
                break;
        }
        blobNumber = normalBlobNumber + blobCoaches.Count + explorateurBlobNumber;
    }


}
