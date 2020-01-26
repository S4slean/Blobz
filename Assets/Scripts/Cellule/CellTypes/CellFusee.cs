using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFusee : CellMain
{
    private int clickforLauchn;
    public ProgressBar progressBar;
    private int Life;
    private bool isFullCharged;


    public override void BlobsTick()
    {
        if (stuckBlobs.Count <= 0 && Life < myCellTemplate.maxLifeProd)
        {
            StockageCapabilityVariation(1);
        }

        if (blobNumber > 0 && !isFullCharged)
        {
            BlobNumberVariation(-myCellTemplate.blobLostPerTick, BlobCheck(), false);
            //CellManager.Instance.EnergyVariation(currentEnergyPerClick);
            RessourceTracker.instance.EnergyVariation(currentEnergyPerClick);
        }

        if (overLoad)
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

    public override void OnShortLeftClickUp(RaycastHit hit)
    {
        actionmade = false;
        if (stuckBlobs.Count > 0)
        {
            stuckBlobs[stuckBlobs.Count - 1].Unstuck();
            actionmade = true;
        }
        else if (overLoad)
        {
            BlobNumberVariation(-1, BlobCheck(), false);
            actionmade = true;
        }

        else if (isFullCharged)
        {
            clickforLauchn++;
            //Play FX 
            if (clickforLauchn > myCellTemplate.clickBeforeLaunch)
            {
                LevelManager.instance.LevelSuccessed();
                Debug.Log("c'est win");
            }
            actionmade = true;
        }

        if (actionmade)
        {
            anim.Play("PlayerInteraction", 0, 0f);
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
        progressBar.UpdateBar(ratio, true);

    }

    public override void BlobNumberVariation(int amount, BlobManager.BlobType _blobType, bool transmission)
    {

        blobAddCheckType(amount, _blobType, transmission);

        CheckForCoach(_blobType);

        CheckForExplo(_blobType);


        if (blobNumber > currentBlobStockage && !isDead)
        {
            isFullCharged = true;
        }


        if (!overLoad)
        {
            float ratio = 0;
            ratio = (float)blobNumber / (float)currentBlobStockage;
            stockageBar.UpdateBar(ratio, true);
        }


        HandleAlerts();

        //Nexus 
        if (isNexus)
        {
            BlobAmountCheck(amount, _blobType);
        }
        UpdateCaract();

    }

    public override void SetupVariable()
    {

        isFullCharged = false;
        clickforLauchn = 0;

        Life = myCellTemplate.maxLifeProd;
        float ratio = (float)Life / (float)myCellTemplate.maxLifeProd;
        progressBar.transform.position = graphTransform.position;
        progressBar.UpdateBar(ratio, true);
        progressBar.ToggleRenderer(false);


        base.SetupVariable();
    }

    protected override void ToggleOverload(bool isOverload)
    {
        overLoad = isOverload;
        if (stockageBar != null)
        {
            stockageBar.ToggleRenderer(!isOverload);
        }
        if (isOverload)
        {
            if (isFullCharged)
            {
                clickforLauchn = 0;
                isFullCharged = false;
            }

            //Debug.Log("enterInOverload ", gameObject);
            blolbNumberAtOverload = blobNumber;
            if (onOverloadEvent != null)
            {
                onOverloadEvent.Invoke();
            }

        }
    }
}
