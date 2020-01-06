using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellArmory : CellMain
{
    [SerializeField]
    private float rfBonus = 0;

    public override void BlobsTick()
    {
        haveExpulse = false;
        if (blobNumber > 0)
        {
            currentTick++;
            if (currentTick == currentTickForActivation)
            {
                //for (int i = 0; i < myCellTemplate.rejectPowerBase + rfBonus; i++) f
                for (int i = 0; i < currentRejectPower + rfBonus; i++)
                {
                    if (blobNumber > 0)
                    {
                        BlobNumberVariation(-1 , BlobManager.BlobType.normal);
                        // Debug.LogWarning("PENSEZ à REGLER le sy")
                        Blob newBlob = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
                        BlobManager.blobList.Add(newBlob);

                        newBlob.blobType = BlobManager.BlobType.soldier;

                        newBlob.Outpool();

                        newBlob.tag = "Untagged";

                        newBlob.transform.position = TargetPos.transform.position + Helper.RandomVectorInUpSphere();

                        //newBlob.Jump(Helper.RandomVectorInUpSphere() * 1);
                        haveExpulse = true;
                    }
                }
                //Ajout du nouveau Systeme de distribution 

                for (int i = 0; i < outputLinks.Count; i++)
                {
                    if (blobNumber <= 0)
                    {
                        break;
                    }
                    //Pour l'instant il y a moyen que si une cellule creve la prochaine 
                    //soit sauté mai squand il y aura les anim , ce sera plus possible
                    outputLinks[i].Transmitt(1 ,BlobCheck());
                    haveExpulse = true;

                }
                currentTick = 0;
            }
        }
        else
        {
            currentTick = 0;
        }
        //}
        if (haveExpulse)
        {
            anim.Play("BlobExpulsion");
        }
    }

    public override void TickInscription()
    {
        //base.TickInscription();
        TickManager.doTick += BlobsTick;
    }

    public override void TickDesinscription()
    {
        //base.TickDesinscription();
        TickManager.doTick -= BlobsTick;
    }

    public override void ProximityLevelModification()
    {
        base.ProximityLevelModification();

        //if (currentProximityLevel > 0)
        //{
        //    switch (currentProximityLevel)
        //    {
        //        case 0:
        //            currentProximityTier = 0;
        //            tickForActivation = (1 / myCellTemplate.BlopPerTick[currentProximityTier]);
        //            break;
        //        case 1:
        //            currentProximityTier = 1;
        //            tickForActivation = (1 / myCellTemplate.BlopPerTick[currentProximityTier]);
        //            break;
        //        case 2:
        //            currentProximityTier = 2;
        //            tickForActivation = (1 / myCellTemplate.BlopPerTick[currentProximityTier]);
        //            break;
        //        case 3:
        //            currentProximityTier = 3;

        //            break;
        //        //si > 0 max tier (soit 4 ) 

        //        default:
        //            currentProximityTier = 3;
        //            tickForActivation = (1 / myCellTemplate.BlopPerTick[currentProximityTier]);
        //            break;

        //    }

        currentTickForActivation = (1 / myCellTemplate.tickForActivation[currentProximityTier]);
        if (currentTickForActivation < 1)
        {
            currentTickForActivation = 1;
            rfBonus = myCellTemplate.BlopPerTick[currentProximityTier] - 1;
        }
        if (currentProximityLevel < 0)
        {
            currentProximityTier = 0;
            currentTickForActivation = (int)(1 / myCellTemplate.BlopPerTick[currentProximityTier]);
        }
    }
}
