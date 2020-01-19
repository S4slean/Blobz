using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPassage : CellMain
{
    public override void BlobsTick()
    {
        if (!overLoad)
        {
            overloadStack = 0;
            if (myCellTemplate.prodPerTickBase > 0)
            {
                BlobNumberVariation(myCellTemplate.prodPerTickBase, BlobManager.BlobType.normal , true);
            }
            haveExpulse = false;

            if (blobNumber > 0)
            {
                currentTick++;
                if (currentTick == currentTickForActivation)
                {
                    for (int i = 0; i < outputLinks.Count; i++)
                    {
                        for (int y = 0; y < currentRejectPower; y++)
                        {
                            if (blobNumber <= 0)
                            {
                                break;
                            }
                            BlobManager.BlobType blobType = BlobCheck();
                            if (blobType != BlobManager.BlobType.aucun)
                            {
                                outputLinks[i].Transmitt(1, BlobCheck());
                                haveExpulse = true;
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
            if (!LevelManager.instance.cellInvisible)
            {
                overloadStack++;
                if (overloadStack >= myCellTemplate.overLoadTickMax)
                {
                    Died(false);
                }
            }
        }
    }
}
