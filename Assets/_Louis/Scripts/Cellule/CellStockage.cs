using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellStockage : CellMain
{
   // [Header("Proximity Level Modif")]
    //public int[] myCellTemplate.stockageCapacity;
    public override void ProximityLevelModification(int Amout)
    {
        
        base.ProximityLevelModification(Amout);
        if (currentProximityLevel > 0)
        {
            switch (currentProximityLevel)
            {
                case 0:
                    //c'est une variable de debug
                    currentProximityTier = 0;
                    currentBlobStockage = myCellTemplate.stockageCapacity[0];
                    break;
                case 1:
                    currentProximityTier = 1;
                    currentBlobStockage = myCellTemplate.stockageCapacity[1];
                    break;
                case 2:
                    currentProximityTier = 2;
                    currentBlobStockage = myCellTemplate.stockageCapacity[2];
                    break;
                case 3:
                    currentProximityTier = 3;
                    currentBlobStockage = myCellTemplate.stockageCapacity[3];
                    break;
                //si > 0 max tier (soit 4 ) 

                default:
                    currentProximityTier = 3;
                    currentBlobStockage = myCellTemplate.stockageCapacity[3];
                    break;

            }
        }
        else
        {
            currentProximityTier = 0;
            currentBlobStockage = myCellTemplate.stockageCapacity[0];
        }
        UpdateCaract();
    }
}
