using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBroyeur : CellMain
{
    [Header("Spécificity")]
    public int energyPerBlob;

    public override void BlobsTick()
    {
        if (BlobNumber > 0 )
        {
            for (int i = 0; i < myCellTemplate.rejectPower_RF; i++)
            {
                RemoveBlob(1);
                Debug.Log("Broyage");
                CellManager.Instance.EnergyVariation(energyPerBlob);
            }
        }
    }

    public override void TickInscription()
    {
        TickManager.doTick2 += BlobsTick;
       // base.TickInscription();
    }


    public override void TickDesinscription()
    {
        //base.TickDesinscription();
        TickManager.doTick2 -= BlobsTick;
    }
}
