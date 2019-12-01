using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProductrice : CellMain
{
    public override void BlobsTick()
    {
        haveExpulse = false;
        //ça marche bien mais à voir si quand 1 batiment meure la produciton saute avec ou pas
        for (int i = 0; i < currentRejectPower; i++)
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
                haveExpulse = true;
            }
        }
        if ((int)Random.Range(0, 101) <= currentSurproductionRate)
        {
            AddBlob(myCellTemplate.prodPerTickBase * 2);
        }
        else
        {
            AddBlob(myCellTemplate.prodPerTickBase);
        }

        if (haveExpulse)
        {
            anim.Play("BlobExpulsion");
        }
    }

}
