﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProductrice : CellMain
{
    [Header("Spécificitées")]
    private int SurproductionRate;
   // [Header("Proximity Level Modif")]
   // [Range(0f, 100f)]
   //// public int[] myCellTemplate.SurprodRate;
    

    public override void BlobsTick()
    {
        //base.BlobsTick();
        //ça marche bien mais à voir si quand 1 batiment meure la produciton saute avec ou pas
        for (int i = 0; i < myCellTemplate.rejectPower_RF; i++)
        {
            if (BlobNumber > 0 && outputLinks.Count > 0)
            {
                if (currentIndex >= outputLinks.Count)
                {
                    return;
                }
                outputLinks[currentIndex].Transmitt();
                currentIndex++;
                currentIndex = Helper.LoopIndex(currentIndex, outputLinks.Count);
            }
        }
        if ((int)Random.Range(0, 101) <= SurproductionRate)
        {
            AddBlob(myCellTemplate.prodPerTick * 2);
        }
        else
        {
            AddBlob(myCellTemplate.prodPerTick);
        }
    }


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
                    SurproductionRate = myCellTemplate.SurproductionRate[0];
                    break;
                case 1:
                    currentProximityTier = 1;
                    SurproductionRate = myCellTemplate.SurproductionRate[1];
                    break;
                case 2:
                    currentProximityTier = 2;
                    SurproductionRate = myCellTemplate.SurproductionRate[2];
                    break;
                case 3:
                    currentProximityTier = 3;
                    SurproductionRate = myCellTemplate.SurproductionRate[3];
                    break;
                //si > 0 max tier (soit 4 ) 

                default:
                    currentProximityTier = 3;
                    SurproductionRate = myCellTemplate.SurproductionRate[3];
                    break;

            }
        }
        else
        {
            currentProximityTier = 0;
            SurproductionRate = myCellTemplate.SurproductionRate[0];
        }
    }
}
