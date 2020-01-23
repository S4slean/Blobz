using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDivine : CellMain
{
    private bool isLoaded;
    private int energie;
    public divineCellButon activationButton;
    public ProgressBar chargeBar;

    public override void BlobsTick()
    {

        if (stuckBlobs.Count <= 0 && currentBlobStockage < myCellTemplate.storageCapability)
        {
            StockageCapabilityVariation(1);
        }


        if (!overLoad)
        {
            if (blobNumber > 0 && !isLoaded)
            {
                BlobNumberVariation(-1, BlobCheck(), false);
                Charge(1);
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
                overloadStack++;
                if (overloadStack >= myCellTemplate.overLoadTickMax)
                {
                    Died(false);
                }
            }
        }

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
            chargeBar.UpdateBar(ratio , false);
        }
    }
    public void Decharge()
    {
        energie = 0;
        isLoaded = false;
        activationButton.ToggleButton(false);
        float ratio = (float)energie / (float)myCellTemplate.maxEnergie;
        chargeBar.UpdateBar(ratio , false);

    }

    public override void SetupVariable()
    {
        energie = 0;
        isLoaded = false;
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

        if (isLoaded && !actionmade)
        {
            InputManager.Instance.shootingCell = this;
            InputManager.SwitchInputMode(InputManager.InputMode.divineShot);
            actionmade = true;
        }

        if (actionmade)
        {
            anim.Play("PlayerInteraction", 0, 0f);
        }

    }

    public override void ProximityLevelModification()
    {
        base.ProximityLevelModification();

        UIManager.Instance.UpdateShootingArea(specifiqueStats);
    }
}
