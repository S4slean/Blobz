using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProductrice : CellMain
{
    [Header("Spécificitées")]
    [Range(0, 100)]
    public int SurproductionRate;


    public override void BlobsTick()
    {
        //base.BlobsTick();
        Debug.Log("Tick");
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

}
