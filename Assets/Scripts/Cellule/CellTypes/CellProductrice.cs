using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProductrice : CellMain
{
    public override void BlobsTick()
    {
        haveExpulse = false;
        //ça marche bien mais à voir si quand 1 batiment meure la produciton saute avec ou pas
        if ((int)Random.Range(0, 101) <= currentSurproductionRate)
        {
            BlobNumberVariation(myCellTemplate.prodPerTickBase * 2);
        }
        else
        {
            BlobNumberVariation(myCellTemplate.prodPerTickBase);
        }


        #region Ancien Systeme de Tick 
        //if (blobNumber > 0)
        //{
        //    currentTick++;
        //    if (currentTick == currentTickForActivation)
        //    {
        //        Debug.Log("tickPass");
        //        for (int i = 0; i < currentRejectPower; i++)
        //        {
        //            if (blobNumber > 0 && outputLinks.Count > 0)
        //            {
        //                if (currentIndex >= outputLinks.Count)
        //                {
        //                    return;
        //                }
        //                outputLinks[currentIndex].Transmitt(1);
        //                currentIndex++;
        //                currentIndex = Helper.LoopIndex(currentIndex, outputLinks.Count);
        //                haveExpulse = true;
        //            }
        //        }
        //        currentTick = 0;
        //    }
        //}
        #endregion
        if (blobNumber > 0)
        {
            currentTick++;
            if (currentTick == currentTickForActivation)
            {

                for (int i = 0; i < outputLinks.Count; i++)
                {
                    if (blobNumber <= 0)
                    {
                        break;
                    }
                    //Pour l'instant il y a moyen que si une cellule creve la prochaine 
                    //soit sauté mai squand il y aura les anim , ce sera plus possible
                    outputLinks[i].Transmitt(1);
                    haveExpulse = true;

                }
                currentTick = 0;
            }
        }
        else
        {
            currentTick = 0;
        }


        if (haveExpulse)
        {
            anim.Play("BlobExpulsion");
        }


    }

}
