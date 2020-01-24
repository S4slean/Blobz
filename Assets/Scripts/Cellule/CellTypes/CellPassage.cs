using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPassage : CellMain
{
    public override void BlobsTick()
    {

        if (stuckBlobs.Count <= 0 && currentBlobStockage < myCellTemplate.storageCapability)
        {
            StockageCapabilityVariation(1);
        }

        if (!overLoad)
        {
            overloadStack = 0;
            if (myCellTemplate.prodPerTickBase > 0)
            {
                BlobNumberVariation(myCellTemplate.prodPerTickBase, BlobManager.BlobType.normal, true);
            }
            haveExpulse = false;

            if (blobNumber > 0)
            {
                currentTick++;
                if (currentTick >= currentTickForActivation)
                {
                    for (int i = 0; i < outputLinks.Count; i++)
                    {
                        bool changeIndex = false;
                        for (int y = 0; y < currentRejectPower; y++)
                        {
                            if (blobNumber > 0 && outputLinks.Count > 0)
                            {

                                BlobManager.BlobType blobType = BlobCheck();

                                if (blobType != BlobManager.BlobType.aucun)
                                {
                                    outputLinks[currentIndex].Transmitt(1, BlobCheck());
                                    haveExpulse = true;
                                    changeIndex = true;
                                }


                            }
                        }
                        if (changeIndex)
                        {
                            if (currentIndex < outputLinks.Count)
                            {
                                currentIndex++;
                                currentIndex = Helper.LoopIndex(currentIndex, outputLinks.Count);
                            }
                            else
                            {
                                currentIndex = 0;
                            }
                        }
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
        else
        {
            if (!LevelManager.instance.cellInvicible)
            {
                overloadSparke.SetSpikeNumberAndSpeed(overloadStack, overloadStack * 0.3f);
                overloadStack++;
                if (overloadStack > myCellTemplate.overLoadTickMax)
                {
                    Died(false);
                }
            }
        }
    }
}
