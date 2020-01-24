using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBroyeur : CellMain
{
    public override void BlobsTick()
    {
        haveExpulse = false;


        if (stuckBlobs.Count <= 0 && currentBlobStockage < myCellTemplate.storageCapability)
        {
            StockageCapabilityVariation(1);
        }

        if (!overLoad)
        {
            overloadStack = 0;
            if (blobNumber > 0)
            {
                int energyGainThisActivation = 0;
                currentTick++;
                if (currentTick >= currentTickForActivation)
                {
                    for (int i = 0; i < blobNumber; i++)
                    {
                        if (blobNumber > 0)
                        {

                            BlobNumberVariation(-1, BlobCheck(), false);
                            RessourceTracker.instance.EnergyVariation(specifiqueStats);
                            haveExpulse = true;
                            energyGainThisActivation += specifiqueStats;

                        }
                    }
                    TextScore newTextescore = ObjectPooler.poolingSystem.GetPooledObject<TextScore>() as TextScore;
                    newTextescore.Outpool();

                    newTextescore.myTransform.position = graphTransform.position + new Vector3(Random.Range(-1f, 1f), 0, 0);
                    newTextescore.textScore.text = ("+" + energyGainThisActivation.ToString());
                    newTextescore.PlayAnim(0);


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
                if (overloadStack >= myCellTemplate.overLoadTickMax)
                {
                    Died(false);
                }
            }
        }
    }
}
