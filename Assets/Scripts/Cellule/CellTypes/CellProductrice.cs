using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellProductrice : CellMain
{
    private int productionBonusRatio;
    private int productionBonusPacket;
    public TextMeshPro prodDisplay;


    private int Life;
    public ProgressBar progressBar;


    public override void BlobsTick()
    {
        int production = outputLinks.Count + productionBonusPacket;
        prodDisplay.text = production.ToString();

        if (stuckBlobs.Count <= 0 && Life < myCellTemplate.maxLifeProd)
        {
            StockageCapabilityVariation(1);
        }

        if (!overLoad)
        {
            overloadStack = 0;
            // ça marche bien mais à voir si quand 1 batiment meure la produciton saute avec ou pas
            //if ((int)Random.Range(0, 101) <= productionBonusRatio)
            //{
            //    BlobNumberVariation(myCellTemplate.prodPerTickBase, BlobManager.BlobType.normal, true);
            //}

            //int productionPerTick = myCellTemplate.prodPerTickBase * (1 + productionBonusPacket);

            //BlobNumberVariation(productionPerTick, BlobManager.BlobType.normal, true);
            haveExpulse = false;

            int nbrTransmission = 0;
            for (int i = 0; i < production; i++)
            {

                if (currentIndex < outputLinks.Count)
                {
                    outputLinks[currentIndex].Transmitt(1, BlobManager.BlobType.normal);
                    haveExpulse = true;
                    currentIndex++;
                    currentIndex = Helper.LoopIndex(currentIndex, outputLinks.Count);
                    nbrTransmission++;
                }
                else
                {
                    Debug.Log(currentIndex + "Reset Index" + "LinkCount" + outputLinks.Count);
                    currentIndex = 0;
                }

            }

            if (haveExpulse && anim != null)
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


    public override void StockageCapabilityVariation(int Amount)
    {
        Life += Amount;
        float ratio = (float)Life / (float)myCellTemplate.maxLifeProd;
        if (Life < myCellTemplate.maxLifeProd)
        {
            progressBar.ToggleRenderer(true);
            //Display Barre
        }
        else
        {
            progressBar.ToggleRenderer(false);
        }


        if (Life <= 0)
        {
            ToggleOverload(true);
        }
        if (Life > 0 && overLoad)
        {
            ToggleOverload(false);
        }
        HandleAlerts();
        progressBar.UpdateBar(ratio , true);

    }

    public override void BlobNumberVariation(int amount, BlobManager.BlobType _blobType, bool transmission)
    {
        switch (_blobType)
        {
            case BlobManager.BlobType.normal:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.normal, amount);
                normalBlobNumber += amount;
                break;

            case BlobManager.BlobType.coach:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                if (!transmission)
                {
                    if (amount < 0)
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            blobCoaches.Remove(blobCoaches[blobCoaches.Count - 1]);
                        }
                    }
                }
                break;

            case BlobManager.BlobType.explorateur:
                explorateurBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
                break;
        }
        blobNumber = normalBlobNumber + blobCoaches.Count + explorateurBlobNumber;
    }


    public void ProductriceProximityGestion(CellProximityDectection collider, bool enter)
    {
        ProductionVariationByProximity(collider.productionBonusRatio, enter);
    }

    private void ProductionVariationByProximity(int amount, bool addition)
    {
        int multiplier;
        multiplier = (addition == true) ? 1 : -1;

        productionBonusRatio += multiplier * amount;

        if (productionBonusRatio >= 100)
        {
            productionBonusPacket++;
            int reste = productionBonusRatio - 100;
            productionBonusRatio = 0;
            productionBonusRatio += reste;
        }
        if (productionBonusRatio < 0)
        {
            productionBonusPacket--;
            int reste = productionBonusRatio;
            productionBonusRatio = 100;
            productionBonusRatio += reste;
        }

    }

    public override void Died(bool intentionnalDeath)
    {
        productionBonusPacket = 0;
        productionBonusRatio = 0;
        base.Died(intentionnalDeath);
    }

    public override void SetupVariable()
    {
        Life = myCellTemplate.maxLifeProd;
        float ratio = (float)Life / (float)myCellTemplate.maxLifeProd;
        progressBar.transform.position = graphTransform.position;

        progressBar.UpdateBar(ratio, true);
        progressBar.ToggleRenderer(false);

        base.SetupVariable();
    }

}
