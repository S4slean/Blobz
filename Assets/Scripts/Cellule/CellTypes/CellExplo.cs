using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellExplo : CellMain
{

    public override void blobAddCheckType(int amount, BlobManager.BlobType _blobType, bool transmission)
    {
        switch (_blobType)
        {
            case BlobManager.BlobType.normal:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
                explorateurBlobNumber += amount;
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

    public override void OnShortLeftClickUp(RaycastHit hit)
    {

        actionmade = false;
        if (stuckBlobs.Count > 0)
        {
            stuckBlobs[stuckBlobs.Count - 1].Unstuck();
            actionmade = true;
        }
        else if (overLoad)
        {
            BlobNumberVariation(-1, BlobCheck(), false);
            actionmade = true;
        }

        if (hasExplo && !actionmade)
        {
            InputManager.SwitchInputMode(InputManager.InputMode.flag);
            actionmade = true;
        }

        if (actionmade)
        {
            anim.Play("PlayerInteraction", 0, 0f);
        }


    }

}
