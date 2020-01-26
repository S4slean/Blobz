using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPilone : CellMain
{
    private int energie;
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
            if (blobNumber > 0)
            {
                BlobNumberVariation(-1, BlobCheck(), false);
                if (energie < myCellTemplate.maxEnergie)
                {
                    ChargeEnergie();
                }
            }
            else
            {
                if (energie > 0)
                {
                    energie--;
                    float ratio = (float)energie / (float)myCellTemplate.maxEnergie;
                    progressBar.UpdateBar(ratio , false);

                }

                if (energie <= 0)
                {
                    //Possible probleme avec les colliders 
                    // myProximityCollider[0].gameObject.SetActive(false);
                    for (int i = 0; i < myProximityCollider.Length; i++)
                    {
                        myProximityCollider[i].transform.position = myProximityCollider[i].initialPool.transform.position;
                        StartCoroutine(Desactive(myProximityCollider[i].gameObject));
                    }
                }
            }
            BlobNumberVariation(myCellTemplate.prodPerTickBase, BlobManager.BlobType.normal, true);

            //ANIM
            haveExpulse = false;

            if (blobNumber > 0)
            {
                currentTick++;
                if (currentTick >= currentTickForActivation)
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
    private void ChargeEnergie()
    {
        energie = myCellTemplate.maxEnergie;
        float ratio = (float)energie / (float)myCellTemplate.maxEnergie;
        progressBar.UpdateBar(ratio , false);


        for (int i = 0; i < myProximityCollider.Length; i++)
        {
            myProximityCollider[i].gameObject.SetActive(true);
            myProximityCollider[i].transform.position = graphTransform.position;
        }
    }

    public override void SetupVariable()
    {
        base.SetupVariable();
        energie = 0;
        myProximityCollider[0].gameObject.SetActive(false);
    }

    //c'est ok avec ça ? 
    protected IEnumerator Desactive(GameObject _gameObject)
    {
        //A changé mais c'est pour test
        yield return new WaitForFixedUpdate();
        _gameObject.SetActive(false);
    }
}
