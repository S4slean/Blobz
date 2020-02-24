using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTourelle : CellMain
{
    private int munitions;
    private bool isLoaded;
    private bool fullLoaded;

    public tourelleCollider tourelleCollider;
    public ProgressBar progressBar;


    public override void BlobsTick()
    {

        if (stuckBlobs.Count <= 0 && currentBlobStockage < myCellTemplate.storageCapability)
        {
            StockageCapabilityVariation(1);
        }


        if (!overLoad)
        {
            overloadStack = 0;
            if (blobNumber > 0 && !fullLoaded)
            {
                BlobNumberVariation(-1, BlobCheck(), false);
                MunitionVariation(1);
            }
            BlobNumberVariation(myCellTemplate.prodPerTickBase, BlobManager.BlobType.normal, true);

            //ANIM
            haveExpulse = false;

            if (blobNumber > 0)
            {
                for (int i = 0; i < outputLinks.Count; i++)
                {
                    if (blobNumber > 0 && outputLinks.Count > 0)
                    {
                        if (currentIndex < outputLinks.Count)
                        {
                            BlobManager.BlobType blobType = BlobCheck();

                            if (blobType != BlobManager.BlobType.aucun)
                            {
                                outputLinks[currentIndex].Transmitt(1, BlobCheck());
                                haveExpulse = true;
                                currentIndex++;
                                currentIndex = Helper.LoopIndex(currentIndex, outputLinks.Count);
                            }
                        }
                        else
                        {
                            currentIndex = 0;
                        }
                    }
                }
            }
            if (isLoaded)
            {
                currentTick++;
                if (currentTick >= currentTickForActivation)
                {
                    //TIR
                    tourelleCollider.Fire();
                    anim.SetBool("makeAction", true);
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

    public void MunitionVariation(int amount)
    {
        munitions += amount;
        if (munitions <= 0)
        {
            munitions = 0;
            isLoaded = false;
            float rat = (float)munitions / (float)myCellTemplate.tourelleMaxMun;
            progressBar.UpdateBar(rat, false);
            return;
        }
        isLoaded = true;

        if (munitions >= myCellTemplate.tourelleMaxMun)
        {
            fullLoaded = true;
            munitions = myCellTemplate.tourelleMaxMun;
        }
        else
        {
            fullLoaded = false;
        }
        float ratio = (float)munitions / (float)myCellTemplate.tourelleMaxMun;
        progressBar.UpdateBar(ratio , false);
    }

    public override void SetupVariable()
    {
        munitions = 0;
        isLoaded = false;
        fullLoaded = false;
        float ratio = (float)munitions / (float)myCellTemplate.tourelleMaxMun;
        progressBar.UpdateBar(ratio , false);
        tourelleCollider.Init(this);
        base.SetupVariable();
    }

    public override void Died(bool intentionnalDeath)
    {
        tourelleCollider.Death();
        base.Died(intentionnalDeath);

    }

    public void setCurrentTick(int value)
    {
        currentTick = 0;
    }

}
