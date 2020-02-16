using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTreblochet : CellMain
{
    //public treblochetChargeur myChargeur;
    //public bool chargerIsFull;
    private bool isLoaded;
    private int energie;
    public CellButon activationButton;
    public ProgressBar chargeBar;
    public Vector3 flagPos;

    //public override void ProximityLevelModification()
    //{
    //    base.ProximityLevelModification();
    //    myChargeur.UpdateSpecificity(specifiqueStats);
    //}

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

            if (isLoaded && flagPos != Vector3.zero)
            {
                SpawnSoldier();
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

    private void SpawnSoldier()
    {
        Vector3 dir = (flagPos - transform.position).normalized;
        Blob soldier = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
        soldier.ChangeType(BlobManager.BlobType.soldier);
        soldier.transform.position = transform.position + dir * 1.5f + Vector3.up * 1.1f;
        soldier.transform.LookAt(flagPos);
        soldier.AssignFlagPos(flagPos);
        soldier.originCell = transform;
        soldier.Outpool();
        soldier.JumpForward();
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
    //private BlobManager.BlobType ChargerCustomBlobCheck()
    //{
    //    if (explorateurBlobNumber > 0)
    //    {
    //        if ((int)Random.Range(1, 101) <= 50)
    //        {
    //            return BlobManager.BlobType.explorateur;
    //        }
    //    }

    //    if (blobNumber - (blobCoaches.Count + explorateurBlobNumber) > 0)
    //    {
    //        return BlobManager.BlobType.soldier;
    //    }
    //    return BlobManager.BlobType.explorateur;
    //}

    //public override void blobAddCheckType(int amount, BlobManager.BlobType _blobType , bool transmission)
    //{
    //    switch (_blobType)
    //    {
    //        case BlobManager.BlobType.normal:
    //            RessourceTracker.instance.AddBlob(BlobManager.BlobType.normal, amount);
    //            normalBlobNumber += amount;
    //            break;

    //        case BlobManager.BlobType.soldier:
    //            RessourceTracker.instance.AddBlob(BlobManager.BlobType.normal, amount);
    //            normalBlobNumber += amount;
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

    //}

    //#endregion

    //public override void SetupVariable()
    //{

    //  //  myChargeur.Init();
    //  //  chargerIsFull = false;
    //    base.SetupVariable();


    //}

    public override void Died(bool intentionnalDeath)
    {
        base.Died(intentionnalDeath);
        //myChargeur.blobInChargeur.Clear();
    }

    public override void SetupVariable()
    {
        Decharge();
        base.SetupVariable();
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

        if (!actionmade)
        {
            InputManager.SwitchInputMode(InputManager.InputMode.flag, myCellTemplate.type);
            actionmade = true;
        }

        if (actionmade)
        {
            anim.Play("PlayerInteraction", 0, 0f);
        }


    }

}

