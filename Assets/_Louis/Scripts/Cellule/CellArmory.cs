using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellArmory : CellMain
{
   // [Header("SPECIFICITE CELL")]
   // public GameObject myCellTemplate.targetDirection;
   // [Header("Proximity Level Modif")]
   // public float[] myCellTemplate.BlopPerTick;

    [SerializeField]
    private float tickForActivation;
    private float currentTick;
    [SerializeField]
    private float rfBonus = 0;



    
    public override void BlobsTick()
    {
        
        if (BlobNumber > 0)
        {
            currentTick++;
            if (currentTick == tickForActivation)
            {
                for (int i = 0; i < myCellTemplate.rejectPower_RF + rfBonus; i++)
                {
                    RemoveBlob(1);
                    // Debug.LogWarning("PENSEZ à REGLER le sy")
                    Blob newBlob = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
                    BlobManager.blobList.Add(newBlob);

                    newBlob.blobType = BlobManager.BlobType.soldier;

                    newBlob.Outpool();

                    newBlob.transform.position = myCellTemplate.targetDirection.transform.position + Helper.RandomVectorInUpSphere();

                    //newBlob.Jump(Helper.RandomVectorInUpSphere() * 1);

                }
                currentTick = 0;
            }
        }
        else
        {
            currentTick = 0;
        }
        //}
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

    public override void ProximityLevelModification(int Amout)
    {
        base.ProximityLevelModification(Amout);
        if (currentProximityLevel > 0)
        {
            switch (currentProximityLevel)
            {
                case 0:
                    currentProximityTier = 0;
                    tickForActivation = (1 / myCellTemplate.BlopPerTick[currentProximityTier]);
                    break;
                case 1:
                    currentProximityTier = 1;
                    tickForActivation = (1 / myCellTemplate.BlopPerTick[currentProximityTier]);
                    break;
                case 2:
                    currentProximityTier = 2;
                    tickForActivation = (1 / myCellTemplate.BlopPerTick[currentProximityTier]);
                    break;
                case 3:
                    currentProximityTier = 3;
                    tickForActivation = (1 / myCellTemplate.BlopPerTick[currentProximityTier]);
                    break;
                //si > 0 max tier (soit 4 ) 

                default:
                    currentProximityTier = 3;
                    tickForActivation = (1 / myCellTemplate.BlopPerTick[currentProximityTier]);
                    break;

            }
            if (tickForActivation < 1)
            {
                tickForActivation = 1;
                rfBonus = myCellTemplate.BlopPerTick[currentProximityLevel] - 1;
            }
        }
        else
        {
            currentProximityTier = 0;
            tickForActivation = (int)(1 / myCellTemplate.BlopPerTick[currentProximityTier]);
        }





    }

}
