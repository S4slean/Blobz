using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellArmory : CellMain
{
    [Header("SPECIFICITE CELL")]
    public GameObject targetDirection;
    [Header("Proximity Level Modif")]
    public float[] BlopPerTick;


    private int tickForActivation;
    private int currentTick;



    public override void BlobsTick()
    {

        if (currentTick == tickForActivation)
        {
            currentTick = 0;
        }

        if (BlobNumber > 0)
        {
            for (int i = 0; i < myCellTemplate.rejectPower_RF; i++)
            {
                RemoveBlob(1);
                // Debug.LogWarning("PENSEZ à REGLER le sy")
                Blob newBlob = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
                BlobManager.blobList.Add(newBlob);

                newBlob.blobType = BlobManager.BlobType.soldier;

                newBlob.Outpool();

                newBlob.transform.position = targetDirection.transform.position + Helper.RandomVectorInUpSphere();

                newBlob.Jump(Helper.RandomVectorInUpSphere() * 100);

            }
        }
        //}
    }

    public override void TickInscription()
    {
        //base.TickInscription();
        TickManager.doTick2 += BlobsTick;
    }

    public override void TickDesinscription()
    {
        //base.TickDesinscription();
        TickManager.doTick2 -= BlobsTick;
    }

    public override void ProximityLevelModification(int Amout)
    {
        base.ProximityLevelModification(Amout);
        if (currentProximityLevel > 0)
        {
            switch (currentProximityLevel)
            {
                case 0:
                    currentProximityTier = 0;

                    break;
                case 1:
                    currentProximityTier = 1;
                    break;
                case 2:
                    currentProximityTier = 2;
                    break;
                case 3:
                    currentProximityTier = 3;
                    break;
                //si > 0 max tier (soit 4 ) 

                default:
                    currentProximityTier = 3;
                    break;

            }
        }
        else
        {
            currentProximityTier = 0;
        }





    }

}
