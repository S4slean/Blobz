using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellArmory : CellMain
{
    [Header("SPECIFICITE CELL")]
    public GameObject targetDirection;
    //public int tickForActivation;

    //private int currentTick;



    public override void BlobsTick()
    {

        //if (currentTick == tickForActivation)
        //{
        //    currentTick = 0;

        if (BlobNumber > 0)
        {
            for (int i = 0; i < myCellTemplate.rejectPower_RF; i++)
            {
                RemoveBlob(1);
                Debug.Log("Spawn");

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

}
