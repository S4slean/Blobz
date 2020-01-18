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
                for (int i = 0; i < outputLinks.Count; i++)
                {
                    if (blobNumber <= 0)
                    {
                        break;
                    }
                    //Pour l'instant il y a moyen que si une cellule creve la prochaine 
                    //soit sauté mai squand il y aura les anim , ce sera plus possible
                    outputLinks[i].Transmitt(1, BlobCheck());
                    haveExpulse = true;
                }
                currentTick = 0;
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
            overloadStack++;
            if (overloadStack >= myCellTemplate.overLoadTickMax)
            {
                Died(false);
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
            chargeBar.UpdateBar(ratio);
        }
    }
    public void Decharge()
    {
        energie = 0;
        isLoaded = false;
        activationButton.ToggleButton(false);
        float ratio = (float)energie / (float)myCellTemplate.maxEnergie;
        chargeBar.UpdateBar(ratio);

    }

    public override void SetupVariable()
    {
        energie = 0;
        isLoaded = false;
        base.SetupVariable();
    }

    public override void OnShortLeftClickUp(RaycastHit hit)
    {
        if (isLoaded)
        {
            InputManager.Instance.shootingCell = this;
            InputManager.SwitchInputMode(InputManager.InputMode.divineShot);
            //passer la reference de l'input de tir 
        }
        else
        {
            //dipslay not enough charge 
        }
    }

    public override void ProximityLevelModification()
    {
        base.ProximityLevelModification();

        UIManager.Instance.UpdateShootingArea(specifiqueStats);
    }
}
