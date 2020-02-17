using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellExplo : CellMain
{
    private bool isLoaded;
    private int energie;
    public CellButon activationButton;
    public ProgressBar chargeBar;
   [HideInInspector] public Vector3 flagPos;


    public override void BlobsTick()
    {
        if (stuckBlobs.Count <= 0 && currentBlobStockage < myCellTemplate.storageCapability)
        {
            StockageCapabilityVariation(1);
        }


        if (!overLoad)
        {
            overloadStack = 0;
            if (blobNumber > 0 && !isLoaded)
            {
                BlobNumberVariation(-1, BlobCheck(), false);
                Charge(1);
            }
            BlobNumberVariation(myCellTemplate.prodPerTickBase, BlobManager.BlobType.normal, true);

            //ANIM
            haveExpulse = false;

            //Generation de Blob Auto
            if (isLoaded && flagPos != Vector3.zero )
            {
                SpawnExplo();
                Decharge();
            }


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

    private void SpawnExplo()
    {
        Vector3 dir = (flagPos - transform.position).normalized;
        Blob explo = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
        explo.ChangeType(BlobManager.BlobType.explorateur);
        explo.transform.position = transform.position + dir * 2.5f + Vector3.up * 1.1f;
        explo.transform.LookAt(flagPos);
        explo.AssignFlagPos(flagPos);
        explo.originCell = transform;
        explo.Outpool();
        explo.JumpForward();
    }

    private void Charge(int amount)
    {
        if (!isLoaded)
        {
            energie += amount;
            if (energie >= myCellTemplate.maxEnergie)
            {
                isLoaded = true;
                energie = myCellTemplate.maxEnergie;
                activationButton.ToggleButton(true);
            }
            float ratio = (float)energie / (float)myCellTemplate.maxEnergie;
            chargeBar.UpdateBar(ratio, false);
        }
    }

    public void Decharge()
    {
        energie = 0;
        isLoaded = false;
        activationButton.ToggleButton(false);
        float ratio = (float)energie / (float)myCellTemplate.maxEnergie;
        chargeBar.UpdateBar(ratio, false);

    }

    //#region ancien treblochet

    //public override void blobAddCheckType(int amount, BlobManager.BlobType _blobType, bool transmission)
    //{
    //    switch (_blobType)
    //    {
    //        case BlobManager.BlobType.normal:
    //            RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
    //            explorateurBlobNumber += amount;
    //            break;

    //        case BlobManager.BlobType.coach:
    //            RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
    //            if (!transmission)
    //            {
    //                if (amount < 0)
    //                {
    //                    for (int i = 0; i < amount; i++)
    //                    {

    //                        blobCoaches.Remove(blobCoaches[blobCoaches.Count - 1]);
    //                    }
    //                }
    //            }
    //            break;

    //        case BlobManager.BlobType.explorateur:
    //            explorateurBlobNumber += amount;
    //            RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
    //            break;
    //    }
    //    blobNumber = normalBlobNumber + blobCoaches.Count + explorateurBlobNumber;
    //    UpdateCaract();
    //}

    //public override void CheckForExplo(BlobManager.BlobType _blobType)
    //{
    //    if (explorateurBlobNumber <= 0)
    //    {
    //        //explorateurBlobNumber = 0;
    //        hasExplo = false;
    //        exploIcon.SetActive(false);
    //    }
    //    else
    //    {
    //        hasExplo = true;
    //        exploIcon.SetActive(true);
    //    }
    //}
    //#endregion

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

        if (!actionmade)
        {
            InputManager.SwitchInputMode(InputManager.InputMode.flag , myCellTemplate.type);
            actionmade = true;
        }

        if (actionmade)
        {
            anim.Play("PlayerInteraction", 0, 0f);
        }


    }

    public override void SetupVariable()
    {
        Decharge();
        base.SetupVariable();
    }
}
